using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;
using PCLCrypto;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsLazySingleton]
    public class ProfileProvider : IProfileProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private UserProfile _currentUser;

        public ProfileProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProfile> GetUserProfile()
        {
            if (_currentUser == null || !_currentUser.LoggedIn)
            {
                var currentAuthenticatedUsers = await _unitOfWork.AuthenticationObjects.GetAsync().ConfigureAwait(false);
                var currentAuthenticatedUser = currentAuthenticatedUsers.FirstOrDefault();
                if (currentAuthenticatedUser != null)
                {
                    _currentUser = await _unitOfWork.UserProfiles.GetAsync(x => x.Email == currentAuthenticatedUser.Email).ConfigureAwait(false);
                }
            }
            return _currentUser;
        }

        public async Task CreateNewProfile()
        {
            var users = await _unitOfWork.AuthenticationObjects.GetAsync().ConfigureAwait(false);
            var user = users?.FirstOrDefault();
            if (await _unitOfWork.UserProfiles.GetAsync(x => x.Email == user.Email).ConfigureAwait(false) != null)
            {
                return;
            }
            UserProfile newProfile = new UserProfile
            {
                Email = user?.Email,
                BlockchainInfo = new BlockchainInfo()
            };
            newProfile.LoggedIn = false;
            await SaveUserProfile(newProfile).ConfigureAwait(false);
        }

        public Task<bool> HasProfile()
        {
            return _unitOfWork.UserProfiles.AnyAsync();
        }

        public async Task SaveUserProfile(UserProfile userProfile)
        {
            _currentUser = userProfile;
            await _unitOfWork.UserProfiles.SaveAsync(userProfile, true).ConfigureAwait(false);
        }

        static string GenerateSalt()
        {
            var cryptoRandomBuffer = new byte[16];
            NetFxCrypto.RandomNumberGenerator.GetBytes(cryptoRandomBuffer);
            return Convert.ToBase64String(cryptoRandomBuffer);
        }

        private static string GenerateHash(string input, string salt)
        {
            var data = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary($"{salt}{input}", Encoding.UTF8);
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            var hash = hasher.HashData(data);
            return Convert.ToBase64String(hash);
        }

        private static string HashValue(string val, UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return null;
            }
            if (userProfile.DataAccesSalt == null)
            {
                userProfile.DataAccesSalt = GenerateSalt();
            }

            return GenerateHash(val, userProfile.DataAccesSalt);
        }

        public bool VerifyPincode(string input, UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return false;
            }
            return userProfile.PinCode == HashValue(input, userProfile);
        }

        public async Task Logout(bool withRest)
        {
            await LogoutAllUsers().ConfigureAwait(false);
            var profile = await GetUserProfile().ConfigureAwait(false);
            profile.LoggedIn = false;
            profile.NeedsReset |= withRest;
            await SaveUserProfile(profile).ConfigureAwait(false);
            _currentUser = null;
        }

        private async Task LogoutAllUsers()
        {
           var users = await _unitOfWork.UserProfiles.GetAllAsync(x => true).ConfigureAwait(false);
            foreach(var user in users)
            {
                user.LoggedIn = false;
                await SaveUserProfile(user).ConfigureAwait(false);
            }
        }

        public async Task Login()
        {
            await LogoutAllUsers().ConfigureAwait(false);
            var profile = await GetUserProfile().ConfigureAwait(false);
            profile.LoggedIn = true;
            profile.InvalidPincodeAttempts = 0;
            await SaveUserProfile(profile).ConfigureAwait(false);
        }

        public async Task SetPinCode(string pincode, bool? enableFingerPrint)
        {
            var profile = await GetUserProfile().ConfigureAwait(false);
            if(profile == null)
            {
                return;
            }
            profile.PinCode = HashValue(pincode, profile);
            profile.NeedsReset = false;
            if(enableFingerPrint != null)
            {
                profile.UseBiometricsToLogin = enableFingerPrint.Value;
            }
            await SaveUserProfile(profile).ConfigureAwait(false);
        }

		public async Task<string> GetLatestPreferedLanguage()
		{
			var profiles = await _unitOfWork.UserProfiles.GetAsync().ConfigureAwait(false);
            if(profiles.Any())
			{
				return profiles.OrderByDescending(x => x.LastUserSession)?.FirstOrDefault()?.PreferredLanguage;
			}
			return null;
		}
	}
}

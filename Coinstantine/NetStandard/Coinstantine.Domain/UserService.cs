using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Mapping;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class UserService : IUserService
    {
        private readonly IBackendService _backendService;
        private readonly IProfileProvider _profileProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper<ApiUser, UserProfile> _userProfileMapper;

        public UserService(IBackendService backendService,
                           IProfileProvider profileProvider,
                           IMapperFactory mapperFactory,
                           IUnitOfWork unitOfWork)
        {
            _backendService = backendService;
            _profileProvider = profileProvider;
            _unitOfWork = unitOfWork;
            _userProfileMapper = mapperFactory.GetMapper<ApiUser, UserProfile>();
        }

        public async Task<bool> CancelBitcoinTalkProfile(string userId)
        {
            if (await _backendService.CancelBitcoinTalkProfile(userId).ConfigureAwait(false))
            {
                await DeleteBitcoinTalkProfile().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<bool> CancelTelegramProfile(string username)
        {
            if (await _backendService.CancelTelegramProfile(username).ConfigureAwait(false))
            {
                await DeleteTelegramProfile().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<bool> CancelTwitterProfile(long twitterId)
        {
            if (await _backendService.CancelTwitterProfile(twitterId).ConfigureAwait(false))
            {
                await DeleteTwitterProfile().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public Task<(BitcoinTalkProfile, bool)> GetBitcoinTalkProfile(string userId)
        {
            return _backendService.GetBitcoinTalkProfile(userId);
        }

        public Task<(TelegramProfile, bool)> GetTelegramProfile(string username)
        {
            return _backendService.GetTelegramProfile(username);
        }

        public Task<(TwitterProfile, bool)> GetTwitterProfile(long tweetId)
        {
            return _backendService.GetTwitterProfile(tweetId);
        }

        public async Task<(BitcoinTalkProfile, bool)> SetBitcoinTalkProfile(string userId)
        {
            var (bitcoiTalkProfile, success) = await _backendService.SetBitcoinTalkProfile(userId).ConfigureAwait(false);
            if (success)
            {
                await UpdateBctUserProfile(bitcoiTalkProfile).ConfigureAwait(false);
            }
            return (bitcoiTalkProfile, success);
        }

        public async Task<(TelegramProfile, bool)> SetTelegramProfile(string username)
        {
            var (telegramProfile, success) = await _backendService.SetTelegramProfile(username).ConfigureAwait(false);
            if (success)
            {
                await UpdateTelegramUserProfile(telegramProfile).ConfigureAwait(false);
            }
            return (telegramProfile, success);
        }

        public async Task<(TwitterProfile, bool)> SetTwitterProfile(TwitterProfile twitterProfile)
        {
            var result = await _backendService.SetTwitterProfile(twitterProfile).ConfigureAwait(false);
            if (result.Success)
            {
                await UpdateTwitterUserProfile(result.TwitterProfile).ConfigureAwait(false);
            }
            return result;
        }

        public async Task<bool> SetUsername(string username)
        {
            var (ApiUser, SucceededCreation) = await _backendService.SetUsername(username).ConfigureAwait(false);
            if (SucceededCreation)
            {
                await UpdateProfile(ApiUser).ConfigureAwait(false);
            }

            return SucceededCreation;
        }

        public Task StartTelegramConversation(string username)
        {
            return _backendService.StartTelegramConversation(username);
        }

        public async Task<(BitcoinTalkProfile, bool)> UpdateBitcoinTalkProfile(string userId)
        {
            var (bitcoiTalkProfile, success) = await _backendService.UpdateBitcoinTalkProfile(userId).ConfigureAwait(false);
            if (success)
            {
                await UpdateBctUserProfile(bitcoiTalkProfile).ConfigureAwait(false);
            }
            return (bitcoiTalkProfile, success);
        }

        public async Task<(TwitterProfile, bool)> UpdateTwitterProfile(long twitterId)
        {
            var (twitterProfile, success) = await _backendService.UpdateTwitterProfile(twitterId).ConfigureAwait(false);
            if (success)
            {
                await UpdateTwitterUserProfile(twitterProfile).ConfigureAwait(false);
            }
            return (twitterProfile, success);
        }

        private async Task DeleteTwitterProfile()
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            await _unitOfWork.TwitterProfiles.DeleteAsync(userProfile.TwitterProfile).ConfigureAwait(false);
            userProfile.TwitterProfile = null;
            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
        }

        private async Task DeleteTelegramProfile()
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            await _unitOfWork.TelegramProfiles.DeleteAsync(userProfile.TelegramProfile).ConfigureAwait(false);
            userProfile.TelegramProfile = null;
            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
        }

        private async Task DeleteBitcoinTalkProfile()
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            await _unitOfWork.BitcoinTalkProfiles.DeleteAsync(userProfile.BitcoinTalkProfile).ConfigureAwait(false);
            userProfile.BitcoinTalkProfile = null;
            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
        }

        private async Task UpdateTwitterUserProfile(TwitterProfile twitterProfile)
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);

            twitterProfile.Id = userProfile?.TwitterProfileId ?? 0;
            userProfile.TwitterProfile = twitterProfile;

            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
        }

        private async Task UpdateBctUserProfile(BitcoinTalkProfile bitcoinTalkProfile)
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);

            bitcoinTalkProfile.Id = userProfile?.BitcoinTalkProfileId ?? 0;
            userProfile.BitcoinTalkProfile = bitcoinTalkProfile;

            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
        }

        private async Task UpdateTelegramUserProfile(TelegramProfile telegramProfile)
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);

            telegramProfile.Id = userProfile?.TelegramProfileId ?? 0;
            userProfile.TelegramProfile = telegramProfile;

            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
        }

        public async Task<UserProfile> UpdateProfile(ApiUser apiUser)
        {
            await _profileProvider.CreateNewProfile().ConfigureAwait(false);
            var userProfile = _userProfileMapper.Map(apiUser);

            var currentProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            userProfile.Id = currentProfile?.Id ?? 0;

            userProfile.TwitterProfileId = currentProfile?.TwitterProfileId;

            userProfile.TelegramProfileId = currentProfile?.TelegramProfileId;

            userProfile.BitcoinTalkProfileId = currentProfile?.BitcoinTalkProfileId;

            userProfile.PhonenumberProfileId = currentProfile?.PhonenumberProfileId;

            userProfile.BlockchainInfoId = currentProfile?.BlockchainInfoId;

            if (currentProfile != null)
            {
                userProfile.DataAccesSalt = currentProfile.DataAccesSalt;
                userProfile.InvalidPincodeAttempts = currentProfile.InvalidPincodeAttempts;
                userProfile.LoggedIn = currentProfile.LoggedIn;
                userProfile.NeedsReset = currentProfile.NeedsReset;
                userProfile.PinCode = currentProfile.PinCode;
                userProfile.UseBiometricsToLogin = currentProfile.UseBiometricsToLogin;
                userProfile.PreferredLanguage = currentProfile.PreferredLanguage;
                userProfile.LastSyncSession = currentProfile.LastSyncSession;
                userProfile.LastUserSession = currentProfile.LastUserSession;

                if (userProfile.TwitterProfile == null)
                {
                    if (!currentProfile?.TwitterProfile?.Validated ?? true)
                    {
                        userProfile.TwitterProfile = currentProfile?.TwitterProfile;
                    }
                }
                else
                {
                    userProfile.TwitterProfile.Id = currentProfile?.TwitterProfileId ?? 0;
                }
                if (userProfile.TelegramProfile == null)
                {
                    if (!currentProfile?.TelegramProfile?.Validated ?? true)
                    {
                        userProfile.TelegramProfile = currentProfile?.TelegramProfile;
                    }
                }
                else
                {
                    userProfile.TelegramProfile.Id = currentProfile?.TelegramProfileId ?? 0;
                }
                if (userProfile.BitcoinTalkProfile == null)
                {
                    if (!currentProfile?.BitcoinTalkProfile?.Validated ?? true)
                    {
                        userProfile.BitcoinTalkProfile = currentProfile?.BitcoinTalkProfile;
                    }
                }
                else
                {
                    userProfile.BitcoinTalkProfile.Id = currentProfile?.BitcoinTalkProfileId ?? 0;
                }
                if (userProfile.BlockchainInfo == null)
                {
                    userProfile.BlockchainInfo = currentProfile?.BlockchainInfo;
                }
                else
                {
                    userProfile.BlockchainInfo.Id = currentProfile?.BlockchainInfoId ?? 0;
                }

            }
            else
            {
                if (userProfile.TwitterProfile == null)
                {
                    userProfile.TwitterProfile = new TwitterProfile();
                }
                if (userProfile.BitcoinTalkProfile == null)
                {
                    userProfile.BitcoinTalkProfile = new BitcoinTalkProfile();
                }
                if (userProfile.TelegramProfile == null)
                {
                    userProfile.TelegramProfile = new TelegramProfile();
                }
                if (userProfile.BlockchainInfo == null)
                {
                    userProfile.BlockchainInfo = new BlockchainInfo
                    {
                        Id = currentProfile?.Id ?? 0,
                        Address = currentProfile?.BlockchainInfo?.Address
                    };
                }
            }

            await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
            return userProfile;
        }
    }
}

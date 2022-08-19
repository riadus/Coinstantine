using System;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface IProfileProvider
    {
        Task SaveUserProfile(UserProfile userProfile);
        Task<UserProfile> GetUserProfile();
        Task<bool> HasProfile();
        bool VerifyPincode(string input, UserProfile userProfile);
        Task Logout(bool withReset);
        Task Login();
        Task SetPinCode(string input, bool? enableFingerPrint);
        Task<string> GetLatestPreferedLanguage();
        Task CreateNewProfile();
    }
}

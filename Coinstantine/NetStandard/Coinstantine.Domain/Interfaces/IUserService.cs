using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface IUserService
    {
        Task<bool> SetUsername(string username);

        Task StartTelegramConversation(string username);
        Task<(TelegramProfile TelegramProfile, bool Success)> GetTelegramProfile(string username);
        Task<(TelegramProfile TelegramProfile, bool Success)> SetTelegramProfile(string username);
        Task<bool> CancelTelegramProfile(string username);

        Task<(BitcoinTalkProfile BitcoinTalkProfile, bool Success)> GetBitcoinTalkProfile(string userId);
        Task<(BitcoinTalkProfile BitcoinTalkProfile, bool Success)> UpdateBitcoinTalkProfile(string userId);
        Task<(BitcoinTalkProfile BitcoinTalkProfile, bool Success)> SetBitcoinTalkProfile(string userId);
        Task<bool> CancelBitcoinTalkProfile(string userId);

        Task<(TwitterProfile TwitterProfile, bool Success)> GetTwitterProfile(long tweetId);
        Task<(TwitterProfile TwitterProfile, bool Success)> SetTwitterProfile(TwitterProfile twitterProfile);
        Task<(TwitterProfile TwitterProfile, bool Success)> UpdateTwitterProfile(long twitterId);
        Task<bool> CancelTwitterProfile(long twitterId);
        Task<UserProfile> UpdateProfile(ApiUser apiUser);
    }
}

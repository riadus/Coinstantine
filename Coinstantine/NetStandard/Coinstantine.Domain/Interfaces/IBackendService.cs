using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Interfaces
{
    public interface IBackendService
    {
        Task<(ApiUser ApiUser, bool CreationSucceeded)> SetUsername(string username);
        Task<ApiUser> GetOnlineUserProfile();
        Task<IEnumerable<Translation>> GetTranslations();
        Task<BlockchainInfo> GetBlockchainInfo();

        Task<Price> GetEthPrice();
        Task<EnvironmentInfo> GetEnvironmentInfo();
        Task<SmartContractDefinition> GetSmartContractDefinition(string name);
        Task<Purchase> BuyPresale(string ethAddress, decimal ethToInvest);
        Task<IEnumerable<BuyingReceipt>> GetPurchases();
        Task<IEnumerable<AirdropDefinition>> GetCurrentAirdrops();
        Task<UserAirdrop> GetUserAirdrops();
        Task<AirdropSubscriptionResult> SubscribeToAirdrop(int airdropId);

        Task<(TwitterProfile TwitterProfile, bool Success)> GetTwitterProfile(long twitterId);
        Task<(TwitterProfile TwitterProfile, bool Success)> SetTwitterProfile(TwitterProfile twitterProfile);
        Task<(TwitterProfile TwitterProfile, bool Success)> UpdateTwitterProfile(long twitterId);
        Task<bool> CancelTwitterProfile(long twitterId);

        Task<(BitcoinTalkProfile BitcoinTalkProfile, bool Success)> GetBitcoinTalkProfile(string userId);
        Task<(BitcoinTalkProfile BitcoinTalkProfile, bool Success)> SetBitcoinTalkProfile(string userId);
        Task<(BitcoinTalkProfile BitcoinTalkProfile, bool Success)> UpdateBitcoinTalkProfile(string userId);
        Task<bool> CancelBitcoinTalkProfile(string userId);

        Task StartTelegramConversation(string username);
        Task<(TelegramProfile TelegramProfile, bool Success)> GetTelegramProfile(string username);
        Task<(TelegramProfile TelegramProfile, bool Success)> SetTelegramProfile(string username);
        Task<bool> CancelTelegramProfile(string username);

        Task<string> WithdrawBalance(string fromAddress, string toAddress);

        Task<IEnumerable<DocumentVersion>> GetDocuments();
        Task<Document> DownloadDocument(DocumentVersion documentVersion);
        Task<DocumentVersion> GetInformation(DocumentType documentType);
    }
}

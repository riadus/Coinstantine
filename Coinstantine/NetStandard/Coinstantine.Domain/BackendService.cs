using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Mapping;
using Coinstantine.Domain.Mapping.DTOs;
using Plugin.Connectivity.Abstractions;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsLazySingleton]
    public class BackendService : IBackendService
    {
        private readonly IApiClient _apiClient;
		private readonly IConnectivity _connectivity;
        private readonly IMapperFactory _mapperFactory;

        public BackendService(IApiClient apiClient,
                              IConnectivity connectivity,
                              IMapperFactory mapperFactory)
        {
            _apiClient = apiClient;
            _connectivity = connectivity;
            _mapperFactory = mapperFactory;
        }

		private void CheckConnectivity()
		{
			if (!_connectivity.IsConnected)
			{
				//throw new NotConnectedException();
			}
		}

        public async Task<(ApiUser, bool)> SetUsername(string username)
        {
			CheckConnectivity();
            var apiUserDTO = new ApiUserDTO
            {
                Username = username
            };
            try
            {
                var apiUserReponse = await _apiClient.PostAsync<ApiUser, ApiUserDTO>("users/username", apiUserDTO).ConfigureAwait(false);
                return (apiUserReponse, apiUserReponse != null);
            }
            catch(Exception)
            {
                return (default(ApiUser), false);
            }
        }

		public async Task<ApiUser> GetOnlineUserProfile()
		{
			CheckConnectivity();
			var response = await _apiClient.GetAsync<ApiUser>("users").ConfigureAwait(false);
			return response;
		}

		public async Task<IEnumerable<Translation>> GetTranslations()
		{
			CheckConnectivity();
			var translations = await _apiClient.GetAsync<TranslationsDTO>("translations").ConfigureAwait(false);
			if(translations.NumberOfTranslations > 0)
			{
                var translationMapper = _mapperFactory.GetMapper<TranslationDTO, Translation>();
                return translations.Translations.SelectMany(x => x.LanguageTranslations).Select(x => translationMapper.Map(x));
			}
            return new List<Translation>();

        }

		public async Task<BlockchainInfo> GetBlockchainInfo()
        {
            CheckConnectivity();
            try
            {
                return await _apiClient.GetAsync<BlockchainInfo>($"sale/balance").ConfigureAwait(false);
            }
            catch (Exception)
            {
                return default(BlockchainInfo);
            }
        }

        public async Task<Price> GetEthPrice()
        {
            CheckConnectivity();
            var etherPriceMapper = _mapperFactory.GetMapper<PriceDTO, Price>();
            var price = await _apiClient.GetAsync<PriceDTO>($"sale/ether").ConfigureAwait(false);
            if(price == null)
            {
                return default(Price);
            }
            return etherPriceMapper.Map(price);
        }

        public async Task<EnvironmentInfo> GetEnvironmentInfo()
        {
            CheckConnectivity();
            var environmentMapper = _mapperFactory.GetMapper<EnvironmentDTO, EnvironmentInfo>();
            var environment = await _apiClient.GetAsync<EnvironmentDTO>("presale/web3").ConfigureAwait(false);
            if(environment == null)
            {
                return default(EnvironmentInfo);
            }
            return environmentMapper.Map(environment);
        }

        public async Task<SmartContractDefinition> GetSmartContractDefinition(string name)
        {
            CheckConnectivity();
            var dto = await _apiClient.GetAsync<SmartContractDefinitionDTO>($"presale/smartContractDefinition/{name}").ConfigureAwait(false);
            if(dto == null)
            {
                return default(SmartContractDefinition);
            }
            return new SmartContractDefinition
            {
                Abi = dto.Abi,
                Address = dto.Address,
                Name = dto.Name
            };
        }

        public async Task<Purchase> BuyPresale(string ethAddress, decimal ethToInvest)
        {
            CheckConnectivity();
            var buyTokensDTO = new BuyTokensDTO
            {
                Address = ethAddress,
                Amount = ethToInvest
            };
            var mapper = _mapperFactory.GetMapper<PurchaseDTO, Purchase>();
            var purchaseDTO = await _apiClient.PostAsync<PurchaseDTO, BuyTokensDTO>($"presale/buy", buyTokensDTO).ConfigureAwait(false);
            if(purchaseDTO == null)
            {
                return default(Purchase);
            }
            return mapper.Map(purchaseDTO);
        }

        public async Task<IEnumerable<AirdropDefinition>> GetCurrentAirdrops()
        {
            CheckConnectivity();
            var dtos = await _apiClient.GetAsync<IEnumerable<AirdropSubscriptionDTO>>("airdrops/current").ConfigureAwait(false);
            var airdropDefinitionMapper = _mapperFactory.GetMapper<AirdropDefinitionDTO, AirdropDefinition>();
            if(dtos == null)
            {
                return default(IEnumerable<AirdropDefinition>);
            }
            return dtos.Select(dto =>
            {
                var airdropDefinition = airdropDefinitionMapper.Map(dto.AirdropDefinition);
                airdropDefinition.NumberOfSubscribers = dto.Count;
                return airdropDefinition;
            });
        }

        public async Task<UserAirdrop> GetUserAirdrops()
        {
            CheckConnectivity();
            var dto = await _apiClient.GetAsync<UserAirdropDTO>("airdrops/current/mine").ConfigureAwait(false);
            if(dto == null)
            {
                return default(UserAirdrop);
            }
            var userAirdropMapper = _mapperFactory.GetMapper<UserAirdropDTO, UserAirdrop>();
            return userAirdropMapper.Map(dto);
        }

        public async Task<AirdropSubscriptionResult> SubscribeToAirdrop(int airdropId)
        {
            CheckConnectivity();
            var dto = await _apiClient.PostAsync<AirdropSubscriptionResultDTO, object>($"airdrops/current/{airdropId}", null).ConfigureAwait(false);
            if(dto == null)
            {
                return default(AirdropSubscriptionResult);
            }
            var userAirdropMapper = _mapperFactory.GetMapper<AirdropSubscriptionResultDTO, AirdropSubscriptionResult>();
            return userAirdropMapper.Map(dto);
        }

        public async Task<IEnumerable<BuyingReceipt>> GetPurchases()
        {
            CheckConnectivity();
            var mapper = _mapperFactory.GetMapper<BuyingReceiptDTO, BuyingReceipt>();
            var dtos = await _apiClient.GetAsync<IEnumerable<BuyingReceiptDTO>>($"presale/purchases").ConfigureAwait(false);
            if(dtos == null)
            {
                return default(IEnumerable<BuyingReceipt>);
            }
            return dtos.Select(mapper.Map);
        }

        private (TwitterProfile, bool) HandleTwitterDTO(TwitterProfileOnline twitterProfileOnline)
        {
            if (twitterProfileOnline == null)
            {
                return (default(TwitterProfile), false);
            }
            var twitterMapper = _mapperFactory.GetMapper<TwitterProfileOnline, TwitterProfile>();
            return (twitterMapper.Map(twitterProfileOnline), true);
        }

        public async Task<(TwitterProfile, bool)> GetTwitterProfile(long twitterId)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.GetAsync<TwitterProfileOnline>($"twitter/{twitterId}").ConfigureAwait(false);
            return HandleTwitterDTO(profileDTO);
        }

        public async Task<(TwitterProfile, bool)> SetTwitterProfile(TwitterProfile twitterProfile)
        {
            CheckConnectivity();
            var twitterMapper = _mapperFactory.GetMapper<TwitterProfile, TwitterProfileDTO>();
            var twitterProfileDto = twitterMapper.Map(twitterProfile);

            var profileDTO = await _apiClient.PostAsync<TwitterProfileOnline, TwitterProfileDTO>($"twitter/{twitterProfile.TweetId}", twitterProfileDto).ConfigureAwait(false);
            return HandleTwitterDTO(profileDTO);
        }

        public async Task<(TwitterProfile, bool)> UpdateTwitterProfile(long twitterId)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.PutAsync<TwitterProfileOnline, object>($"twitter/{twitterId}", null).ConfigureAwait(false);
            return HandleTwitterDTO(profileDTO);
        }

        public Task<bool> CancelTwitterProfile(long twitterId)
        {
            CheckConnectivity();
            return _apiClient.DeleteAsync($"twitter/{twitterId}");
        }

        private (BitcoinTalkProfile, bool) HandleBitcoinTalkDTO(BitcoinTalkUserOnline bctOnline, string userId)
        {
            if (bctOnline == null)
            {
                return (default(BitcoinTalkProfile), false);
            }
            var bctMapper = _mapperFactory.GetMapper<BitcoinTalkUserOnline, BitcoinTalkProfile>();
            var bctProfile = bctMapper.Map(bctOnline);
            bctProfile.UserId = userId;
            return (bctProfile, true);
        }

        public async Task<(BitcoinTalkProfile, bool)> GetBitcoinTalkProfile(string userId)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.GetAsync<BitcoinTalkUserOnline>($"bitcointalk/{userId}").ConfigureAwait(false);
            return HandleBitcoinTalkDTO(profileDTO, userId);
        }

        public async Task<(BitcoinTalkProfile, bool)> SetBitcoinTalkProfile(string userId)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.PostAsync<BitcoinTalkUserOnline, object>($"bitcointalk/{userId}", null).ConfigureAwait(false);
            return HandleBitcoinTalkDTO(profileDTO, userId);
        }

        public async Task<(BitcoinTalkProfile, bool)> UpdateBitcoinTalkProfile(string userId)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.PutAsync<BitcoinTalkUserOnline, object>($"bitcointalk/{userId}", null).ConfigureAwait(false);
            return HandleBitcoinTalkDTO(profileDTO, userId);
        }

        public Task<bool> CancelBitcoinTalkProfile(string userId)
        {
            CheckConnectivity();
            return _apiClient.DeleteAsync($"bitcointalk/{userId}");
        }

        private (TelegramProfile, bool) HandleTelegramDTO(TelegramProfileOnline telegramProfileOnline)
        {
            if (telegramProfileOnline == null)
            {
                return (default(TelegramProfile), false);
            }
            var telegramMapper = _mapperFactory.GetMapper<TelegramProfileOnline, TelegramProfile>();
            var telegramProfile = telegramMapper.Map(telegramProfileOnline);
            return (telegramProfile, true);
        }

        public Task StartTelegramConversation(string username)
        {
            CheckConnectivity();
            return _apiClient.PostEmptyAsync($"telegram/{username}/startConversation");
        }

        public async Task<(TelegramProfile, bool)> GetTelegramProfile(string username)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.GetAsync<TelegramProfileOnline>($"telegram/{username}").ConfigureAwait(false);
            return HandleTelegramDTO(profileDTO);
        }

        public async Task<(TelegramProfile, bool)> SetTelegramProfile(string username)
        {
            CheckConnectivity();
            var profileDTO = await _apiClient.PostAsync<TelegramProfileOnline, object>($"telegram/{username}", null).ConfigureAwait(false);
            return HandleTelegramDTO(profileDTO);
        }

        public Task<bool> CancelTelegramProfile(string username)
        {
            CheckConnectivity();
            return _apiClient.DeleteAsync($"telegram/{username}");
        }

        public async Task<string> WithdrawBalance(string fromAddress, string toAddress)
        {
            CheckConnectivity();
            var sendFundsDto = new SendFundsDTO
            {
                FromAddress = fromAddress,
                ToAddress = toAddress
            };
            var receipt = await _apiClient.PostAsync<WithdrawalReceiptDTO, SendFundsDTO>("ethereum/send", sendFundsDto).ConfigureAwait(false);
            return receipt?.TransactionHash;
        }

        private readonly List<(string Endpoint, DocumentType DocumentType)> _documentsList = new List<(string Endpoint, DocumentType DocumentType)>{
                ("whitepaper", DocumentType.WhitePaper),
                ("privacypolicy", DocumentType.PrivacyPolicy),
                ("termsandservices", DocumentType.TermsAndServices)
            };

        public Task<IEnumerable<DocumentVersion>> GetDocuments()
        {
            CheckConnectivity();
            return _apiClient.GetAsync<IEnumerable<DocumentVersion>>($"documents/");
        }

        public async Task<Document> DownloadDocument(DocumentVersion documentVersion)
        {
            var bytes = await _apiClient.GetBytesAsync($"documents/{documentVersion.DocumentType}/file").ConfigureAwait(false);
            return new Document
            {
                Bytes = bytes,
                DocumentAvailable = documentVersion.DocumentAvailable,
                DocumentType = documentVersion.DocumentType,
                Filename = documentVersion.Filename,
                FileType = documentVersion.FileType.ApplicationType,
                LastModifiedDate = documentVersion.LastModifiedDate ?? new DateTime()
            };
        }

        public async Task<DocumentVersion> GetInformation(DocumentType documentType)
        {
            CheckConnectivity();
            return await _apiClient.GetAsync<DocumentVersion>($"documents/{documentType}").ConfigureAwait(false);
        }
    }

	public class NotConnectedException : Exception
    {
        public NotConnectedException()
        {
        }

        public NotConnectedException(string message) : base(message)
        {
        }

        public NotConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

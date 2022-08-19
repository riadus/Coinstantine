using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Airdrops;

namespace Coinstantine.Domain.Airdrops
{
    [RegisterInterfaceAsDynamic]
    public class AirdropDefinitionsService : IAirdropDefinitionsService
    {
        private readonly IBackendService _backendService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfileProvider _profileProvider;

        public AirdropDefinitionsService(IBackendService backendService,
                                         IUnitOfWork unitOfWork,
                                         IProfileProvider profileProvider)
        {
            _backendService = backendService;
            _unitOfWork = unitOfWork;
            _profileProvider = profileProvider;
        }

        public Task<IEnumerable<AirdropDefinition>> GetAirdropDefinitions()
        {
            return _unitOfWork.AirdropDefinitions.GetAsync();
        }

        public async Task<UserAirdrop> GetUserAirdrop()
        {
            var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            return await _unitOfWork.UserAirdrops.GetAsync(x => x.UserId == user.Id).ConfigureAwait(false);
        }

        public async Task<bool> SyncAirdropDefinitions()
        {
            var definitions = await _backendService.GetCurrentAirdrops().ConfigureAwait(false);
            var userAidrop = await _backendService.GetUserAirdrops().ConfigureAwait(false);
            if(definitions?.Any() ?? false)
            {
                await _unitOfWork.AirdropDefinitions.ReplaceAll(definitions).ConfigureAwait(false);
            }
            if(userAidrop != null)
            {
                var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
                userAidrop.UserId = user.Id;
                await _unitOfWork.UserAirdrops.ReplaceAll(userAidrop);
            }
            return true;
        }

        public async Task<(bool Success, FailReason FailReason)> SubscribeToAirdrop(AirdropDefinition airdropDefinition)
        {
            var subscriptionResult = await _backendService.SubscribeToAirdrop(airdropDefinition.AirdropId).ConfigureAwait(false);
            if(subscriptionResult.Success && (subscriptionResult.UserAirdrops?.AidropDefinitionIds?.Any() ?? false))
            {
                var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
                subscriptionResult.UserAirdrops.UserId = user.Id;
                await _unitOfWork.UserAirdrops.ReplaceAll(subscriptionResult.UserAirdrops);
            }
            return (subscriptionResult.Success, subscriptionResult.FailReason);
        }

        public async Task<AirdropStatus> GetStatus(AirdropDefinition airdropDefinition)
        {
            var userAirdrop = await GetUserAirdrop().ConfigureAwait(false);
            return GetStatusFromDefinition(airdropDefinition, userAirdrop);
        }

        private AirdropStatus GetStatusFromDefinition(AirdropDefinition airdropDefinition, UserAirdrop userAirdrop)
        {
            if(userAirdrop?.AidropDefinitionIds?.Contains(airdropDefinition.AirdropId) ?? false)
            {
                return AirdropStatus.AlreadySubscribed;
            }
            if (airdropDefinition.StartDate >= DateTime.Now)
            {
                return AirdropStatus.NotStarted;
            }
            if (airdropDefinition.ExpirationDate <= DateTime.Now && airdropDefinition.ExpirationDate.Year > 1900)
            {
                return AirdropStatus.Expired;
            }
            if (airdropDefinition.NumberOfSubscribers >= airdropDefinition.MaxLimit)
            {
                return AirdropStatus.Full;
            }
            if (airdropDefinition.NumberOfSubscribers >= airdropDefinition.MaxLimit * 0.9)
            {
                return AirdropStatus.SoonToBeFull;
            }
            return AirdropStatus.Ok;
        }
    }

    public class FakeAirropDefinitions : IAirdropDefinitionsService
    {
        public async Task<IEnumerable<AirdropDefinition>> GetAirdropDefinitions()
        {
            var airdrop1 = new AirdropDefinition
            {
                AirdropId = 1,
                AirdropName = "Fake airdrop#1",
                Amount = 1000,
                MaxLimit = 10000,
                NumberOfSubscribers = 12,
                StartDate = new DateTime(2018, 10, 1),
                TokenName = "eBTC",
                OtherInfoToDisplay = "Welcome",
                ExpirationDate = new DateTime(2018, 12, 31),
                BitcoinTalkAirdropRequirement = new BitcoinTalkAirdropRequirement
                {
                    HasAccount = true,
                    MinimumActivity = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1),
                    MinimumPosition = BitcoinTalkRank.Newbie
                },
                TelegramAirdropRequirement = new TelegramAirdropRequirement
                {
                    HasAccount = true
                },
                TwitterAirdropRequirement = new TwitterAirdropRequirement
                {
                    HasAccount = true,
                    MinimumFollowers = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1)
                }
            };

            var airdrop2 = new AirdropDefinition
            {
                AirdropId = 2,
                AirdropName = "Fake airdrop #2",
                Amount = 1000,
                MaxLimit = 10000,
                NumberOfSubscribers = 12,
                StartDate = new DateTime(2018, 10, 1),
                TokenName = "eBTC",
                OtherInfoToDisplay = "Welcome",
                ExpirationDate = new DateTime(2018, 12, 31),
                BitcoinTalkAirdropRequirement = new BitcoinTalkAirdropRequirement
                {
                    HasAccount = true,
                    MinimumActivity = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1),
                    MinimumPosition = BitcoinTalkRank.Newbie
                },
                TelegramAirdropRequirement = new TelegramAirdropRequirement
                {
                    HasAccount = true
                },
                TwitterAirdropRequirement = new TwitterAirdropRequirement
                {
                    HasAccount = true,
                    MinimumFollowers = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1)
                }
            };

            var airdrop3 = new AirdropDefinition
            {
                AirdropId = 3,
                AirdropName = "Fake airdrop #3",
                Amount = 1000,
                MaxLimit = 10000,
                NumberOfSubscribers = 12,
                StartDate = new DateTime(2018, 10, 1),
                TokenName = "eBTC",
                OtherInfoToDisplay = "Welcome",
                ExpirationDate = new DateTime(2018, 12, 31),
                BitcoinTalkAirdropRequirement = new BitcoinTalkAirdropRequirement
                {
                    HasAccount = true,
                    MinimumActivity = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1),
                    MinimumPosition = BitcoinTalkRank.Newbie
                },
                TelegramAirdropRequirement = new TelegramAirdropRequirement
                {
                    HasAccount = true
                },
                TwitterAirdropRequirement = new TwitterAirdropRequirement
                {
                    HasAccount = true,
                    MinimumFollowers = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1)
                }
            };
            var airdrop4 = new AirdropDefinition
            {
                AirdropId = 4,
                AirdropName = "Fake airdrop #4",
                Amount = 1000,
                MaxLimit = 10000,
                NumberOfSubscribers = 12,
                StartDate = new DateTime(2018, 10, 1),
                TokenName = "eBTC",
                OtherInfoToDisplay = "Welcome",
                ExpirationDate = new DateTime(2018, 12, 31),
                BitcoinTalkAirdropRequirement = new BitcoinTalkAirdropRequirement
                {
                    HasAccount = true,
                    MinimumActivity = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1),
                    MinimumPosition = BitcoinTalkRank.Newbie
                },
                TelegramAirdropRequirement = new TelegramAirdropRequirement
                {
                    HasAccount = true
                },
                TwitterAirdropRequirement = new TwitterAirdropRequirement
                {
                    HasAccount = true,
                    MinimumFollowers = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1)
                }
            };
            var airdrop5 = new AirdropDefinition
            {
                AirdropId = 5,
                AirdropName = "Fake airdrop #5",
                Amount = 1000,
                MaxLimit = 10000,
                NumberOfSubscribers = 12,
                StartDate = new DateTime(2018, 10, 1),
                TokenName = "eBTC",
                OtherInfoToDisplay = "Welcome",
                ExpirationDate = new DateTime(2018, 12, 31),
                BitcoinTalkAirdropRequirement = new BitcoinTalkAirdropRequirement
                {
                    HasAccount = true,
                    MinimumActivity = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1),
                    MinimumPosition = BitcoinTalkRank.Newbie
                },
                TelegramAirdropRequirement = new TelegramAirdropRequirement
                {
                    HasAccount = true
                },
                TwitterAirdropRequirement = new TwitterAirdropRequirement
                {
                    HasAccount = true,
                    MinimumFollowers = 10,
                    MinimumCreationDate = new DateTime(2018, 1, 1)
                }
            };

            return new[] { airdrop1, airdrop2, airdrop3, airdrop4, airdrop5 };
        }

        public async Task<AirdropStatus> GetStatus(AirdropDefinition airdropDefinition)
        {
            var userAirdrop = await GetUserAirdrop().ConfigureAwait(false);
            return GetStatusFromDefinition(airdropDefinition, userAirdrop);
        }

        private AirdropStatus GetStatusFromDefinition(AirdropDefinition airdropDefinition, UserAirdrop userAirdrop)
        {
            if (userAirdrop?.AidropDefinitionIds?.Contains(airdropDefinition.AirdropId) ?? false)
            {
                return AirdropStatus.AlreadySubscribed;
            }
            if (airdropDefinition.StartDate >= DateTime.Now)
            {
                return AirdropStatus.NotStarted;
            }
            if (airdropDefinition.ExpirationDate <= DateTime.Now && airdropDefinition.ExpirationDate.Year > 1900)
            {
                return AirdropStatus.Expired;
            }
            if (airdropDefinition.NumberOfSubscribers >= airdropDefinition.MaxLimit)
            {
                return AirdropStatus.Full;
            }
            if (airdropDefinition.NumberOfSubscribers >= airdropDefinition.MaxLimit * 0.9)
            {
                return AirdropStatus.SoonToBeFull;
            }
            return AirdropStatus.Ok;
        }

        public Task<UserAirdrop> GetUserAirdrop()
        {
            return Task.FromResult<UserAirdrop>(null);
        }

        public Task<(bool Success, FailReason FailReason)> SubscribeToAirdrop(AirdropDefinition airdropDefinition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SyncAirdropDefinitions()
        {
            return Task.FromResult(true);
        }
    }
}

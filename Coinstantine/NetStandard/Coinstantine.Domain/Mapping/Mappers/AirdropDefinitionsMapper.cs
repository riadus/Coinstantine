using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class AirdropDefinitionsMapper : IMapper<AirdropDefinitionDTO, AirdropDefinition>
    {
        private readonly IMapperFactory _mapperFactory;

        public AirdropDefinitionsMapper(IMapperFactory mapperFactory)
        {
            _mapperFactory = mapperFactory;
        }

        public AirdropDefinition Map(AirdropDefinitionDTO source)
        {
            var airdropDefinition = new AirdropDefinition
            {
                AirdropId = source.Id,
                AirdropName = source.AirdropName,
                TokenName = source.TokenName,
                Amount = source.Amount,
                OtherInfoToDisplay = source.OtherInfoToDisplay,
                ExpirationDate = source.ExpirationDate,
                StartDate = source.StartDate,
                MaxLimit = source.MaxLimit
            };

            if (source.TwitterAirdropRequirement != null)
            {
                airdropDefinition.TwitterAirdropRequirement = new TwitterAirdropRequirement
                {
                    HasAccount = source.TwitterAirdropRequirement.HasAccount,
                    HasAccountApplies = source.TwitterAirdropRequirement.HasAccountApplies,
                    MinimumCreationDate = source.TwitterAirdropRequirement.MinimumCreationDate,
                    MinimumCreationDateApplies = source.TwitterAirdropRequirement.MinimumCreationDateApplies,
                    MinimumFollowers = source.TwitterAirdropRequirement.MinimumFollowers,
                    MinimumFollowersApplies = source.TwitterAirdropRequirement.MinimumFollowersApplies
                };
            }

            if (source.BitcoinTalkAirdropRequirement != null)
            {
                var rankMapper = _mapperFactory.GetMapper<BitcoinTalkRank?, string>();
                airdropDefinition.BitcoinTalkAirdropRequirement = new BitcoinTalkAirdropRequirement
                {
                    HasAccount = source.BitcoinTalkAirdropRequirement.HasAccount,
                    HasAccountApplies = source.BitcoinTalkAirdropRequirement.HasAccountApplies,
                    MinimumCreationDate = source.BitcoinTalkAirdropRequirement.MinimumCreationDate,
                    MinimumCreationDateApplies = source.BitcoinTalkAirdropRequirement.MinimumCreationDateApplies,
                    ExactPosition = source.BitcoinTalkAirdropRequirement.ExactRank,
                    ExactRank = rankMapper.Map(source.BitcoinTalkAirdropRequirement.ExactRank),
                    ExactRankApplies = source.BitcoinTalkAirdropRequirement.ExactRankApplies,
                    MinimumPosition = source.BitcoinTalkAirdropRequirement.MinimumRank,
                    MinimumRank = rankMapper.Map(source.BitcoinTalkAirdropRequirement.MinimumRank),
                    MinimumRankApplies = source.BitcoinTalkAirdropRequirement.MinimumRankApplies,
                    MinimumActivity = source.BitcoinTalkAirdropRequirement.MinimumActivity,
                    MinimumActivityApplies = source.BitcoinTalkAirdropRequirement.MinimumActivityApplies,
                    MinimumPosts = source.BitcoinTalkAirdropRequirement.MinimumPosts,
                    MinimumPostsApplies = source.BitcoinTalkAirdropRequirement.MinimumPostsApplies
                };
            }

            if (source.TelegramAirdropRequirement != null)
            {
                airdropDefinition.TelegramAirdropRequirement = new TelegramAirdropRequirement
                {
                    HasAccount = source.TelegramAirdropRequirement.HasAccount,
                    HasAccountApplies = source.TelegramAirdropRequirement.HasAccountApplies
                };
            }
            return airdropDefinition;
        }

        public AirdropDefinitionDTO MapBack(AirdropDefinition source)
        {
            throw new NotImplementedException();
        }
    }
}

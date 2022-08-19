using System;
using System.Collections.Generic;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Airdrops
{
    [RegisterInterfaceAsDynamic]
    public class RequirementToLambda : IRequirementToLambda
    {
        public IEnumerable<IBitcoinTalkRequirement> GetRequirements(BitcoinTalkAirdropRequirement bitcoinTalkAirdropRequirement)
        {
            var result = new List<IBitcoinTalkRequirement>
            {
                new BitcoinTalkRequirement
                {
                    Name = TranslationKeys.Airdrop.BctAccount,
                    MatchFunc = p => bitcoinTalkAirdropRequirement.HasAccount && (p?.Validated ?? false),
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.HasAccountApplies,
                    Value = p => p?.Username
                },
                new BitcoinTalkRequirement
                {
                    Name = TranslationKeys.Airdrop.BctMinimumActivity,
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumActivity <=  p?.Activity,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumActivityApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumActivity.ToString()
                },
                new BitcoinTalkRequirement
                {
                    Name = TranslationKeys.Airdrop.BctExactPosition,
                    MatchFunc = p => bitcoinTalkAirdropRequirement.ExactPosition == p?.Position,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.ExactRankApplies,
                    Value = p => bitcoinTalkAirdropRequirement.ExactRank
                },
                new BitcoinTalkRequirement
                {
                    Name = TranslationKeys.Airdrop.BctMinimumPosition,
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumPosition <= p?.Position,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumRankApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumRank
                },
                new BitcoinTalkRequirement
                {
                    Name = TranslationKeys.Airdrop.BctMinimumPosts,
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumPosts <= p?.Posts,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumPostsApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumPosts.ToString()
                },
                new BitcoinTalkRequirement
                {
                    Name = TranslationKeys.Airdrop.BctCreationDate,
                    MatchFunc = p => bitcoinTalkAirdropRequirement.MinimumCreationDate >= p?.RegistredDate && p?.RegistredDate.Year >= 1900,
                    RequirementApplies = () => bitcoinTalkAirdropRequirement.MinimumCreationDateApplies,
                    Value = p => bitcoinTalkAirdropRequirement.MinimumCreationDate?.ToString("D")
                }
            };

            return result;
        }

        public IEnumerable<ITelegramRequirement> GetRequirements(TelegramAirdropRequirement telegramAirdropRequirement)
        {
            var result = new List<TelegramRequirement>
            {
                new TelegramRequirement
                {
                    Name = TranslationKeys.Airdrop.TelegramAccount,
                    MatchFunc = p => telegramAirdropRequirement.HasAccount && (p?.Validated ?? false),
                    RequirementApplies = () => telegramAirdropRequirement.HasAccountApplies,
                    Value = p => p?.Username
                }
            };

            return result;
        }

        public IEnumerable<ITwitterRequirement> GetRequirements(TwitterAirdropRequirement twitterAirdropRequirement)
        {
            var result = new List<ITwitterRequirement>
            {
                new TwitterRequirement
                {
                    Name = TranslationKeys.Airdrop.TwitterAccount,
                    MatchFunc = p => twitterAirdropRequirement.HasAccount && (p?.Validated ?? false),
                    RequirementApplies = () => twitterAirdropRequirement.HasAccountApplies,
                    Value = p => p?.ScreenName
                },
                new TwitterRequirement
                {
                    Name = TranslationKeys.Airdrop.TwitterFollowers,
                    MatchFunc = new Func<TwitterProfile, bool>(p => twitterAirdropRequirement.MinimumFollowers <= p?.Followers),
                    RequirementApplies = () => twitterAirdropRequirement.MinimumFollowersApplies,
                    Value = p => twitterAirdropRequirement.MinimumFollowers.ToString()
                },
                new TwitterRequirement
                {
                    Name = TranslationKeys.Airdrop.TwitterCreationDate,
                    MatchFunc = new Func<TwitterProfile, bool>(p => twitterAirdropRequirement.MinimumCreationDate >= p?.CreationDate && p?.CreationDate.Year > 2000),
                    RequirementApplies = () => twitterAirdropRequirement.MinimumCreationDateApplies,
                    Value = p => twitterAirdropRequirement.MinimumCreationDate?.ToString("D")
                }
            };

            return result;
        }

        public IEnumerable<IRequirement> GetRequirements(IAirdropRequirement requirement)
        {
            if (requirement is TwitterAirdropRequirement twitterRequirement)
            {
                return GetRequirements(twitterRequirement);
            }
            if (requirement is TelegramAirdropRequirement telegramRequirement)
                return GetRequirements(telegramRequirement);
            if (requirement is BitcoinTalkAirdropRequirement bitcoinTalkAirdropRequirement)
                return GetRequirements(bitcoinTalkAirdropRequirement);

            return default(IEnumerable<Requirement<IProfileItem>>);
        }
    }
}

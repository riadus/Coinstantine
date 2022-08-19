using System;
using System.Collections.Generic;
using System.Linq;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Airdrops
{
    [RegisterInterfaceAsDynamic]
    public class RequirementConstructor : IRequirementsConstructor
    {
        private readonly IRequirementToLambda _requirementToLambda;

        public RequirementConstructor(IRequirementToLambda requirementToLambda)
        {
            _requirementToLambda = requirementToLambda;
        }

        public IEnumerable<ItemInfo> BuildRequirements(IAirdropRequirement airdropRequirement, IProfileItem profileItem)
        {
            var result = new List<ItemInfo>();
            result.AddRange(BuildRequirement(airdropRequirement, profileItem));
            return result;
        }

        public bool MeetsAllRequirement(UserProfile userProfile, IEnumerable<IAirdropRequirement> airdropRequirements)
        {
            bool result = true;
            foreach(var airdropRequirement in airdropRequirements.Where(x => x != null))
            {
                var requirements = _requirementToLambda.GetRequirements(airdropRequirement);
                foreach (var req in requirements)
                {
                    if (!result) { break; }
                    if (req is ITwitterRequirement twitterRequirement)
                    {
                        if (twitterRequirement.RequirementApplies())
                        {
                            result &= twitterRequirement.MatchFunc(userProfile.TwitterProfile);
                            continue;
                        }
                    }
                    if (req is ITelegramRequirement telegramRequirement)
                    {
                        if (telegramRequirement.RequirementApplies())
                        {
                            result &= telegramRequirement.MatchFunc(userProfile.TelegramProfile);
                            continue;
                        }
                    }
                    if (req is IBitcoinTalkRequirement bitcoinTalkRequirement)
                    {
                        if (bitcoinTalkRequirement.RequirementApplies())
                        {
                            result &= bitcoinTalkRequirement.MatchFunc(userProfile.BitcoinTalkProfile);
                            continue;
                        }
                    }
                }
            }
            return result;
        }

        private IEnumerable<ItemInfo> BuildRequirement(IAirdropRequirement airdropRequirement, IProfileItem profileItem)
        {
            var result = new List<ItemInfo>();
            var requirements = _requirementToLambda.GetRequirements(airdropRequirement);
            if (!requirements?.Any() ?? true)
            {
                return result;
            }
            foreach (var req in requirements)
            {
                if (req is ITwitterRequirement twitterRequirement)
                {
                    if (twitterRequirement.RequirementApplies())
                    {
                        result.Add((twitterRequirement.Name, twitterRequirement.Value(profileItem as TwitterProfile), Display.New));
                        result.Add((TranslationKeys.General.Blank, twitterRequirement.MatchFunc(profileItem as TwitterProfile).ToString(), Display.Grouped));
                        continue;
                    }
                }
                if (req is ITelegramRequirement telegramRequirement)
                {
                    if (telegramRequirement.RequirementApplies())
                    {
                        result.Add((telegramRequirement.Name, telegramRequirement.Value(profileItem as TelegramProfile), Display.New));
                        result.Add((TranslationKeys.General.Blank, telegramRequirement.MatchFunc(profileItem as TelegramProfile).ToString(), Display.Grouped));
                        continue;
                    }
                }
                if (req is IBitcoinTalkRequirement bitcoinTalkRequirement)
                {
                    if (bitcoinTalkRequirement.RequirementApplies())
                    {
                        result.Add((bitcoinTalkRequirement.Name, bitcoinTalkRequirement.Value(profileItem as BitcoinTalkProfile), Display.New));
                        result.Add((TranslationKeys.General.Blank, bitcoinTalkRequirement.MatchFunc(profileItem as BitcoinTalkProfile).ToString(), Display.Grouped));
                        continue;
                    }
                }
            }

            return result;
        }
    }
}

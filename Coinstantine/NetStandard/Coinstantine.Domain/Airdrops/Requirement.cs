using System;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Airdrops
{
    public class Requirement : IRequirement
    {
        public TranslationKey Name { get; set; }
        public virtual Func<IProfileItem, bool> MatchFunc { get; set; }
        public Func<bool> RequirementApplies { get; set; }
        public virtual Func<IProfileItem, string> Value { get; set; }
    }

    public class Requirement<T> : Requirement where T : IProfileItem
    {
        public new Func<T, bool> MatchFunc { get; set; }
        public new Func<T, string> Value { get; set; }
    }

    public class TwitterRequirement : Requirement<TwitterProfile>, ITwitterRequirement
    {
    }

    public class TelegramRequirement : Requirement<TelegramProfile>, ITelegramRequirement
    {
    }

    public class BitcoinTalkRequirement : Requirement<BitcoinTalkProfile>, IBitcoinTalkRequirement
    {
    }
}

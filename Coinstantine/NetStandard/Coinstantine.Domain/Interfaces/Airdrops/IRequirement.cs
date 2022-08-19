using System;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Interfaces.Airdrops
{
    public interface IRequirement
    {
        TranslationKey Name { get; set; }
        Func<bool> RequirementApplies { get; set; }
    }

    public interface IRequirement<T> : IRequirement
    {
        Func<T, string> Value { get; set; }
        Func<T, bool> MatchFunc { get; set; }
    }

    public interface ITwitterRequirement : IRequirement<TwitterProfile>
    {

    }

    public interface ITelegramRequirement : IRequirement<TelegramProfile>
    {

    }

    public interface IBitcoinTalkRequirement : IRequirement<BitcoinTalkProfile>
    {

    }
}

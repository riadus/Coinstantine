using System;
using Coinstantine.Data;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class BctRankMapper : IMapper<BitcoinTalkRank?, string>
    {
        public string Map(BitcoinTalkRank? source)
        {
            if(source == null)
            {
                return string.Empty;
            }
            switch (source)
            {
                case BitcoinTalkRank.BrandNew:
                    return "Brand New";
                case BitcoinTalkRank.FullMember:
                    return "Full Member";
                case BitcoinTalkRank.HeroMember:
                    return "Hero Member";
                case BitcoinTalkRank.JrMember:
                    return "Junior Member";
                case BitcoinTalkRank.Legendary:
                    return "Legendary";
                case BitcoinTalkRank.Member:
                    return "Member";
                case BitcoinTalkRank.Newbie:
                    return "Newbie";
                case BitcoinTalkRank.SrMember:
                    return "Senior Member";
                default: return string.Empty;
            }
        }

        public BitcoinTalkRank? MapBack(string source)
        {
            throw new NotImplementedException();
        }
    }
}

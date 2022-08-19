using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class BitcoinTalkMapper : IMapper<BitcoinTalkUserOnline, BitcoinTalkProfile>
    {
        private readonly IMapper<BitcoinTalkRank?, string> _bctRankMapper;
        public BitcoinTalkMapper(IMapper<BitcoinTalkRank?, string> bctRankMapper)
        {
            _bctRankMapper = bctRankMapper;
        }
        public BitcoinTalkProfile Map(BitcoinTalkUserOnline source)
        {
            return new BitcoinTalkProfile
            {
                Location = source.Location,
                Rank = _bctRankMapper.Map(source.Position),
                Position = source.Position,
                Username = source.Username,
                Activity = source.Activity,
                Posts = source.Posts,
                UserId = source.BctId.ToString(),
                RegistredDate = source.RegistrationDate,
                Validated = source.Validated,
                ValidationDate = source.ValidationDate
            };
        }

        public BitcoinTalkUserOnline MapBack(BitcoinTalkProfile source)
        {
            throw new NotImplementedException();
        }
    }
}

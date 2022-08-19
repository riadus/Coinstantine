using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class TelegramOnlineMapper : IMapper<TelegramProfileOnline, TelegramProfile>
    {
        public TelegramProfile Map(TelegramProfileOnline source)
        {
            return new TelegramProfile
            {
                TelegramId = source.TelegramId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Username = source.Username,
                Validated = source.Validated,
                ValidationDate = source.ValidationDate
            };
        }

        public TelegramProfileOnline MapBack(TelegramProfile source)
        {
            throw new NotImplementedException();
        }
    }
}

using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class EtherPriceMapper : IMapper<PriceDTO, Price>
    {
        public Price Map(PriceDTO source)
        {
            return new Price
            {
                Eur = source.Eur,
                Usd = source.Usd
            };
        }

        public PriceDTO MapBack(Price source)
        {
            return new PriceDTO
            {
                Eur = source.Eur,
                Usd = source.Usd
            };
        }
    }
}

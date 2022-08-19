using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class CoinstantinePriceConfigMapper : IMapper<CoinstantinePriceConfigDTO, CoinstantinePriceConfig>
    {
        public CoinstantinePriceConfig Map(CoinstantinePriceConfigDTO source)
        {
            return new CoinstantinePriceConfig
            {
                Bonus = source.Bonus,
                Eth = source.Eth,
                MinimumEth = source.MinimumEth
            };
        }

        public CoinstantinePriceConfigDTO MapBack(CoinstantinePriceConfig source)
        {
            return new CoinstantinePriceConfigDTO
            {
                Bonus = source.Bonus,
                Eth = source.Eth,
                MinimumEth = source.MinimumEth
            };
        }
    }
}

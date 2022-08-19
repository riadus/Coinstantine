using System;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class EnvironmentMapper : IMapper<EnvironmentDTO, EnvironmentInfo>
    {
        public EnvironmentInfo Map(EnvironmentDTO source)
        {
            return new EnvironmentInfo
            {
                ApiEnvironment = source.Environment.ToEnum<ApiEnvironment>(),
                Web3Url = source.Web3Url,
                EtherscanUrl = source.EtherscanUrl,
                EthereumNetwork = source.EthereumEnvironment.ToEnum<EthereumNetwork>()
            };
        }

        public EnvironmentDTO MapBack(EnvironmentInfo source)
        {
            throw new NotImplementedException();
        }
    }
}

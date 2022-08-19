using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class AirdropSubscriptionResultMapper : IMapper<AirdropSubscriptionResultDTO, AirdropSubscriptionResult>
    {
        private readonly IMapper<UserAirdropDTO, UserAirdrop> userAirdropMapper;
        public AirdropSubscriptionResultMapper(IMapperFactory mapperFactory)
        {
            userAirdropMapper = mapperFactory.GetMapper<UserAirdropDTO, UserAirdrop>();
        }

        public AirdropSubscriptionResult Map(AirdropSubscriptionResultDTO source)
        {
            return new AirdropSubscriptionResult
            {
                UserAirdrops = userAirdropMapper.Map(source.UserAirdrops),
                FailReason = source.FailReason,
                Success = source.Success
            };
        }

        public AirdropSubscriptionResultDTO MapBack(AirdropSubscriptionResult source)
        {
            throw new NotImplementedException();
        }
    }
}

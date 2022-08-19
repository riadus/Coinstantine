using System;
using System.Linq;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain.Mapping.Mappers
{
    [RegisterInterfaceAsDynamic]
    public class UserAirdropMapper : IMapper<UserAirdropDTO, UserAirdrop>
    {
        public UserAirdrop Map(UserAirdropDTO source)
        {
            if (source == null)
                return null;
            return new UserAirdrop
            {
                AidropDefinitionIds = source.AirdropIds
            };
        }

        public UserAirdropDTO MapBack(UserAirdrop source)
        {
            throw new NotImplementedException();
        }
    }
}

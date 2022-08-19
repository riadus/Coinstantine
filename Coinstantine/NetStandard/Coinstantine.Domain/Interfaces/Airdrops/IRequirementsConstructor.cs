using System.Collections.Generic;
using Coinstantine.Data;
using Coinstantine.Domain.Airdrops;

namespace Coinstantine.Domain.Interfaces.Airdrops
{
    public interface IRequirementsConstructor
    {
        IEnumerable<ItemInfo> BuildRequirements(IAirdropRequirement airdropRequirement, IProfileItem profileItem);
        bool MeetsAllRequirement(UserProfile userProfile, IEnumerable<IAirdropRequirement> requirements);
    }
}

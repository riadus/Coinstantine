using System.Collections.Generic;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces.Airdrops
{
    public interface IRequirementToLambda
    {
        IEnumerable<IRequirement> GetRequirements(IAirdropRequirement requirement);
    }
}

using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces.Airdrops;

namespace Coinstantine.UnitTests.Builders
{
    public class RequirementToLambdaBuilder
    {
        public IRequirementToLambda Build()
        {
            return new RequirementToLambda();
        }
    }
}

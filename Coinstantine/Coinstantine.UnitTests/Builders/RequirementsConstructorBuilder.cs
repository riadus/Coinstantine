using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces.Airdrops;
using FakeItEasy;

namespace Coinstantine.UnitTests.Builders
{
    public class RequirementsConstructorBuilder
    {
        private IRequirementToLambda _requirementToLambda = A.Fake<IRequirementToLambda>();
        public IRequirementsConstructor Build()
        {
            return new RequirementConstructor(_requirementToLambda);
        }

        public RequirementsConstructorBuilder WithRequirementToLambda(IRequirementToLambda requirementToLambda)
        {
            _requirementToLambda = requirementToLambda;
            return this;
        }
    }
}

using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Airdrops;
using FakeItEasy;

namespace Coinstantine.UnitTests.Builders
{
    public class AirdropDefinitionsServiceBuilder
    {
        private IBackendService _backendService = A.Fake<IBackendService>();
        private IUnitOfWork _unitOfWork = new MockUnitOfWork();
        private IProfileProvider _profileProvider = A.Fake<IProfileProvider>();
        private UserProfile _userProfile = new UserProfile
        {
            Id = 1
        };
        public IAirdropDefinitionsService Build()
        {
            A.CallTo(() => _profileProvider.GetUserProfile()).Returns(Task.FromResult(_userProfile));
            return new AirdropDefinitionsService(_backendService, _unitOfWork, _profileProvider);
        }

        public AirdropDefinitionsServiceBuilder WithBackendService(IBackendService backendService)
        {
            _backendService = backendService;
            return this;
        }

        public AirdropDefinitionsServiceBuilder WithUnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            return this;
        }

        public AirdropDefinitionsServiceBuilder WithUserProfile(UserProfile userProfile)
        {
            _userProfile = userProfile;
            return this;
        }
    }
}

using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class ProfileValidationService : IProfileValidationService
    {
        private readonly IProfileProvider _profileProvider;
        public ProfileValidationService(IProfileProvider profileProvider)
        {
            _profileProvider = profileProvider;
        }

        public Task<bool> ValidateTwitterAccount()
        {
            return Task.FromResult(true);
        }
    }
}

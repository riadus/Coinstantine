using System.Threading.Tasks;

namespace Coinstantine.Core.Services
{
    public interface IBiometricsAuthenticator
    {
        Task<bool> HasBiometricsCapability();
        Task<string> BiometricsTechnologyName();
        Task<bool> Authenticate(string reason);
    }
}

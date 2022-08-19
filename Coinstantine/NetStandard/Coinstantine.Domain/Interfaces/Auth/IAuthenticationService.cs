using System.Threading.Tasks;

namespace Coinstantine.Domain.Interfaces.Auth
{
    public interface IAuthenticationService
    {
        Task<bool> Login();
        Task ResetPassword();
        Task<bool> LoginAndRenewTokenIfNeeded();
        Task<bool> CreateAccount();
    }
}

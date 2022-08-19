using System.Threading.Tasks;
using Coinstantine.Domain.Interfaces.Auth;

namespace Coinstantine.Domain.Interfaces
{
    public interface ITokenProvider
    {
        Task SaveToken(Tokens token);
        Task<string> GetToken();
        Task<string> GetRefreshToken();
        Task<bool> HasExpiredToken();
        Task<bool> HasExpiredRefreshToken();
    }
}

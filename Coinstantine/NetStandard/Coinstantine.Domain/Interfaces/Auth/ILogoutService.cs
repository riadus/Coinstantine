using System;
using System.Threading.Tasks;

namespace Coinstantine.Domain.Interfaces.Auth
{
    public interface ILogoutService
    {
        Task SessionExpired();
        Task TooManyWrongPincodes();
        Task RegularLogout();
    }

    public interface ITokenRefreshService
    {
        Task<bool> Refresh();
    }

    public class Tokens
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public long ExpirationDate { get; set; }
    }
}

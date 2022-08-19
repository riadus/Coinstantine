using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Domain.Auth.Models;

namespace Coinstantine.Domain.Interfaces.Auth
{
    public interface IAccountBackendService
    {
        Task ConfirmAccount(string email, string confirmationCode);
        Task<AccountCorrect> CreateAccount(AccountCreationModel accountCreationModel);
        Task<IEnumerable<Country>> GetCountries();
        Task<AccountCorrect> IsAccountCorrect(AccountCreationModel accountCreationModel);
        Task RequestChangePassword(string email);
        Task ResetPassword(string userId, string confirmationCode);
        Task SendUsername(string email);
        Task<LoginStatus> Login(LoginModel loginModel);
    }
}

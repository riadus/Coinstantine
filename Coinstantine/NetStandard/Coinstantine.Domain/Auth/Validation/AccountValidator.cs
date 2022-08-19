using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Auth.Validation
{
    [RegisterInterfaceAsDynamic]
    public class AccountValidator : IAccountValidator
    {
        private readonly IAccountBackendService _accountBackendService;

        public AccountValidator(IAccountBackendService accountBackendService)
        {
            _accountBackendService = accountBackendService;
        }

        public async Task<ValidationResult<AccountCorrect>> IsValid(AccountCreationModel param)
        {
            var accountCorrect = await _accountBackendService.IsAccountCorrect(param).ConfigureAwait(false);
            var validationResult = new ValidationResult<AccountCorrect> { Result = accountCorrect };
            if (!accountCorrect.EmailAvailable)
            {
                validationResult.ErrorMessage = TranslationKeys.UserAccount.EmailTaken;
            }
            if (!accountCorrect.UsernameAvailable)
            {
                validationResult.ErrorMessage = TranslationKeys.UserAccount.UsernameTaken;
            }
            if (!accountCorrect.PasswordCorrect)
            {
                validationResult.ErrorMessage = TranslationKeys.UserAccount.PasswordInvalid;
            }
            return validationResult;
        }
    }
}

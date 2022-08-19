using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Auth.Validation
{
    [RegisterInterfaceAsDynamic]
    public class PasswordValidator : IPasswordValidator
    {
        private const string PasswordRegexPattern = "((?=.*\\d)(?=.*[A-Z]).{8,})";
        public Task<ValidationResult<bool>> IsValid(string param)
        {
            var validationResult = new ValidationResult<bool>
            {
                Result = false,
                ErrorMessage = TranslationKeys.UserAccount.PasswordPattern
            };

            if (param == null || !Regex.IsMatch(param, PasswordRegexPattern))
            {
                return Task.FromResult(validationResult);
            }
            validationResult.Result = true;
            validationResult.ErrorMessage = null;
            return Task.FromResult(validationResult);
        }
    }
}

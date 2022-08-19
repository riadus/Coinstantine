using System;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Auth.Validation
{
    [RegisterInterfaceAsDynamic]
    public class DateOfBirthValidator : IDateOfBirthValidator
    {
        public Task<ValidationResult<bool>> IsValid(DateTime? param)
        {
            var validationResult = new ValidationResult<bool>
            {
                Result = true
            };
            if (param != null && param <= DateTime.Now.AddYears(-18))
            {
                return Task.FromResult(validationResult);
            }
            validationResult.Result = false;
            validationResult.ErrorMessage = TranslationKeys.UserAccount.DateOfBirthInvalid;
            return Task.FromResult(validationResult);
        }
    }
}

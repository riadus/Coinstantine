using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Auth.Validation
{
    [RegisterInterfaceAsDynamic]
    public class NotNullOrEmptyValidator : INotNullOrEmptyValidator
    {
        public Task<ValidationResult<bool>> IsValid(string param)
        {
            return Task.FromResult(new ValidationResult<bool>
            {
                Result = param.IsNotNull(),
                ErrorMessage = TranslationKeys.General.Blank
            });
        }
    }
}

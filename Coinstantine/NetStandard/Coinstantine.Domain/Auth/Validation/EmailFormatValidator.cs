using System.Net.Mail;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Auth.Validation
{
    [RegisterInterfaceAsDynamic]
    public class EmailFormatValidator : IEmailFormatValidator
    {
        public Task<ValidationResult<bool>> IsValid(string param)
        {
            try
            {
                var mailAddress = new MailAddress(param);
                return Task.FromResult(new ValidationResult<bool>
                {
                    Result = true
                });
            }
            catch
            {
                return Task.FromResult(new ValidationResult<bool>
                {
                    Result = false,
                    ErrorMessage = TranslationKeys.UserAccount.EmailInvalid
                });
            }
        }
    }
}

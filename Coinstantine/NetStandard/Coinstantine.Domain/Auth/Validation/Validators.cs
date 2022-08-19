using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces.Auth.Validation;

namespace Coinstantine.Domain.Auth.Validation
{
    [RegisterInterfaceAsDynamic]
    public class Validators : IValidators
    {
        public Validators(IPasswordValidator passwordValidator,
                          IEmailFormatValidator emailFormatValidator,
                          IAccountValidator accountValidator,
                          INotNullOrEmptyValidator notNullOrEmptyValidator,
                          IDateOfBirthValidator dateOfBirthValidator)
        {
            PasswordValidator = passwordValidator;
            EmailFormatValidator = emailFormatValidator;
            AccountValidator = accountValidator;
            NotNullOrEmptyValidator = notNullOrEmptyValidator;
            DateOfBirthValidator = dateOfBirthValidator;
        }
        public IPasswordValidator PasswordValidator { get; }
        public IEmailFormatValidator EmailFormatValidator { get; }
        public IAccountValidator AccountValidator { get; }
        public INotNullOrEmptyValidator NotNullOrEmptyValidator { get; }
        public IDateOfBirthValidator DateOfBirthValidator { get; }
    }
}

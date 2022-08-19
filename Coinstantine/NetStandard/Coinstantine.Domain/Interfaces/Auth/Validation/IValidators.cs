namespace Coinstantine.Domain.Interfaces.Auth.Validation
{
    public interface IValidators
    {
        IPasswordValidator PasswordValidator { get; }
        IEmailFormatValidator EmailFormatValidator { get; }
        IAccountValidator AccountValidator { get; }
        INotNullOrEmptyValidator NotNullOrEmptyValidator { get; }
        IDateOfBirthValidator DateOfBirthValidator { get; }
    }
}

using System;

namespace Coinstantine.Domain.Interfaces.Auth.Validation
{
    public interface IDateOfBirthValidator : IAccountValidator<DateTime?, bool> { }
}

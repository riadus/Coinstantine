using System.Threading.Tasks;
using Coinstantine.Domain.Auth.Models;

namespace Coinstantine.Domain.Interfaces.Auth.Validation
{
    public interface IAccountValidator<TParam, TReturnType>
    {
        Task<ValidationResult<TReturnType>> IsValid(TParam param);
    }

    public interface IAccountValidator : IAccountValidator<AccountCreationModel, AccountCorrect> { }
}

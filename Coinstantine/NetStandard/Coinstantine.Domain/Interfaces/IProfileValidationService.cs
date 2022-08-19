using System.Threading.Tasks;

namespace Coinstantine.Domain.Interfaces
{
    public interface IProfileValidationService
    {
        Task<bool> ValidateTwitterAccount();
    }
}

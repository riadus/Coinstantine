using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coinstantine.Core.UIServices
{
    public interface IUserOptionService
    {
        Task<UserOption<T>> ShowOptions<T>(string message, string cancel, IEnumerable<UserOption<T>> options);
        Task<ShareOption> ShowShareOptions(string message, string cancel);
    }

    public enum ShareOption
    {
        Copy,
        Send
    }
}

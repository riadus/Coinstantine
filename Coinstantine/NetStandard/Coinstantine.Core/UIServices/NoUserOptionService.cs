using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Core.UIServices
{
    [RegisterInterfaceAsDynamic(IoCPlatform.iOS)]
    public class NoUserOptionService : IUserOptionService
    {
        public Task<UserOption<T>> ShowOptions<T>(string message, string cancel, IEnumerable<UserOption<T>> options)
        {
            return Task.FromResult(options.ElementAt(0));
        }

        public Task<ShareOption> ShowShareOptions(string message, string cancel)
        {
            return Task.FromResult(ShareOption.Send);
        }
    }
}

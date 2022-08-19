using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Blockchain;
using Nethereum.Geth;

namespace Coinstantine.Domain.Blockchain
{
    [RegisterInterfaceAsLazySingleton]
    public class Web3Provider : IWeb3Provider
    {
        private readonly IEnvironmentInfoProvider _environmentInfoProvider;

        public Web3Provider(IEnvironmentInfoProvider environmentInfoProvider)
        {
            _environmentInfoProvider = environmentInfoProvider;
        }

        public async Task<Web3Geth> GetWeb3()
        {
            var url = await _environmentInfoProvider.GetWeb3Url().ConfigureAwait(false);
            return new Web3Geth(url);
        }
    }
}

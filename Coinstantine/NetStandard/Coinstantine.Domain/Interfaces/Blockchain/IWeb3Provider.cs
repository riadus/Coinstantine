using System.Threading.Tasks;
using Nethereum.Geth;

namespace Coinstantine.Domain.Interfaces.Blockchain
{
    public interface IWeb3Provider
    {
        Task<Web3Geth> GetWeb3();
    }
}

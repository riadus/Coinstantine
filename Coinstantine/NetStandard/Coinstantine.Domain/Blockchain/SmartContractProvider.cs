using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interface.Blockchain;
using Coinstantine.Domain.Interfaces.Blockchain;
using Nethereum.Contracts;

namespace Coinstantine.Domain.Blockchain
{
    [RegisterInterfaceAsDynamic]
    public class SmartContractProvider : ISmartContractProvider
    {
        private readonly IWeb3Provider _web3Provider;
        private readonly IUnitOfWork _unitOfWork;

        public SmartContractProvider(IWeb3Provider web3Provider,
                                     IUnitOfWork unitOfWork)
        {
            _web3Provider = web3Provider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Contract> GetSmartContract(string name)
        {
            var web3 = await _web3Provider.GetWeb3().ConfigureAwait(false);
            var smartContract = await _unitOfWork.SmartContractDefinitions.GetAsync(x => x.Name == name).ConfigureAwait(false);
            if(smartContract == null)
            {
                return null;
            }
            return web3.Eth.GetContract(smartContract.Abi, smartContract.Address);
        }
    }
}

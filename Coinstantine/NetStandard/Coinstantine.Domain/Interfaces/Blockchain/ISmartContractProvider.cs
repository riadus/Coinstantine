using System;
using System.Threading.Tasks;
using Nethereum.Contracts;

namespace Coinstantine.Domain.Interface.Blockchain
{
    public interface ISmartContractProvider
    {
        Task<Contract> GetSmartContract(string name);
    }
}

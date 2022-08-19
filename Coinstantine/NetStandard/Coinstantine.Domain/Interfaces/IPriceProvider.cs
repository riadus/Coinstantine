using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface IPriceProvider
    {
		Task<CoinstantinePriceConfig> GetCoinstantinePriceConfig();
		Task<Price> GetEthPrice();
        Task<bool> SyncEthPrice();
        Task<bool> SyncCoinstantinePriceConfig();
    }
}

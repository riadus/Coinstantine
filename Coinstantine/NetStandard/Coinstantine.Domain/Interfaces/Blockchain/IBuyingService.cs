using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces.Blockchain
{
    public interface IBuyingService
    {
        Task<BuyingReceipt> Buy(decimal ethToInvest);
        Task<IEnumerable<BuyingReceipt>> GetReceipts();
        Task<IEnumerable<BuyingReceipt>> SyncReceipts();
    }
}

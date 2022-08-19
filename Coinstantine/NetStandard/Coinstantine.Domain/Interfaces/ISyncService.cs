using System.Threading.Tasks;

namespace Coinstantine.Domain.Interfaces
{
    public interface ISyncService
    {
		Task SyncIfNeeded();
        Task<bool> SyncTranslations();
        Task ForceSync();
        Task<bool> CheckOnlineProfileAndUnpdateIfNeeded();
        Task<bool> NeedsToSync();
    }
}

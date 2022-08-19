using System.Threading.Tasks;

namespace Coinstantine.Data.Migrations
{
    public interface IMigrationVersionProvider
    {
        Task UpdateDbVersion(int newVersion);
        Task<int> GetCurrentDbVersion();
        Task<bool> IsCleanDatabase();
    }
}

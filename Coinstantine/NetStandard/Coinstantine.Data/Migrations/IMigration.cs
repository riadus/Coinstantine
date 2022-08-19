using System.Threading.Tasks;

namespace Coinstantine.Data.Migrations
{
    public interface IMigration
    {
        int Version { get; }
        string Description { get; }

        Task Up();
        Task Down();
    }
}

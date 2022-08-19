using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Coinstantine.Data.Migrations
{
    public interface IMigrationService
    {
        Task MigrateToLatest(Assembly assemblyToSearch);
        Task MigrateToLatest(Assembly assemblyToSearch, IProgress<MigrationProgress> progress);
        Task MigrateTo(int? targetVersion, Assembly assemblyToSearch);
        Task MigrateTo(int? targetVersion, Assembly assemblyToSearch, IProgress<MigrationProgress> progress);
        Task<bool> NeedsMigration(Assembly assemblyToSearch);
    }
}

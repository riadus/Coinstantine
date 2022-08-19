using System;
using System.Collections.Generic;
using System.Reflection;

namespace Coinstantine.Data.Migrations
{
    public interface IMigrationsProvider
    {
        IDictionary<string, Type> RetrieveMigrations(Assembly assembly);
        IEnumerable<IMigration> RetrieveInflatedMigrations(Assembly assemblyToSearch, IMigrationInflater inflater, int fromVersionExclusive, int? toVersion);
    }
}

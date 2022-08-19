using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Data.Migrations
{
    [RegisterInterfaceAsDynamic]
    public class MigrationsProvider : IMigrationsProvider
    {
        public IDictionary<string, Type> RetrieveMigrations(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            var migrations = new Dictionary<string, Type>();
            foreach (var migration in assembly.DefinedTypes
                     .Where(t => t.ImplementedInterfaces.Contains(typeof(IMigration)) && !t.IsAbstract).OrderBy(t => t.Name))
            {
                migrations.Add(migration.Name, migration.AsType());
            }
            return migrations;
        }

        public IEnumerable<IMigration> RetrieveInflatedMigrations(Assembly assemblyToSearch, IMigrationInflater inflater, int fromVersionExclusive, int? toVersion)
        {

            var migrations = RetrieveMigrations(assemblyToSearch)
                          .Select(m => inflater.Inflate(m.Value))
                          .OrderBy(m => m.Version)
                          .SkipWhile(m => m.Version <= fromVersionExclusive);
            return toVersion.HasValue
                ? migrations.TakeWhile(m => m.Version <= toVersion.Value)
                : migrations;

        }
    }
}

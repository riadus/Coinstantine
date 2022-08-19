using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Data.Migrations
{
    [RegisterInterfaceAsDynamic]
    public class MigrationService : IMigrationService
    {
        readonly IMigrationsProvider _provider;
        readonly IMigrationInflater _inflater;
        readonly IMigrationVersionProvider _versionProvider;

        public MigrationService(IMigrationsProvider provider, IMigrationInflater migrationInflater, IMigrationVersionProvider versionProvider)
        {
            _provider = provider;
            _inflater = migrationInflater;
            _versionProvider = versionProvider;
        }

        public Task MigrateTo(int? targetVersion, Assembly assemblyToSearch)
        {
            return MigrateTo(targetVersion, assemblyToSearch, null);
        }

        /// <summary>
        /// Migrates to the provided version, or to the latest available version if not provided.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="targetVersion">The target version</param>
        /// <param name="assemblyToSearch">The assembly to search for migrations.</param>
        /// <param name="progress">Reports the migration status</param>
        public async Task MigrateTo(int? targetVersion, Assembly assemblyToSearch, IProgress<MigrationProgress> progress)
        {
            progress?.Report(MigrationProgress.Preparing);
            var currentVersion = await _versionProvider.GetCurrentDbVersion().ConfigureAwait(false);

            var migrations = GetInflatedMigrations(assemblyToSearch, currentVersion, targetVersion)
                                      .ToList();

            if (!migrations.Any())
            {
                throw new InvalidOperationException($"No migrations were found. Check 'NeedsMigration' before calling MigrateTo. The current database version is {currentVersion}");
            }

            if (targetVersion.HasValue && !migrations.Any(m => m.Version == targetVersion.Value))
            {
                throw new ArgumentException($"A migration with requested version {targetVersion.Value} was not found");
            }

            var migrationCount = migrations.Count;

            // if the database is empty we can update the version to the latest migration we have without executing the migrations
            if (await _versionProvider.IsCleanDatabase().ConfigureAwait(false))
            {
                await _versionProvider.UpdateDbVersion(migrations.Last().Version).ConfigureAwait(false);
                progress?.Report(new MigrationProgress(MigrationStatus.Success, migrationCount, migrationCount));
                return;
            }

            for (int i = 0; i < migrationCount; i++)
            {
                var migration = migrations[i];
                var oneBasedMigrationIndex = i + 1;
                progress?.Report(new MigrationProgress(MigrationStatus.Migrating, oneBasedMigrationIndex, migrationCount));

                if (await TryExecuteMigration(migration).ConfigureAwait(false))
                {
                    await _versionProvider.UpdateDbVersion(migration.Version).ConfigureAwait(false);
                }
                else
                {
                    progress?.Report(new MigrationProgress(MigrationStatus.Failed, oneBasedMigrationIndex, migrationCount));
                    throw new MigrationFailedException($"Migration to version {migration.Version} has failed");
                }
                if (targetVersion.HasValue && migration.Version == targetVersion.Value) break;
            }
            progress?.Report(new MigrationProgress(MigrationStatus.Success, migrationCount, migrationCount));
        }

        internal async Task<bool> TryExecuteMigration(IMigration migration)
        {
            try
            {
                await migration.Up().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //revert the changes if Up failed
                await TryRollbackMigration(migration).ConfigureAwait(false);
                return false;
            }
        }

        private async Task TryRollbackMigration(IMigration migration)
        {
            try
            {
                await migration.Down().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public Task MigrateToLatest(Assembly assemblyToSearch)
        {
            return MigrateToLatest(assemblyToSearch, null);
        }

        public Task MigrateToLatest(Assembly assemblyToSearch, IProgress<MigrationProgress> progress)
        {
            return MigrateTo(null, assemblyToSearch, progress);
        }


        private IEnumerable<IMigration> GetInflatedMigrations(Assembly assemblyToSearch, int fromVersion)
        {
            return GetInflatedMigrations(assemblyToSearch, fromVersion, null);
        }

        private IEnumerable<IMigration> GetInflatedMigrations(Assembly assemblyToSearch, int fromVersion, int? toVersion)
        {
            return _provider.RetrieveInflatedMigrations(assemblyToSearch, _inflater, fromVersion, toVersion);
        }

        public async Task<bool> NeedsMigration(Assembly assemblyToSearch)
        {
            var currentVersion = await _versionProvider.GetCurrentDbVersion().ConfigureAwait(false);
            var migrations = GetInflatedMigrations(assemblyToSearch, currentVersion);
            return migrations.Any() && migrations.Last().Version > currentVersion;
        }
    }
}

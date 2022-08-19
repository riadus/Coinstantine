using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Data.Migrations;
using SQLite;

namespace Coinstantine.Database.Migrations
{
    [RegisterInterfaceAsDynamic]
    public class SQLiteMigrationVersionProvider : IMigrationVersionProvider
    {
        readonly List<SQLiteAsyncConnection> _connections;

        public SQLiteMigrationVersionProvider(ISqLiteConnectionProvider sqLiteConnectionProvider)
        {
            _connections = new List<SQLiteAsyncConnection>
            {
                sqLiteConnectionProvider.GetConnection(SqliteDatabase.ConnectionType.First),
                sqLiteConnectionProvider.GetConnection(SqliteDatabase.ConnectionType.Second),
                sqLiteConnectionProvider.GetConnection(SqliteDatabase.ConnectionType.Third)
            };
        }

        public async Task UpdateDbVersion(int newVersion)
        {
            foreach (var connection in _connections)
            {
                await connection.ExecuteAsync($"PRAGMA user_version = {newVersion}");
            }
        }

        public Task<int> GetCurrentDbVersion()
        {
            return _connections[0].ExecuteScalarAsync<int>("PRAGMA user_version");
        }

        public async Task<bool> IsCleanDatabase()
        {
            return await GetCurrentDbVersion().ConfigureAwait(false) == 0 && await _connections[1].Table<UserProfile>().CountAsync().ConfigureAwait(false) == 0;
        }
    }
}

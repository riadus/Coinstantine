using System.Threading.Tasks;
using Coinstantine.Data;
using SQLite;
using static Coinstantine.Database.SqliteDatabase;

namespace Coinstantine.Database.Migrations
{
    public class Migration_00001_NewLoginFlow : SQLiteMigration
    {
        public Migration_00001_NewLoginFlow(ISqLiteConnectionProvider sqLiteConnectionProvider) : base(sqLiteConnectionProvider)
        {
        }

        public override Task Down()
        {
            return Task.FromResult(0);
        }

        public override Task Up()
        {
            var connection = GetConnectionWithLock(ConnectionType.Third);
            connection.DropTable<AuthenticationObject>();
            connection.CreateTable<AuthenticationObject>();
            return Task.FromResult(0);
        }

        public SQLiteConnectionWithLock GetConnectionWithLock(ConnectionType connectionType)
        {
            var asyncConnection = GetAsyncConnection(connectionType);
            return asyncConnection.GetConnection();
        }
    }
}

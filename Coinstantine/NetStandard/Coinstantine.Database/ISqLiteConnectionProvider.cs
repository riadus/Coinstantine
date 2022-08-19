using SQLite;
using static Coinstantine.Database.SqliteDatabase;

namespace Coinstantine.Database
{
    public interface ISqLiteConnectionProvider
    {
        SQLiteAsyncConnection GetConnection(ConnectionType connectionType);
    }
}

using Coinstantine.Common.Attributes;
using SQLite;
using static Coinstantine.Database.SqliteDatabase;

namespace Coinstantine.Database
{
    [RegisterInterfaceAsDynamic]
    public class SqLiteConnectionProvider : ISqLiteConnectionProvider
    {
        public SQLiteAsyncConnection GetConnection(ConnectionType connectionType) => GetAsyncConnection(connectionType);
    }
}

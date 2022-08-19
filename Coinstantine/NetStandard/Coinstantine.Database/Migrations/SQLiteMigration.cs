using System.Threading.Tasks;
using Coinstantine.Data.Migrations;
using SQLite;

namespace Coinstantine.Database.Migrations
{
    public abstract class SQLiteMigration : IMigration
    {
        protected ISqLiteConnectionProvider _sqLiteConnectionProvider;
        public int Version { get; private set; }

        public string Description { get; private set; }

        protected SQLiteMigration(ISqLiteConnectionProvider sqLiteConnectionProvider)
        {
            _sqLiteConnectionProvider = sqLiteConnectionProvider;
            var nameParts = GetType().Name.Split('_');
            Version = int.Parse(nameParts[1]);
            Description = nameParts.Length > 2 ? nameParts[2] : string.Empty;
        }

        public abstract Task Up();
        public abstract Task Down();
    }
}

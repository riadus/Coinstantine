using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Data.Migrations;
using SQLite;

namespace Coinstantine.Database.Migrations
{
    [RegisterInterfaceAsDynamic]
    public class SQLiteMigrationInflater : IMigrationInflater
    {
        readonly ISqLiteConnectionProvider _sqLiteConnectionProvider;

        public SQLiteMigrationInflater(ISqLiteConnectionProvider sqLiteConnectionProvider)
        {
            _sqLiteConnectionProvider = sqLiteConnectionProvider;
        }

        public virtual IMigration Inflate(Type type)
        {
            return (SQLiteMigration)Activator.CreateInstance(type, _sqLiteConnectionProvider);
        }
    }
}

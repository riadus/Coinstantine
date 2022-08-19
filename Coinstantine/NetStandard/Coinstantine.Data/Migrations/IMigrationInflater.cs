using System;

namespace Coinstantine.Data.Migrations
{
    public interface IMigrationInflater
    {
        IMigration Inflate(Type type);
    }
}

using System;
using System.Runtime.Serialization;

namespace Coinstantine.Data.Migrations
{
    [Serializable]
    public class MigrationFailedException : Exception
    {
        public MigrationFailedException()
        {
        }

        public MigrationFailedException(string message) : base(message)
        {
        }

        public MigrationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MigrationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

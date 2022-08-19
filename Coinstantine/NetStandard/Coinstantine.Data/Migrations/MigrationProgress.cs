using System;

namespace Coinstantine.Data.Migrations
{
    public class MigrationProgress
    {
        public static MigrationProgress Preparing
        {
            get { return new MigrationProgress(MigrationStatus.Preparing, 0, 0); }
        }

        public MigrationProgress(MigrationStatus status, int currentMigration, int totalMigrations)
        {
            if (status == MigrationStatus.Preparing && currentMigration > 0)
            {
                throw new ArgumentException("We cannot have an active migration while we're preparing", nameof(currentMigration));
            }
            Status = status;
            CurrentMigration = currentMigration;
            TotalMigrations = totalMigrations;
        }

        public int CurrentMigration { get; }
        public int TotalMigrations { get; }

        public MigrationStatus Status { get; }
    }
}

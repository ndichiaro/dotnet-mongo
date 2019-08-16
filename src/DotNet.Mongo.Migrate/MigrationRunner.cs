using DotNet.Mongo.Migrate.Operations;
using DotNet.Mongo.Migrate.Options;
using System;

namespace DotNet.Mongo.Migrate
{
    /// <summary>
    /// A runner class that manages executing the migration
    /// </summary>
    public static class MigrationRunner
    {
        /// <summary>
        /// Executes the migration
        /// </summary>
        /// <param name="options">Migration options</param>
        public static string Run(MigrationOptions options)
        {
            IMigrationOperation migrationOperation;

            switch (options.Operation)
            {
                case MigrationOperation.None:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
                case MigrationOperation.Up:
                    migrationOperation = new UpMigrationOperation(options.Uri.ConnectionString);
                    break;
                case MigrationOperation.Down:
                    migrationOperation = null;
                    break;
                case MigrationOperation.Status:
                    migrationOperation = null;
                    break;
                case MigrationOperation.Create:
                    migrationOperation = new CreateMigrationOperation(options.MigrationName);
                    break;
                default:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
            }

            return migrationOperation.Execute();
        }
    }
}

using DotNet.Mongo.Migrate.Operations;
using DotNet.Mongo.Migrate.Options;
using System;
using System.IO;
using System.Reflection;

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
        /// <exception cref="NotSupportedException">Unsupported Migration Options</exception>
        public static string Run(MigrationOptions options)
        {
            IMigrationOperation migrationOperation;

            var executingLocation = Assembly.GetExecutingAssembly().Location;
            
            switch (options.Operation)
            {
                case MigrationOperation.None:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
                case MigrationOperation.Up:
                    migrationOperation = new UpMigrationOperation(options.Uri.ConnectionString, options.ProjectFile);
                    break;
                case MigrationOperation.Down:
                    migrationOperation = new DownMigrationOperation(options.Uri.ConnectionString, options.ProjectFile);
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

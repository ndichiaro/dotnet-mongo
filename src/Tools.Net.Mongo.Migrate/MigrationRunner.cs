using System;
using Tools.Net.Mongo.Core;
using Tools.Net.Mongo.Core.Builders;
using Tools.Net.Mongo.Migrate.Operations;
using Tools.Net.Mongo.Migrate.Options;

namespace Tools.Net.Mongo.Migrate
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
        public static MigrationResult Run(MigrationOptions options, IMongoDbContextBuilder contextBuilder)
        {
            IMigrationOperation migrationOperation;
            IMongoDbContext dbContext = null;

            if (options.Uri != null)
            {
                dbContext = contextBuilder.Build(options.Uri.ConnectionString);
            }

            switch (options.Operation)
            {
                case MigrationOperation.Up:
                    migrationOperation = new UpMigrationOperation(dbContext, options.ProjectFile);
                    break;
                case MigrationOperation.Down:
                    migrationOperation = new DownMigrationOperation(dbContext, options.ProjectFile);
                    break;
                case MigrationOperation.Status:
                    migrationOperation = new StatusMigrationOperation(dbContext, options.ProjectFile); ;
                    break;
                case MigrationOperation.Create:
                    migrationOperation = new CreateMigrationOperation(options.MigrationName);
                    break;
                case MigrationOperation.None:
                default:
                    return new MigrationResult
                    {
                        Operation = options.Operation,
                        Result = $"{options.Operation} is not a supported operation."
                    };
            }

            var result = migrationOperation.Execute();
            return new MigrationResult
            {
                Operation = options.Operation,
                Result = result
            };
        }
    }
}

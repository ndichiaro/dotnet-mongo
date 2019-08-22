using DotNet.Mongo.Core;
using DotNet.Mongo.Core.Builders;
using DotNet.Mongo.Migrate.Operations;
using DotNet.Mongo.Migrate.Options;
using MongoDB.Driver;
using System;
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
        public static MigrationResult Run(MigrationOptions options, IMongoDbContextBuilder contextBuilder)
        {
            IMigrationOperation migrationOperation;
            IMongoDbContext dbContext = contextBuilder.Build(options.Uri.ConnectionString);

            switch (options.Operation)
            {
                case MigrationOperation.None:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
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
                default:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
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

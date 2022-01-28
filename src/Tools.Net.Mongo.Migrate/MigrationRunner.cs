using MongoDB.Driver;
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
        public static MigrationResult Run(MigrationOptions options)
        {
            IMigrationOperation migrationOperation;
            IMongoDbContext dbContext = null;
            string result;
            var isSuccessful = true;

            try
            {
                if (options.Uri != null)
                {
                    dbContext = new MigrationContext(options.Uri.ConnectionString);
                }
                else
                {
                    if (options.Operation != MigrationOperation.Create
                        && options.Operation != MigrationOperation.None)
                    {
                        return new MigrationResult
                        {
                            Operation = options.Operation,
                            Result = "A vaild MongoDB URI is required. Run --help for more information.",
                            IsSuccessful = false
                        };
                    }
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
                            Result = $"{options.Operation} is not a supported operation.",
                            IsSuccessful = false
                        };
                }
                result = migrationOperation.Execute();
            }
            catch(MongoConfigurationException e)
            {
                result = e.Message;
                isSuccessful = false;
            }

            return new MigrationResult
            {
                Operation = options.Operation,
                Result = result,
                IsSuccessful = isSuccessful
            };
        }
    }
}

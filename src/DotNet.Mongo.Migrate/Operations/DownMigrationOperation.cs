using DotNet.Cli.Driver;
using DotNet.Cli.Driver.Configuration;
using DotNet.Cli.Driver.Options;
using DotNet.Mongo.Core;
using DotNet.Mongo.Migrate.Collections;
using DotNet.Mongo.Migrate.Extensions;
using System;
using System.IO;

namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for downgrading a database instance
    /// </summary>
    public class DownMigrationOperation : IMigrationOperation
    {
        private readonly string _connectionString;
        private readonly string _projectFile;

        /// <summary>
        /// Creates an DownMigrationOperation instance.
        /// </summary>
        /// <param name="connectionString">The MongoDB database connection string</param>
        /// <param name="projectFile">The absolute path the the project file that migrations are stored</param>
        public DownMigrationOperation(string connectionString, string projectFile)
        {
            _connectionString = connectionString;
            _projectFile = projectFile;
        }

        /// <summary>
        /// Executes the migration's down function to downgrade the database
        /// based on it's current changelog
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            var fileInfo = new FileInfo(_projectFile);
            var dbContext = new MongoDbContext(_connectionString);
            
            // check changelog for the latest migration run
            var changeLogCollection = new ChangeLogCollection(dbContext);
            var latestChange = changeLogCollection.GetLatestChange();

            // get all classes in a namespace from project assembly
            var workingDirectory = fileInfo.Directory.FullName;

            // create build command for project where migrations are created
            var runner = CLI.DotNet(x => x.WorkingDirectory = workingDirectory)
                               .Build(x => new BuildCommandOptions
                               {

                                   BuildConfiguration = BuildConfiguration.Debug
                               })
                               .Create();
            // run cli command
            var results = runner.Run();

            if (!results.IsSuccessful) return $"{fileInfo.Name} failed to build with the following errors: {results.Message}";

            var migration = MigrationExtensions.GetMigration(latestChange.FileName, fileInfo);
            if (migration == null) return $"Unable to locate migration {latestChange.FileName}";

            var instance = Activator.CreateInstance(migration);
            var isMigrated = (bool)migration.GetMethod("Down")
                                    .Invoke(instance, new[] { dbContext.Db });

            if (!isMigrated)
                return $"Error: {migration.Name} was not migrated successfully.";

            var deleteResult = changeLogCollection.Delete(latestChange);

            if (deleteResult == 0) return $"Error: {migration.Name} was not downgraded"; 
            return $"Migrated: {migration.Name}";
        }
    }
}

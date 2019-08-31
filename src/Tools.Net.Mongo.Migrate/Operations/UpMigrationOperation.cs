using Tools.Net.Cli.Driver;
using Tools.Net.Mongo.Core;
using Tools.Net.Mongo.Migrate.Collections;
using Tools.Net.Cli.Driver.Configuration;
using System.IO;
using Tools.Net.Cli.Driver.Options;
using System;
using Tools.Net.Mongo.Migrate.Models;
using Tools.Net.Mongo.Migrate.Extensions;
using System.Text;

namespace Tools.Net.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    internal class UpMigrationOperation : IMigrationOperation
    {
        private readonly IMongoDbContext _mongoDbContext;
        private readonly string _projectFile;

        /// <summary>
        /// Creates an UpMigrationOperation instance.
        /// </summary>
        /// <param name="mongoDbContext">The MongoDB database connection</param>
        /// <param name="projectFile">The absolute path the the project file that migrations are stored</param>
        public UpMigrationOperation(IMongoDbContext mongoDbContext, string projectFile)
        {
            _mongoDbContext = mongoDbContext;
            _projectFile = projectFile;
        }

        /// <summary>
        /// Executes the migration's up function to upgrade the database
        /// based on it's current changelog
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            var fileInfo = new FileInfo(_projectFile);
            
            // check changelog for the latest migration run
            var changeLogCollection = new ChangeLogCollection(_mongoDbContext);

            var changeLog = changeLogCollection.All();
            var latestChange = changeLog.GetLatestChange();

            // get all classes in a namespace from project assembly
            var workingDirectory = fileInfo.Directory.FullName;

            // create build command for project where migrations are created
            var runner = CLI.DotNet(x => x.WorkingDirectory = workingDirectory)
                               .Build(x => new BuildCommandOptions
                               {
                                   BuildConfiguration = BuildConfiguration.Debug
                               })
                               .Create();
            // run command
            var results = runner.Run();

            if (!results.IsSuccessful) return $"Error: {fileInfo.Name} failed to build with the following errors: {results.Message}";

            var migrations = MigrationExtensions.GetMigrationTypes(fileInfo);
            if (migrations.Count == 0) return "Error: No migration files found in project";

            var remainingMigrations = migrations.GetRange(0, migrations.Count);

            // grab the latest changes if any migrations were previously executed
            if(latestChange != null)
                remainingMigrations = migrations.GetRemainingMigrations(latestChange.FileName);

            if (remainingMigrations.Count == 0) return "The database is already up to date";
            
            // string build result for multiple migrations
            var migrationResult = new StringBuilder();
            foreach (var migration in remainingMigrations)
            {
                var instance = Activator.CreateInstance(migration);
                var isMigrated = (bool)migration.GetMethod("Up")
                                        .Invoke(instance, new[] { _mongoDbContext.Db });

                if (!isMigrated)
                    return $"Error: {migration.Name} was not migrated successfully";

                changeLogCollection.Insert(new ChangeLog
                {
                    AppliedAt = DateTime.Now,
                    FileName = migration.Name
                });
                migrationResult.AppendLine($"Migrated: {migration.Name}");
            }

            if (migrationResult.Length > 0) return migrationResult.ToString();

            return "Error: Unable to location migrations to be executed. Verify that a Migrations directory exists in your project";
        }
    }
}

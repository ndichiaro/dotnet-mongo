using DotNet.Cli.Driver;
using DotNet.Mongo.Core;
using DotNet.Mongo.Migrate.Collections;
using DotNet.Cli.Driver.Configuration;
using MongoDB.Driver;
using System.IO;
using System.Linq;
using DotNet.Cli.Driver.Options;
using System.Reflection;
using DotNet.Cli.Driver.Tools;
using System.Collections.Generic;
using System;
using DotNet.Mongo.Migrate.Models;
using DotNet.Mongo.Migrate.Extensions;

namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    internal class UpMigrationOperation : IMigrationOperation
    {
        private readonly string _connectionString;
        private readonly string _projectFile;

        /// <summary>
        /// Creates an UpMigrationOperation instance.
        /// </summary>
        /// <param name="connectionString">The MongoDB database connection string</param>
        /// <param name="projectFile">The absolute path the the project file that migrations are stored</param>
        public UpMigrationOperation(string connectionString, string projectFile)
        {
            _connectionString = connectionString;
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
            // run command
            var results = runner.Run();

            if (!results.IsSuccessful) return $"{fileInfo.Name} failed to build with the following errors: {results.Message}";

            var migrations = MigrationExtensions.GetMigrationTypes(fileInfo);
            if (migrations.Count == 0) return "No migration files found in project.";

            var remainingMigrations = migrations.GetRange(0, migrations.Count);

            // grab the latest changes if any migrations were previously executed
            if(latestChange != null)
                remainingMigrations = migrations.GetRemainingMigrations(latestChange.FileName);

            foreach (var migration in remainingMigrations)
            {
                var instance = Activator.CreateInstance(migration);
                var isMigrated = (bool)migration.GetMethod("Up")
                                        .Invoke(instance, new[] { dbContext.Db });

                if (!isMigrated)
                    return $"Error: {migration.Name} was not migrated successfully.";

                changeLogCollection.Insert(new ChangeLog
                {
                    AppliedAt = DateTime.Now,
                    FileName = migration.Name
                });
                return $"Migrated: {migration.Name}";
            }

            return "Unable to location migrations to be executed. Verify that a Migrations directory exists in your project.";
        }
    }
}

﻿using System.IO;
using System.Linq;
using System.Text;
using Tools.Net.Cli.Driver;
using Tools.Net.Cli.Driver.Configuration;
using Tools.Net.Mongo.Migrate.Extensions;
using Tools.Net.Mongo.Migrate.Repositories;

namespace Tools.Net.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for the status of a database
    /// </summary>
    public class StatusMigrationOperation : IMigrationOperation
    {
        private readonly MigrationContext _migrationContext;
        private readonly string _projectFile;

        /// <summary>
        /// Creates an StatusMigrationOperation instance.
        /// </summary>
        /// <param name="migrationContext">The MongoDB database connection</param>
        /// <param name="projectFile">The absolute path the the project file that migrations are stored</param>
        public StatusMigrationOperation(MigrationContext migrationContext, string projectFile)
        {
            _migrationContext = migrationContext;
            _projectFile = projectFile;
        }

        /// <summary>
        /// Shows the status of the database migrations
        /// </summary>
        /// <returns>A csv indicating the database status</returns>
        public string Execute()
        {
            var fileInfo = new FileInfo(_projectFile);

            // check changelog for the latest migration run
            var changeLogCollection = new ChangelogRepository(_migrationContext);

            var changeLog = changeLogCollection.All();
            var latestChange = changeLog.GetLatestChange();

            // get all classes in a namespace from project assembly
            var workingDirectory = fileInfo.Directory.FullName;

            // create build command for project where migrations are created
            var runner = CLI.DotNet(x => x.WorkingDirectory = workingDirectory)
                               .Build(x => x.BuildConfiguration = BuildConfiguration.Debug)
                               .Create();
            // run command
            var results = runner.Run();

            if (!results.IsSuccessful)
            {
                return $"Error: {fileInfo.Name} failed to build with the following errors: {results.Message}";
            }

            var migrations = MigrationExtensions.GetMigrationTypes(fileInfo);
            if (migrations.Count == 0)
            {
                return "Error: No migration files found in project";
            }

            var remainingMigrations = migrations.GetRange(0, migrations.Count);

            // grab the latest changes if any migrations were previously executed
            if (latestChange != null)
            {
                remainingMigrations = migrations.GetRemainingMigrations(latestChange.FileName);
            }

            var migrationStatus = new StringBuilder();
            // add headers to migration status
            migrationStatus.AppendLine("Migration,Applied At");
            // loop through executed migrations by data applied
            foreach (var change in changeLog.OrderBy(x => x.AppliedAt))
            {
                migrationStatus.AppendLine($"{change.FileName},{change.AppliedAt.ToString("MM/dd/yyyy h:mm tt")}");
            }

            // return status if there are no remaining migrations
            if (remainingMigrations == null)
            {
                return migrationStatus.ToString();
            }

            // loop through all pending migrations
            foreach (var migration in remainingMigrations)
            {
                migrationStatus.AppendLine($"{migration.Name},PENDING");
            }
            return migrationStatus.ToString();
        }
    }
}

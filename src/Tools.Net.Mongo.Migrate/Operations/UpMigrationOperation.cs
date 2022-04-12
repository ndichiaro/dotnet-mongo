using System;
using System.IO;
using System.Text;
using Tools.Net.Cli.Driver;
using Tools.Net.Cli.Driver.Configuration;
using Tools.Net.Mongo.Migrate.Respositories;
using Tools.Net.Mongo.Migrate.Extensions;
using Tools.Net.Mongo.Migrate.Logging;
using Tools.Net.Mongo.Migrate.Models;

namespace Tools.Net.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    internal class UpMigrationOperation : IMigrationOperation
    {
        private readonly MigrationContext _migrationContext;
        private readonly string _projectFile;
        private readonly MigrationLogger _logger;

        /// <summary>
        /// Creates an UpMigrationOperation instance.
        /// </summary>
        /// <param name="migrationContext">The MongoDB database connection</param>
        /// <param name="projectFile">The absolute path the the project file that migrations are stored</param>
        public UpMigrationOperation(MigrationContext migrationContext, string projectFile)
        {
            _migrationContext = migrationContext;
            _projectFile = projectFile;
            _logger = MigrationLogger.Instance;
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

            if (remainingMigrations.Count == 0)
            {
                return "The database is already up to date";
            }

            // string build result for multiple migrations
            var migrationResult = new StringBuilder();
            foreach (var migration in remainingMigrations)
            {
                var isMigrated = false;
                string logPath = string.Empty;

                try
                {
                    var instance = Activator.CreateInstance(migration);
                    isMigrated = (bool)migration.GetMethod("Up")
                                            .Invoke(instance, new[] { _migrationContext.Db });
                }
                catch(Exception e)
                {
                    logPath = _logger.Log(migration.Name, e.ToString());
                }

                if (!isMigrated)
                {
                    return $"Error: {migration.Name} was not migrated successfully. See {logPath} for more details.";
                }

                changeLogCollection.Insert(new Changelog
                {
                    AppliedAt = DateTime.Now,
                    FileName = migration.Name
                });
                migrationResult.AppendLine($"Migrated: {migration.Name}");
            }

            if (migrationResult.Length > 0)
            {
                return migrationResult.ToString();
            }

            return "Error: Unable to location migrations to be executed. Verify that a Migrations directory exists in your project";
        }
    }
}

using System;
using System.IO;
using Tools.Net.Cli.Driver;
using Tools.Net.Cli.Driver.Configuration;
using Tools.Net.Mongo.Migrate.Respositories;
using Tools.Net.Mongo.Migrate.Extensions;
using Tools.Net.Mongo.Migrate.Logging;

namespace Tools.Net.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for downgrading a database instance
    /// </summary>
    internal class DownMigrationOperation : IMigrationOperation
    {
        private readonly string _projectFile;
        private readonly MigrationContext _migrationContext;
        private readonly MigrationLogger _logger;

        /// <summary>
        /// Creates an DownMigrationOperation instance.
        /// </summary>
        /// <param name="migrationContext">The MongoDB database connection</param>
        /// <param name="projectFile">The absolute path the the project file that migrations are stored</param>
        public DownMigrationOperation(MigrationContext migrationContext, string projectFile)
        {
            _migrationContext = migrationContext;
            _projectFile = projectFile;
            _logger = MigrationLogger.Instance;
        }

        /// <summary>
        /// Executes the migration's down function to downgrade the database
        /// based on it's current changelog
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            var fileInfo = new FileInfo(_projectFile);
            var isMigrated = false;
            string logPath = string.Empty;

            // check changelog for the latest migration run
            var changeLogCollection = new ChangelogRepository(_migrationContext);

            var changeLog = changeLogCollection.All();
            var latestChange = changeLog.GetLatestChange();

            if (latestChange == null)
            {
                return "No changes to downgrade";
            }

            // get all classes in a namespace from project assembly
            var workingDirectory = fileInfo.Directory.FullName;

            // create build command for project where migrations are created
            var runner = CLI.DotNet(x => x.WorkingDirectory = workingDirectory)
                               .Build(x => x.BuildConfiguration = BuildConfiguration.Debug)
                               .Create();
            // run cli command
            var results = runner.Run();

            if (!results.IsSuccessful)
            {
                return $"Error: {fileInfo.Name} failed to build with the following errors: {results.Message}";
            }

            var migration = MigrationExtensions.GetMigration(latestChange.FileName, fileInfo);
            if (migration == null)
            {
                return $"Error: Unable to locate migration {latestChange.FileName}";
            }

            try
            {
                var instance = Activator.CreateInstance(migration);
                isMigrated = (bool)migration.GetMethod("Down")
                                        .Invoke(instance, new[] { _migrationContext.Db });
            }
            catch (Exception e)
            {
                logPath = _logger.Log(migration.Name, e.ToString());
            }

            if (!isMigrated)
            {
                return $"Error: {migration.Name} was not migrated successfully. See {logPath} for more details.";
            }

            var deleteResult = changeLogCollection.Delete(latestChange);

            if (deleteResult == 0)
            {
                return $"Error: {migration.Name} was not downgraded";
            }

            return $"Downgraded: {migration.Name}";
        }
    }
}

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

namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    public class UpMigrationOperation : IMigrationOperation
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
        /// Executes the migrations up function to upgrade the database
        /// based on it's current changelog
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            var fileInfo = new FileInfo(_projectFile);
            var dbContext = new MongoDbContext(_connectionString);
            
            // check changelog for the latest migration run
            var changeLogCollection = new ChangeLogCollection(dbContext);
            var changeLog = changeLogCollection.All().ToList();
            var latestChange = changeLog.OrderByDescending(x => x.AppliedAt).FirstOrDefault();

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

            var migrations = GetMigrationTypes(fileInfo);
            if (migrations.Count == 0) return "No migration files found in project.";

            var remainingMigrations = migrations.GetRange(0, migrations.Count);

            // grab the latest changes if any migrations were previously executed
            if(latestChange != null)
                remainingMigrations = GetRemainingMigrations(migrations, latestChange.FileName);

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
                
        /// <summary>
        /// Gets Migration Types from the compiled project assembly
        /// </summary>
        /// <param name="fileInfo">The project file directory</param>
        /// <returns>A list of migration types</returns>
        private List<Type> GetMigrationTypes(FileInfo fileInfo)
        {
            // get project framework
            var csProjectFile = CsProjectFileReader.Read(_projectFile);
            var targetFramework = csProjectFile.TargetFramework;
            // there can be multiple target frameworks. split and pick first
            var framework = targetFramework.Split(';')[0];

            var projectDll = Path.Combine(
                new[]
                {
                        fileInfo.DirectoryName,
                        "bin",
                        BuildConfiguration.Debug.ToString(),
                        framework,
                        fileInfo.Name.Replace(fileInfo.Extension, ".dll")
                }
            );

            return Assembly.LoadFrom(projectDll).GetTypes()
                            .Where(x => x.IsClass && x.Namespace == "Migrations")
                            .OrderBy(x => x.Name)
                            .ToList();
        }

        /// <summary>
        /// Filter the migrations list by migrations that were not run
        /// </summary>
        /// <param name="migrations">List of all migrations</param>
        /// <returns>A filter list of migrations</returns>
        private List<Type> GetRemainingMigrations(List<Type> migrations, string latestChange)
        {
            var latestChangeType = migrations.FirstOrDefault(x => x.Name == latestChange);
            // latestChange migration file was not found
            if (latestChangeType == null) return new List<Type>();

            var index = migrations.IndexOf(latestChangeType);

            // get the remaining migrations
            return migrations.GetRange(index + 1, migrations.Count - index);
        }
    }
}

using DotNet.Cli.Driver;
using DotNet.Mongo.Core;
using DotNet.Mongo.Migrate.Collections;
using DotNet.Cli.Driver.Configuration;
using MongoDB.Driver;
using System.IO;
using System.Linq;
using DotNet.Cli.Driver.Options;

namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    public class UpMigrationOperation : IMigrationOperation
    {
        private readonly string _connectionString;
        private readonly string _projectFile;

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
            
            // filter migration files by ones to execute
            var latestChangeIndex = 0;
            if (latestChange != null) latestChangeIndex = changeLog.FindIndex(x => x == latestChange);

            // get all classes in a namespace from project assembly
            var workingDirectory = fileInfo.Directory.FullName;

            var runner = CLI.DotNet(x => x.WorkingDirectory = workingDirectory)
                               .Build(x => new BuildCommandOptions
                               {
                                   BuildConfiguration = BuildConfiguration.Debug
                               }).Create();
            var results = runner.Run();
            return string.Empty;
        }
    }
}

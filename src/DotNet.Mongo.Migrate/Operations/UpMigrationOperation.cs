using DotNet.Mongo.Core;
using DotNet.Mongo.Migrate.Collections;
using DotNet.Mongo.Migrate.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    public class UpMigrationOperation : IMigrationOperation
    {
        private readonly string _connectionString;

        public UpMigrationOperation(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes the migrations up function to upgrade the database
        /// based on it's current changelog
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            // 1. get all migration files from Migration directory
            var files = Directory.GetFiles("Migrations").ToList();

            var dbContext = new MongoDbContext(_connectionString);
            // 2. check changelog for the latest migration run
            var changeLogCollection = new ChangeLogCollection(dbContext);
            var changeLog = changeLogCollection.All().ToList();
            var latestChange = changeLog.OrderByDescending(x => x.AppliedAt).FirstOrDefault();
            
            // 3. filter migration files by ones to execute
            var latestChangeIndex = 0;
            if (latestChange != null) latestChangeIndex = changeLog.FindIndex(x => x == latestChange);

            var migrationFiles = files.GetRange(latestChangeIndex, files.Count - latestChangeIndex);
            // 4. foreach migration file
            foreach (var file in migrationFiles)
            {
                var fileInfo = new FileInfo(file);
                // a. generate c# class
                var code = File.ReadAllText(file);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);

                var assemblyName = Path.GetRandomFileName();

                var references = new MetadataReference[]
                {
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IMongoDatabase).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IMigration).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IEnumerable).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(BsonDocument).GetTypeInfo().Assembly.Location)
                };
                var compilation = CSharpCompilation.Create(
                    assemblyName,
                    syntaxTrees: new[] { syntaxTree },
                    references: references,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                );

                using (var ms = new MemoryStream())
                {
                    var result = compilation.Emit(ms);

                    ms.Seek(0, SeekOrigin.Begin);

                    var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);

                    var className = $"Migrations.{fileInfo.Name.Replace(".cs", string.Empty)}";
                    var type = assembly.GetType(className);
                    var instance = assembly.CreateInstance(className);
                    
                    // b. execute up funciton
                    var upMethod = type.GetMember("Up").First() as MethodInfo;
                    var isSuccessful = (bool)upMethod.Invoke(instance, new[] { dbContext.Db });

                    if (isSuccessful)
                    {
                        changeLogCollection.Insert(new ChangeLog
                        {
                            AppliedAt = DateTime.Now,
                            FileName = fileInfo.Name
                        });
                        return $"{fileInfo.Name} ran successfully";
                    }
                }
            }
            return string.Empty;
        }
    }
}

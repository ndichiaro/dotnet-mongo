using DotNet.Mongo.Migrate.Templates;
using System;
using System.IO;

namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for creating a new migration
    /// </summary>
    internal class CreateMigrationOperation : IMigrationOperation
    {
        private readonly string _migrationName;

        /// <summary>
        /// Creates a CreateMigrationOperation object
        /// </summary>
        /// <param name="migrationName">The name of the migration</param>
        public CreateMigrationOperation(string migrationName)
        {
            _migrationName = migrationName;
        }

        /// <summary>
        /// Creates a new migration file
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            var className = $"M{DateTime.Now.ToString("yyyyMMddHHmmssf")}_{_migrationName}";
            var migrationTemplate = new MigrationTemplate(className);
            var classContent = migrationTemplate.TransformText();

            var filePath = $"Migrations/{className}.cs";
            var classFile = new FileInfo(filePath);
            classFile.Directory.Create();
            File.WriteAllText(classFile.FullName, classContent);

            if (File.Exists(filePath))
                return $"Created: {filePath}";

            return $"Error: {filePath} was not created sucessfully";
        }
    }
}

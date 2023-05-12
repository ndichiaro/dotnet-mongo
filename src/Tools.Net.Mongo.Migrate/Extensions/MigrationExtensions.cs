using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tools.Net.Cli.Driver.Configuration;
using Tools.Net.Cli.Driver.Tools;

namespace Tools.Net.Mongo.Migrate.Extensions
{
    /// <summary>
    /// Provides additional migration functionality
    /// </summary>
    internal static class MigrationExtensions
    {
        /// <summary>
        /// Gets a migration type from a compiled project assembly
        /// </summary>
        /// <param name="migration">The name of the migration</param>
        /// <param name="fileInfo">The project file directory</param>
        /// <returns></returns>
        public static Type GetMigration(string migration, FileInfo fileInfo)
        {
            return GetMigrationTypes(fileInfo).FirstOrDefault(x => x.Name == migration);
        }

        /// <summary>
        /// Gets Migration Types from a compiled project assembly
        /// </summary>
        /// <param name="fileInfo">The project file directory</param>
        /// <returns>A list of migration types</returns>
        public static List<Type> GetMigrationTypes(FileInfo fileInfo)
        {
            // get project framework
            var csProjectFile = CsProjectFileReader.Read(fileInfo.FullName);
            var framework = csProjectFile.GetLatestTargetFramework();

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
                        .Where(x => x.IsClass && x.Namespace == "Migrations" && x.IsPublic)
                        .OrderBy(x => x.Name)
                        .ToList();
        }

        /// <summary>
        /// Filter the migrations list by migrations that were not run
        /// </summary>
        /// <param name="migrations">List of all migrations</param>
        /// <returns>A filter list of migrations</returns>
        public static List<Type> GetRemainingMigrations(this List<Type> migrations, string latestChange)
        {
            var latestChangeType = migrations.FirstOrDefault(x => x.Name == latestChange);
            // latestChange migration file was not found
            if (latestChangeType == null)
            {
                return new List<Type>();
            }

            var index = migrations.IndexOf(latestChangeType);

            // get the remaining migrations
            var position = index + 1;
            return migrations.GetRange(position, migrations.Count - position);
        }
    }
}

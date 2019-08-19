using DotNet.Mongo.Migrate.Collections;
using DotNet.Mongo.Migrate.Models;
using System.Linq;

namespace DotNet.Mongo.Migrate.Extensions
{
    /// <summary>
    /// Provides additional change log functionality
    /// </summary>
    internal static class ChangeLogExtension
    {
        /// <summary>
        /// Gets the latest change
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns>The latest migration executed</returns>
        public static ChangeLog GetLatestChange(this ChangeLogCollection collection)
        {
            var changeLog = collection.All().ToList();
            return changeLog.OrderByDescending(x => x.AppliedAt).FirstOrDefault();
        }
    }
}

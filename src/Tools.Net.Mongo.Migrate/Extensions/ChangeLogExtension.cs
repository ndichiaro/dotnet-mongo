using Tools.Net.Mongo.Migrate.Collections;
using Tools.Net.Mongo.Migrate.Models;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Net.Mongo.Migrate.Extensions
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
        public static ChangeLog GetLatestChange(this IEnumerable<ChangeLog> changeLog)
        {
            return changeLog.OrderByDescending(x => x.AppliedAt).FirstOrDefault();
        }
    }
}

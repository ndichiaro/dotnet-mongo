using System.Collections.Generic;
using System.Linq;
using Tools.Net.Mongo.Migrate.Models;

namespace Tools.Net.Mongo.Migrate.Extensions
{
    /// <summary>
    /// Provides additional change log functionality
    /// </summary>
    public static class ChangelogExtension
    {
        /// <summary>
        /// Gets the latest change
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns>The latest migration executed</returns>
        public static Changelog GetLatestChange(this IEnumerable<Changelog> changeLog)
        {
            return changeLog.OrderByDescending(x => x.AppliedAt).FirstOrDefault();
        }
    }
}

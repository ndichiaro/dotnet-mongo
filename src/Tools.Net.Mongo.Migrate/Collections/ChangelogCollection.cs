using MongoDB.Driver;
using Tools.Net.Mongo.Core;
using Tools.Net.Mongo.Migrate.Models;

namespace Tools.Net.Mongo.Migrate.Collections
{
    /// <summary>
    /// Manages the interaction for the changelog MongoDB
    /// collection
    /// </summary>
    internal class ChangelogCollection : MongoDbCollection<Changelog>
    {
        /// <summary>
        /// Creates an instance of ChangeLogCollection
        /// </summary>
        /// <param name="context">MongoDB database context</param>
        public ChangelogCollection(IMongoDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Deletes a change log
        /// </summary>
        /// <param name="changelog">The change log to be deleted</param>
        /// <returns></returns>
        public long Delete(Changelog changelog)
        {
            return Delete(x => x.Id, changelog.Id);
        }
    }
}

using MongoDB.Driver;
using Tools.Net.Mongo.Core;
using Tools.Net.Mongo.Migrate.Models;

namespace Tools.Net.Mongo.Migrate.Collections
{
    /// <summary>
    /// Manages the interaction for the changeLog MongoDB
    /// collection
    /// </summary>
    internal class ChangeLogCollection : MongoDbCollection<ChangeLog>
    {
        /// <summary>
        /// Creates an instance of ChangeLogCollection
        /// </summary>
        /// <param name="context">MongoDB database context</param>
        public ChangeLogCollection(IMongoDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Deletes a change log
        /// </summary>
        /// <param name="changeLog">The change log to be deleted</param>
        /// <returns></returns>
        public long Delete(ChangeLog changeLog)
        {
            var filterDefinition = Builders<ChangeLog>.Filter.Eq(x => x.Id, changeLog.Id);
            return Delete(filterDefinition);
        }
    }
}

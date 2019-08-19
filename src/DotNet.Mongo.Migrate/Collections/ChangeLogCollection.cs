using System;
using DotNet.Mongo.Core;
using DotNet.Mongo.Migrate.Models;
using MongoDB.Driver;

namespace DotNet.Mongo.Migrate.Collections
{
    /// <summary>
    /// Manages the interaction for the changeLog MongoDB
    /// collection
    /// </summary>
    public class ChangeLogCollection : MongoDbCollection<ChangeLog>
    {
        public ChangeLogCollection(IMongoDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Deletes a change log
        /// </summary>
        /// <param name="changeLog">The change log to be deleted</param>
        /// <returns></returns>
        internal long Delete(ChangeLog changeLog)
        {
            var filterDefinition = Builders<ChangeLog>.Filter.Eq(x => x.Id, changeLog.Id);
            return Delete(filterDefinition);
        }
    }
}

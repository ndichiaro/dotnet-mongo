using DotNet.Mongo.Core;
using DotNet.Mongo.Migrate.Models;

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
    }
}

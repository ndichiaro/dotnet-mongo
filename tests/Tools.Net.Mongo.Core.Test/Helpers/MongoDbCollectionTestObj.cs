using MongoDB.Driver;

namespace Tools.Net.Mongo.Core.Test.Helpers
{
    /// <summary>
    /// A class used for testing the funcitonality of the 
    /// abstract MongoDbCollection class
    /// </summary>
    public class MongoDbCollectionTestObj : MongoDbCollection<TestEntity>
    {
        /// <summary>
        /// Exposes the protected collection name for testing purposes
        /// </summary>
        public string CollectionNameTestProperty => CollectionName;

        /// <summary>
        /// Creates a MongoDbCollectionTestObj instance
        /// </summary>
        /// <param name="context"></param>
        public MongoDbCollectionTestObj(IMongoDbContext context) : base(context)
        {
        }

        /// <summary>
        /// A wrapper method to enable testing for the Collection 
        /// property
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<TestEntity> GetMongoCollection()
        {
            return Collection;
        }
    }
}

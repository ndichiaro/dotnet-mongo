using Tools.Net.Mongo.Core.Test.Helpers;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace Tools.Net.Mongo.Core.Test
{
    /// <summary>
    /// A suite of unit tests to validate the functionality of 
    /// the MongoDbCollection class
    /// </summary>
    public class MongoDbCollectionTests
    {
        #region Variables
        private readonly IMongoDbContext _context;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<TestEntity> _mongoCollection;
        private readonly MongoDbCollectionTestObj _testCollection;
        #endregion

        #region Constructors
        public MongoDbCollectionTests()
        {
            _context = Substitute.For<IMongoDbContext>();
            _mongoDatabase = Substitute.For<IMongoDatabase>();
            _mongoCollection = Substitute.For<IMongoCollection<TestEntity>>();
            _testCollection = new MongoDbCollectionTestObj(_context);
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Tests that the colleciton name is built from 
        /// the entity class name
        /// </summary>
        [Fact]
        public void CanBuildCollectionNameFromEntity()
        {
            const string expectedName = "testEntity";

            var collectionName = _testCollection.CollectionName;

            Assert.Equal(expectedName, collectionName);
        }

        /// <summary>
        /// Tests the a mongo collection instance can be created for the 
        /// testEntity
        /// </summary>
        [Fact]
        public void CanGetCollection()
        {
            _context.Db.Returns(_mongoDatabase);
            _mongoDatabase.GetCollection<TestEntity>(Arg.Is<string>("testEntity")).Returns(_mongoCollection);

            var collection = _testCollection.GetMongoCollection();

            Assert.Equal(_mongoCollection, collection);
        }
        #endregion
    }
}

using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace Tools.Net.Mongo.Core.Test
{
    /// <summary>
    /// A suite of unit tests to validate the functionality of 
    /// the MongoDbContext class
    /// </summary>
    public class MongoDbContextTests
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;

        /// <summary>
        /// Provides test setup
        /// </summary>
        public MongoDbContextTests()
        {
            _mongoClient = Substitute.For<IMongoClient>();
            _mongoDatabase = Substitute.For<IMongoDatabase>();
        }

        /// <summary>
        /// Validates a MongoDbInstance can be created with a 
        /// Db instance
        /// </summary>
        [Fact]
        public void CanGetDbInstance()
        {
            const string databaseName = "testDatabase";

            _mongoClient.GetDatabase(databaseName).Returns(_mongoDatabase);
            var dbContext = new MongoDbContext(_mongoClient, databaseName);

            var result = dbContext.Db;

            Assert.Equal(_mongoDatabase, result);
        }
    }
}

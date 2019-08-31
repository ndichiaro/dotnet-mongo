using System;
using Tools.Net.Mongo.Core.Builders;
using Xunit;

namespace Tools.Net.Mongo.Core.Test.Builders
{
    /// <summary>
    /// A suite of unit tests to validate the functionality of
    /// the MongoDbContextBuilder class
    /// </summary>
    public class MongoDbContextBuilderTest
    {
        #region Variables
        private readonly IMongoDbContextBuilder _mongoDbContextBuilder;
        #endregion

        #region Constructors
        public MongoDbContextBuilderTest()
        {
            _mongoDbContextBuilder = new MongoDbContextBuilder();
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Tests that when a null or empty string is passed and a parameter to 
        /// build a db context that an ArgumentNullException is thrown
        /// </summary>
        /// <param name="connectionString">Test parameter</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void HandleNullOrEmptyConnectionString(string connectionString)
        {
            Assert.Throws<ArgumentNullException>(() => _mongoDbContextBuilder.Build(connectionString));
        }

        /// <summary>
        /// Tests the a MongoDbContext instance can be created successfully
        /// </summary>
        [Fact]
        public void CanBuildMongoDbContext()
        {
            const string connectionString = "mongodb://localhost:27017/testDb";

            var result = _mongoDbContextBuilder.Build(connectionString);

            Assert.IsType<MongoDbContext>(result);
        }
        #endregion
    }
}

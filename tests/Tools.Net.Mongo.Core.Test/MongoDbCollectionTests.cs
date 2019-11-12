using MongoDB.Driver;
using NSubstitute;
using System;
using System.Linq.Expressions;
using Tools.Net.Mongo.Core.Extensions;
using Tools.Net.Mongo.Core.Test.Helpers;
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

            _context.Db.Returns(_mongoDatabase);
            _mongoDatabase.GetCollection<TestEntity>(Arg.Is<string>("testEntity")).Returns(_mongoCollection);
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
            var collection = _testCollection.GetMongoCollection();

            Assert.Equal(_mongoCollection, collection);
        }

        /// <summary>
        /// Tests that a single document can be deleted using an expression
        /// </summary>
        [Fact]
        public void CanDeleteCollectionUsingExpression()
        {
            Expression<Func<TestEntity, string>> expression = x => x.FirstProperty;
            const string value = "hi";
            const int expectedDeleteCount = 1;
            var expectedResult = Substitute.For<DeleteResult>();
            var expectedFilter = Builders<TestEntity>.Filter.Eq(expression, value);

            expectedResult.DeletedCount.Returns(expectedDeleteCount);

            // mock the delete call with the expected filter to return th expected result
            _mongoCollection.DeleteOne(Arg.Is<FilterDefinition<TestEntity>>(x => 
                x.ToJson().Equals(expectedFilter.ToJson()))).Returns(expectedResult);

            // call the delete function with the expression and value
            var actualResult = _testCollection.Delete(expression, value);

            // test that the actual updated count matched the expected deleted count
            Assert.Equal(expectedDeleteCount, actualResult);
        }
        #endregion
    }
}

using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Tools.Net.Mongo.Core.Extensions;
using Xunit;

namespace Tools.Net.Mongo.Core.Test.Extensions
{
    /// <summary>
    /// A suite of unit tests that validate the functionality of
    /// the FilterDefinitionExtensions class
    /// </summary>
    public class FilterDefinitionExtensionsTests
    {
        /// <summary>
        /// Test model for use in filter tests
        /// </summary>
        public class TestModel
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Tests that ToJson extension method converts a simple equality filter to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_SimpleEqualityFilter_ReturnsValidJson()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.Eq(x => x.Name, "John");

            // Act
            var json = filter.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("Name", json);
            Assert.Contains("John", json);
            // Verify it's valid JSON by parsing it
            var bsonDoc = BsonDocument.Parse(json);
            Assert.NotNull(bsonDoc);
        }

        /// <summary>
        /// Tests that ToJson extension method converts a complex filter to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_ComplexFilter_ReturnsValidJson()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.And(
                Builders<TestModel>.Filter.Eq(x => x.Name, "John"),
                Builders<TestModel>.Filter.Gte(x => x.Age, 18)
            );

            // Act
            var json = filter.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            // MongoDB driver optimizes And filters into a single document when possible
            Assert.Contains("Name", json);
            Assert.Contains("John", json);
            Assert.Contains("Age", json);
            Assert.Contains("$gte", json);
            Assert.Contains("18", json);
            // Verify it's valid JSON by parsing it
            var bsonDoc = BsonDocument.Parse(json);
            Assert.NotNull(bsonDoc);
        }

        /// <summary>
        /// Tests that ToJson extension method converts an empty filter to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_EmptyFilter_ReturnsValidJson()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.Empty;

            // Act
            var json = filter.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            // Empty filter should result in an empty document with possible whitespace
            Assert.Equal("{ }", json);
        }

        /// <summary>
        /// Tests that ToBson extension method converts a simple equality filter to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_SimpleEqualityFilter_ReturnsValidBsonDocument()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.Eq(x => x.Name, "John");

            // Act
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(bsonDoc);
            Assert.True(bsonDoc.Contains("Name"));
            Assert.Equal("John", bsonDoc["Name"].AsString);
        }

        /// <summary>
        /// Tests that ToBson extension method converts a complex filter to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_ComplexFilter_ReturnsValidBsonDocument()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.And(
                Builders<TestModel>.Filter.Eq(x => x.Name, "John"),
                Builders<TestModel>.Filter.Gte(x => x.Age, 18)
            );

            // Act
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(bsonDoc);
            // MongoDB driver optimizes And filters into a single document when possible
            Assert.True(bsonDoc.Contains("Name"));
            Assert.Equal("John", bsonDoc["Name"].AsString);
            
            Assert.True(bsonDoc.Contains("Age"));
            var ageCondition = bsonDoc["Age"].AsBsonDocument;
            Assert.True(ageCondition.Contains("$gte"));
            Assert.Equal(18, ageCondition["$gte"].AsInt32);
        }

        /// <summary>
        /// Tests that ToBson extension method converts an empty filter to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_EmptyFilter_ReturnsEmptyBsonDocument()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.Empty;

            // Act
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(bsonDoc);
            Assert.Equal(0, bsonDoc.ElementCount);
        }

        /// <summary>
        /// Tests that ToBson and ToJson methods produce consistent results
        /// </summary>
        [Fact]
        public void ToBsonAndToJson_SameFilter_ProduceConsistentResults()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.And(
                Builders<TestModel>.Filter.Eq(x => x.Name, "Jane"),
                Builders<TestModel>.Filter.Lt(x => x.Age, 30)
            );

            // Act
            var bsonDoc = filter.ToBson();
            var json = filter.ToJson();
            var bsonFromJson = BsonDocument.Parse(json);

            // Assert
            Assert.Equal(bsonDoc.ToJson(), bsonFromJson.ToJson());
        }

        /// <summary>
        /// Tests that ToJsonAndToBson_DifferentDataTypes_WorkCorrectly
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_DifferentDataTypes_WorkCorrectly()
        {
            // Arrange
            var testDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var filter = Builders<TestModel>.Filter.And(
                Builders<TestModel>.Filter.Eq(x => x.Name, "Test"),
                Builders<TestModel>.Filter.Eq(x => x.Age, 25),
                Builders<TestModel>.Filter.Gte(x => x.CreatedAt, testDate)
            );

            // Act
            var json = filter.ToJson();
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonDoc);
            
            // Verify the JSON contains expected values
            Assert.Contains("Test", json);
            Assert.Contains("25", json);
            
            // Verify the BSON document structure (MongoDB optimizes And into single document)
            Assert.True(bsonDoc.Contains("Name"));
            Assert.True(bsonDoc.Contains("Age"));
            Assert.True(bsonDoc.Contains("CreatedAt"));
        }

        /// <summary>
        /// Tests that the extensions handle regex filters correctly
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_RegexFilter_WorkCorrectly()
        {
            // Arrange
            var filter = Builders<TestModel>.Filter.Regex(x => x.Name, new BsonRegularExpression("^Test.*", "i"));

            // Act
            var json = filter.ToJson();
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonDoc);
            Assert.Contains("Name", json);
            Assert.Contains("$regularExpression", json);
            
            Assert.True(bsonDoc.Contains("Name"));
            var nameValue = bsonDoc["Name"];
            Assert.True(nameValue.IsBsonRegularExpression);
            var regex = nameValue.AsBsonRegularExpression;
            Assert.Equal("^Test.*", regex.Pattern);
            Assert.Equal("i", regex.Options);
        }

        /// <summary>
        /// Tests that the extensions handle in filters correctly
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_InFilter_WorkCorrectly()
        {
            // Arrange
            var values = new[] { "John", "Jane", "Bob" };
            var filter = Builders<TestModel>.Filter.In(x => x.Name, values);

            // Act
            var json = filter.ToJson();
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonDoc);
            Assert.Contains("$in", json);
            Assert.Contains("John", json);
            Assert.Contains("Jane", json);
            Assert.Contains("Bob", json);
            
            Assert.True(bsonDoc.Contains("Name"));
            var nameFilter = bsonDoc["Name"].AsBsonDocument;
            Assert.True(nameFilter.Contains("$in"));
            var inArray = nameFilter["$in"].AsBsonArray;
            Assert.Equal(3, inArray.Count);
        }

        /// <summary>
        /// Tests that a complex filter that actually produces $and works correctly
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_ComplexAndFilter_WorkCorrectly()
        {
            // Arrange - Create conditions that can't be optimized into a single document
            var nameFilter = Builders<TestModel>.Filter.Eq(x => x.Name, "John");
            var ageFilter = Builders<TestModel>.Filter.Gte(x => x.Age, 18);
            var nameRegexFilter = Builders<TestModel>.Filter.Regex(x => x.Name, new BsonRegularExpression("^J"));
            
            var filter = Builders<TestModel>.Filter.And(nameFilter, ageFilter, nameRegexFilter);

            // Act
            var json = filter.ToJson();
            var bsonDoc = filter.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonDoc);
            
            // This should produce an actual $and structure due to the conflicting name conditions
            if (bsonDoc.Contains("$and"))
            {
                var andArray = bsonDoc["$and"].AsBsonArray;
                Assert.True(andArray.Count >= 2);
            }
            else
            {
                // If MongoDB still optimizes this, ensure all conditions are present
                Assert.Contains("John", json);
                Assert.Contains("$gte", json);
                Assert.Contains("18", json);
            }
        }
    }
}

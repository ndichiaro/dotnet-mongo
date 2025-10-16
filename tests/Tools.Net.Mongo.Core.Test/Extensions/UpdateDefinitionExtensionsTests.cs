using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Tools.Net.Mongo.Core.Extensions;
using Xunit;

namespace Tools.Net.Mongo.Core.Test.Extensions
{
    /// <summary>
    /// A suite of unit tests that validate the functionality of
    /// the UpdateDefinitionExtensions class
    /// </summary>
    public class UpdateDefinitionExtensionsTests
    {
        /// <summary>
        /// Test model for use in update tests
        /// </summary>
        public class TestModel
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public double Score { get; set; }
            public DateTime CreatedAt { get; set; }
            public string[] Tags { get; set; } = Array.Empty<string>();
        }

        /// <summary>
        /// Tests that ToJson extension method converts a simple set update to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_SimpleSetUpdate_ReturnsValidJson()
        {
            // Arrange
            var update = Builders<TestModel>.Update.Set(x => x.Name, "John");

            // Act
            var json = update.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("$set", json);
            Assert.Contains("Name", json);
            Assert.Contains("John", json);
            // Verify it's valid JSON by parsing it
            var bsonDoc = BsonDocument.Parse(json);
            Assert.NotNull(bsonDoc);
        }

        /// <summary>
        /// Tests that ToJson extension method converts multiple set operations to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_MultipleSetOperations_ReturnsValidJson()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Set(x => x.Name, "Jane")
                .Set(x => x.Age, 25);

            // Act
            var json = update.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("$set", json);
            Assert.Contains("Name", json);
            Assert.Contains("Jane", json);
            Assert.Contains("Age", json);
            Assert.Contains("25", json);
            // Verify it's valid JSON by parsing it
            var bsonDoc = BsonDocument.Parse(json);
            Assert.NotNull(bsonDoc);
        }

        /// <summary>
        /// Tests that ToJson extension method converts increment operations to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_IncrementOperation_ReturnsValidJson()
        {
            // Arrange
            var update = Builders<TestModel>.Update.Inc(x => x.Age, 5);

            // Act
            var json = update.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("$inc", json);
            Assert.Contains("Age", json);
            Assert.Contains("5", json);
            // Verify it's valid JSON by parsing it
            var bsonDoc = BsonDocument.Parse(json);
            Assert.NotNull(bsonDoc);
        }

        /// <summary>
        /// Tests that ToJson extension method converts combined operations to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_CombinedOperations_ReturnsValidJson()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Set(x => x.Name, "Bob")
                .Inc(x => x.Age, 3)
                .CurrentDate(x => x.CreatedAt);

            // Act
            var json = update.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("$set", json);
            Assert.Contains("$inc", json);
            Assert.Contains("$currentDate", json);
            Assert.Contains("Name", json);
            Assert.Contains("Bob", json);
            Assert.Contains("Age", json);
            Assert.Contains("3", json);
            Assert.Contains("CreatedAt", json);
            // Verify it's valid JSON by parsing it
            var bsonDoc = BsonDocument.Parse(json);
            Assert.NotNull(bsonDoc);
        }

        /// <summary>
        /// Tests that ToJson extension method converts empty update to JSON correctly
        /// </summary>
        [Fact]
        public void ToJson_EmptyUpdate_ReturnsValidJson()
        {
            // Arrange
            var update = Builders<TestModel>.Update.Combine();

            // Act
            var json = update.ToJson();

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Equal("{ }", json);
        }

        /// <summary>
        /// Tests that ToBson extension method converts a simple set update to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_SimpleSetUpdate_ReturnsValidBsonValue()
        {
            // Arrange
            var update = Builders<TestModel>.Update.Set(x => x.Name, "John");

            // Act
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(bsonValue);
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            Assert.True(bsonDoc.Contains("$set"));
            var setDoc = bsonDoc["$set"].AsBsonDocument;
            Assert.True(setDoc.Contains("Name"));
            Assert.Equal("John", setDoc["Name"].AsString);
        }

        /// <summary>
        /// Tests that ToBson extension method converts multiple set operations to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_MultipleSetOperations_ReturnsValidBsonValue()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Set(x => x.Name, "Jane")
                .Set(x => x.Age, 30);

            // Act
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(bsonValue);
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            Assert.True(bsonDoc.Contains("$set"));
            var setDoc = bsonDoc["$set"].AsBsonDocument;
            Assert.True(setDoc.Contains("Name"));
            Assert.True(setDoc.Contains("Age"));
            Assert.Equal("Jane", setDoc["Name"].AsString);
            Assert.Equal(30, setDoc["Age"].AsInt32);
        }

        /// <summary>
        /// Tests that ToBson extension method converts combined operations to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_CombinedOperations_ReturnsValidBsonValue()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Set(x => x.Name, "Alice")
                .Inc(x => x.Age, 2)
                .Mul(x => x.Score, 1.5);

            // Act
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(bsonValue);
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            
            // Verify $set operation
            Assert.True(bsonDoc.Contains("$set"));
            var setDoc = bsonDoc["$set"].AsBsonDocument;
            Assert.True(setDoc.Contains("Name"));
            Assert.Equal("Alice", setDoc["Name"].AsString);
            
            // Verify $inc operation
            Assert.True(bsonDoc.Contains("$inc"));
            var incDoc = bsonDoc["$inc"].AsBsonDocument;
            Assert.True(incDoc.Contains("Age"));
            Assert.Equal(2, incDoc["Age"].AsInt32);
            
            // Verify $mul operation
            Assert.True(bsonDoc.Contains("$mul"));
            var mulDoc = bsonDoc["$mul"].AsBsonDocument;
            Assert.True(mulDoc.Contains("Score"));
            Assert.Equal(1.5, mulDoc["Score"].AsDouble);
        }

        /// <summary>
        /// Tests that ToBson extension method converts empty update to BSON correctly
        /// </summary>
        [Fact]
        public void ToBson_EmptyUpdate_ReturnsEmptyBsonDocument()
        {
            // Arrange
            var update = Builders<TestModel>.Update.Combine();

            // Act
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(bsonValue);
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            Assert.Equal(0, bsonDoc.ElementCount);
        }

        /// <summary>
        /// Tests that ToBson and ToJson methods produce consistent results
        /// </summary>
        [Fact]
        public void ToBsonAndToJson_SameUpdate_ProduceConsistentResults()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Set(x => x.Name, "Test")
                .Inc(x => x.Age, 1);

            // Act
            var bsonValue = update.ToBson();
            var json = update.ToJson();
            var bsonFromJson = BsonDocument.Parse(json);

            // Assert
            Assert.Equal(bsonValue.ToJson(), bsonFromJson.ToJson());
        }

        /// <summary>
        /// Tests that the extensions work with array operations
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_ArrayOperations_WorkCorrectly()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Push(x => x.Tags, "new-tag")
                .Set(x => x.Name, "Updated");

            // Act
            var json = update.ToJson();
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonValue);
            Assert.Contains("$push", json);
            Assert.Contains("$set", json);
            Assert.Contains("Tags", json);
            Assert.Contains("new-tag", json);
            Assert.Contains("Updated", json);
            
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            Assert.True(bsonDoc.Contains("$push"));
            Assert.True(bsonDoc.Contains("$set"));
        }

        /// <summary>
        /// Tests that the extensions handle unset operations correctly
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_UnsetOperation_WorkCorrectly()
        {
            // Arrange
            var update = Builders<TestModel>.Update
                .Unset(x => x.Score)
                .Set(x => x.Name, "NoScore");

            // Act
            var json = update.ToJson();
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonValue);
            Assert.Contains("$unset", json);
            Assert.Contains("$set", json);
            Assert.Contains("Score", json);
            Assert.Contains("NoScore", json);
            
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            Assert.True(bsonDoc.Contains("$unset"));
            Assert.True(bsonDoc.Contains("$set"));
            
            var unsetDoc = bsonDoc["$unset"].AsBsonDocument;
            Assert.True(unsetDoc.Contains("Score"));
        }

        /// <summary>
        /// Tests that the extensions handle different data types correctly
        /// </summary>
        [Fact]
        public void ToJsonAndToBson_DifferentDataTypes_WorkCorrectly()
        {
            // Arrange
            var testDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var update = Builders<TestModel>.Update
                .Set(x => x.Name, "TypeTest")
                .Set(x => x.Age, 42)
                .Set(x => x.Score, 95.5)
                .Set(x => x.CreatedAt, testDate);

            // Act
            var json = update.ToJson();
            var bsonValue = update.ToBson();

            // Assert
            Assert.NotNull(json);
            Assert.NotNull(bsonValue);
            
            // Verify JSON contains expected values
            Assert.Contains("TypeTest", json);
            Assert.Contains("42", json);
            Assert.Contains("95.5", json);
            
            // Verify BSON structure
            Assert.True(bsonValue.IsBsonDocument);
            var bsonDoc = bsonValue.AsBsonDocument;
            Assert.True(bsonDoc.Contains("$set"));
            var setDoc = bsonDoc["$set"].AsBsonDocument;
            
            Assert.Equal("TypeTest", setDoc["Name"].AsString);
            Assert.Equal(42, setDoc["Age"].AsInt32);
            Assert.Equal(95.5, setDoc["Score"].AsDouble);
            Assert.Equal(testDate, setDoc["CreatedAt"].ToUniversalTime());
        }
    }
}

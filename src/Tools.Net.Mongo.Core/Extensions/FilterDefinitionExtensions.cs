using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Tools.Net.Mongo.Core.Extensions
{
    /// <summary>
    /// A set of extension methods for the MongoDB Driver class
    /// FilterDefinition
    /// </summary>
    public static class FilterDefinitionExtensions
    {
        /// <summary>
        /// Converts a FilterDefinition to a JSON string
        /// </summary>
        /// <typeparam name="T">The Type used in the FilterDefinition</typeparam>
        /// <param name="filter">The filter to be serialized</param>
        /// <returns>A JSON string</returns>
        public static string ToJson<T>(this FilterDefinition<T> filter)
        {
            return filter.ToBson<T>().ToJson();
        }

        /// <summary>
        /// Converts a FilterDefinition to a BSON Document
        /// </summary>
        /// <typeparam name="T">The Type used in the FilterDefinition</typeparam>
        /// <param name="filter">The filter to converted</param>
        /// <returns>A BSON Document representing the filter</returns>
        public static BsonDocument ToBson<T>(this FilterDefinition<T> filter)
        {
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<T>();

            var renderArgs = new RenderArgs<T>(documentSerializer, serializerRegistry);
            return filter.Render(renderArgs);
        }
    }
}

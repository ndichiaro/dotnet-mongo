using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Tools.Net.Mongo.Core.Extensions
{
    /// <summary>
    /// A set of extension methods for the MongoDB Driver class
    /// UpdateDefinition
    /// </summary>
    public static class UpdateDefinitionExtensions
    {
        /// <summary>
        /// Converts a UpdateDefinition to a JSON string
        /// </summary>
        /// <typeparam name="T">The Type used in the UpdateDefinition</typeparam>
        /// <param name="definition">The definition to be serialized</param>
        /// <returns>A JSON string</returns>
        public static string ToJson<T>(this UpdateDefinition<T> definition)
        {
            return definition.ToBson<T>().ToJson();
        }

        /// <summary>
        /// Converts a UpdateDefinition to a BSON Document
        /// </summary>
        /// <typeparam name="T">The Type used in the UpdateDefinition</typeparam>
        /// <param name="definition">The definition to converted</param>
        /// <returns>A BSON value representing the definition</returns>
        public static BsonValue ToBson<T>(this UpdateDefinition<T> definition)
        {
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<T>();
            return definition.Render(documentSerializer, serializerRegistry);
        }
    }
}

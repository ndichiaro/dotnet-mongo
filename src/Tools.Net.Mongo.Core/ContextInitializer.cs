using MongoDB.Driver;
using System.Linq;

namespace Tools.Net.Mongo.Core
{
    internal static class ContextInitializer
    {
        /// <summary>
        /// Initialize a <see cref="MongoDbContext"/> instance and it's <see cref="IMongoCollection{TDocument}"/> properties.
        /// </summary>
        /// <param name="context">The <see cref="MongoDbContext"/> instance being initialized.</param>
        /// <param name="database">An <see cref="IMongoDatabase"/> instance to link context properies to the database instance colletions.</param>
        internal static void Initialize(this MongoDbContext context, IMongoDatabase database)
        {
            var properties = context.GetType()
                                    .GetProperties()
                                    .Where(p => p.PropertyType.IsGenericType && 
                                                p.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>))
                                    .ToList();

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType.GenericTypeArguments[0];

                var collectionName = CollectionNameGenerator.Generate(property);

                var getCollectionMethod = database.GetType().GetMethod(nameof(database.GetCollection));
                var genericMethod = getCollectionMethod.MakeGenericMethod(propertyType);

                var collection = genericMethod.Invoke(database, new[] { collectionName, null });
                property.SetValue(context, collection);
            }
        }
    }
}

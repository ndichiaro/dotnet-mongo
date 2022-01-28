using System.Reflection;

namespace Tools.Net.Mongo.Core
{
    internal static class CollectionNameGenerator
    {
        /// <summary>
        /// Generates a collection name based on the property name. This method
        /// assumes the property name follows .NET naming conventions and generates
        /// a camel case name from the property name.
        /// </summary>
        /// <param name="property">The property the name will be generated from.</param>
        /// <returns>A camel case string based on the property name</returns>
        internal static string Generate(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute(typeof(CollectionNameAttribute));

            if (attribute == null)
                return char.ToLower(property.Name[0]) + property.Name.Substring(1);

            return (attribute as CollectionNameAttribute).Name;
        }
    }
}

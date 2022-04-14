using System;

namespace Tools.Net.Mongo.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class CollectionNameAttribute : Attribute
    {
        public string Name { get; set; }

        public CollectionNameAttribute(string name)
        {
            Name = name;
        }
    }
}

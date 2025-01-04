# Tools .NET Mongo Core

## Overview

A core library for managing MongoDB databases from a .NET application

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo.Core)](https://www.nuget.org/packages/Tools.Net.Mongo.Core)

## Getting Started

To get started create a context class that inherits from the `MongoDbContext` base class. A context class is used to manage the appication's interactions with MongoDB collections by using class Properties.

`MongoDbContext` is an abstract class that establishes a connection to new or existing databases. The context class inhereting from `MongoDbContext` needs to contain properties of type [IMongoCollection\<TDocument\>](https://mongodb.github.io/mongo-csharp-driver/2.8/apidocs/html/T_MongoDB_Driver_IMongoCollection_1.htm) to map to the corresponding MongoDB Collection. The class used for `TDocument` defines the document structure for the given MongoDB collection. The data model should use [MongoDB Serialization Attributes](https://mongodb.github.io/mongo-csharp-driver/2.8/apidocs/html/N_MongoDB_Bson_Serialization_Attributes.htm) to define the behavior and data type of each property.

By default, the MongoDB collection names will be the camel cased name of the given `IMongoCollection` property in the context class. To override the default naming, use the `CollectionName` attribute.

## Example

```
public class Pet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonIgnoreIfDefault]
    public string Type { get; set; }
    
    [BsonIgnoreIfDefault]
    public string Breed { get; set; }
    
    [BsonRequired]
    public string Name { get; set; }
    
    [BsonRepresentation(BsonType.Int32)]
    public int Age { get; set; }
}
```
```
public class VeterinaryDbContext : MongoDbContext
{
    public IMongoCollection<Pet> Pets { get; set; }

    public VeterinaryDbContext(IMongoDbContext context) : base(connectionString)
    {
    }
}
```

 A MongoDB context class represents the connection to a single MongoDB database. To establish a connection to the MongoDB database just pass the connection string into the constructor.

```
var vetContext = new VeterinaryDbContext("mongodb://localhost:27017/petDatabase");
```

Once the connection is established, any of the MongoDB operation can be called from any `IMongoCollection` property.

```
var insertResult = vetContext.Pets.InsertOne(pet);
```

For direct [IMongoDatabase](https://mongodb.github.io/mongo-csharp-driver/2.8/apidocs/html/T_MongoDB_Driver_IMongoDatabase.htm) access, use the `MongoDbContext.Db` property. 
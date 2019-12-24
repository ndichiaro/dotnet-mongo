# Tools.NET Mongo Core

## Overview

A core library for managing MongoDB databases from a .NET application

[![Build Status](https://dev.azure.com/councildevelopment/Dot%20Net%20Mongo/_apis/build/status/ndichiaro.dotnet-mongo?branchName=master)](https://dev.azure.com/councildevelopment/Dot%20Net%20Mongo/_build/latest?definitionId=5&branchName=master) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo.Core)](https://www.nuget.org/packages/Tools.Net.Mongo.Core)

## Getting Started

To get started create a *Collection* class that inherits the `MongoDbCollection` class. A Collection class is used to manage the appication's interactions with a single MongoDB collection in a database.

`MongoDbCollection` is a generic, abstract class that contains common MongoDB CRUD operations. The model used for `TEntityType` defines the document structure for the given MongoDB collection. The data model should use [MongoDB Serialization Attributes](https://api.mongodb.com/csharp/2.2/html/N_MongoDB_Bson_Serialization_Attributes.htm) to define the MongoDB data type and behaviors of each model property.  `MongoDbCollection` can be used with new or existing MongoDB databases.

By default, the MongoDb collection connect to a `MongoDbCollection` class will be the camel cased name of the class passed in as the `TEntityType`. For the example below, the MongoDB collection create will be pet. To override the default naming, override the `CollectionName` property.

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
public class PetCollection : MongoDbCollection<Pet>
{
    public PetCollection(IMongoDbContext context) : base(context)
    {
    }
}
```

A `MongoDbCollection` requires an instance of `IMongoDbContext` to be passed into the constructor. A MongoDB context class represents the connection to a single MongoDB database. To create an instance of `IMongoDbContext` use the `MongoDbContextBuilder` class.

```
var petDatabase = MongoDbContextBuilder.Build("mongodb://localhost:27017/petDatabase");
var petCollection = new PetCollection(petDatabase);
```

Once the collection is created, any of the CRUD operation can be called from the collection instance.

```
var insertResult = petCollection.Insert(pet);
```

To create custom queries, use the `Collection` property to access the underlying MongoDB driver [MongoCollection](https://api.mongodb.com/csharp/2.2/html/T_MongoDB_Driver_MongoCollection_1.htm) class. 
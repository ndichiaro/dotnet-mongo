# Tools.NET Mongo

## Overview

A Global Tool for the dotnet CLI to manage MongoDB databases in .NET.

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo)](https://www.nuget.org/packages/Tools.Net.Mongo)

## Installation

This package contains a .NET Core Global Tool you can call from the shell/command line. To install use the following command:

```
dotnet tool install --global Tools.Net.Mongo
```

## CLI Usage

```
Tools:
  
  migrate [tool-options] <command>        Manages MongoDB migrations

Tool Options:

  -h|--help                               Prints usage information
  -v|--version                            Prints version information

Commands:

  create <name>                           Creates a new migration file. NAME is required to create a migration
  up [command-options]                    Runs all migrations that have not been applied   
  down [command-options]                  Downgrades the database by undoing the last applied migration
  status [command-options]                Prints the changelog of the database
  
Command Options:
  
  -i|--uri                                The MongoDB connection string
  
```

## Getting Started

Start by creating a .NET Core or .NET Standard project. Once the project in created, change your shell/command line directory to the directory of the project file.

Next, install the [Tools.Net.Mongo.Core](https://github.com/ndichiaro/dotnet-mongo/blob/master/src/Tools.Net.Mongo.Core) package in your project. *Note: The dotnet Mongo tool uses the Mongo Core package for its migrations. If you're receiving build errors after creating a migration, verify the MOngo Core package is installed properly.

### Migrations

.NET Mongo Migrations provides functionality to manage the state of a MongoDB instance using the `dotnet mongo migrate` command.

#### Creating a New Migration
To create a new migration use the `dotnet mongo migrate create <NAME>` command. A migration file will be created in the Migrations directory of the project.

```
PS C:\Repositories\demo\Tools.Mongo.Demo> dotnet mongo migrate create MigrationDemo
Created: Migrations/M201908311227533_MigrationDemo.cs
```

After the migration is created, implement the *Up* and *Down* functions in the generated migration file. 

```
public bool Up(IMongoDatabase database)
{
    var usersCollection = database.GetCollection<BsonDocument>("users");
    usersCollection.InsertOne(new BsonDocument
    {
        {"firstName", "Rick"},
        {"lastName", "Grimes"},
        {"email", "rgrimes@twd.com"},
    });

    var filterDefinition = Builders<BsonDocument>.Filter.Eq("firstName", "Rick");
    filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("lastName", "Grimes");
    filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("email", "rgrimes@twd.com");

    var newUser = usersCollection.Find(filterDefinition).ToList();

    if (newUser.Count == 1) return true;

    return false;
}
```

```
public bool Down(IMongoDatabase database)
{
    var usersCollection = database.GetCollection<BsonDocument>("users");

    var filterDefinition = Builders<BsonDocument>.Filter.Eq("firstName", "Rick");
    filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("lastName", "Grimes");
    filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("email", "rgrimes@twd.com");

    var result = usersCollection.DeleteOne(filterDefinition);

    if (result.DeletedCount == 1) return true;
    return false;
}
```

*Note: Each function returns a bool value to indicate if the script was run successfully*

#### Upgrading a Database

To upgrate a database, run the `dotnet mongo migrate up --url <connectionString>` command. The `up` command will run all migrations that have not been applied to a given database instance. *Note: This command must be executed in the project directory where the Migrations folder lives.*

```
PS C:\Repositories\demo\Tools.Mongo.Demo> dotnet mongo migrate up --uri mongodb://localhost:27017/twdDb
Migrated: M201908311227533_MigrationDemo
```

#### Downgrading a Database

To downgrade a database, run the `dotnet mongo migrate down --url <connectionString>` command. The `down` command will undo the latest migration from a given database. *Note: This command must be executed in the project directory where the Migrations folder lives.*

```
PS C:\Repositories\demo\Tools.Mongo.Demo> dotnet mongo migrate down --uri mongodb://localhost:27017/twdDb
Downgraded: M201908311227533_MigrationDemo
```

#### Checking the Migration Status

To see the status of a database instance, run the `dotnet mongo migrate status --url <connectionString>` command. *Note: This command must be executed in the project directory where the Migrations folder lives.*

```
PS C:\Repositories\demo\Tools.Mongo.Demo> dotnet mongo migrate status --uri mongodb://localhost:27017/twdDb
+--------------------------------+------------+
| Migration                      | Applied At |
+--------------------------------+------------+
| M201908311227533_MigrationDemo | PENDING    |
+--------------------------------+------------+
```

## Development & Testing

For information about running tests and contributing to this project, see:

- **[End-to-End Testing Guide](docs/E2E-TESTING.md)** - Comprehensive guide for running E2E tests in Docker containers
- **[Tests Directory](tests/)** - Unit tests and integration tests for all components

The project includes a full Docker-based E2E testing suite that validates the migration tool in containerized environments with real MongoDB instances.

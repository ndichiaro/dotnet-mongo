# .NET Mongo

## Overview

A Global Tool for the dotnet CLI to manage MongoDB databases in .NET.

[![Build Status](https://dev.azure.com/councildevelopment/Dot%20Net%20Mongo/_apis/build/status/ndichiaro.dotnet-mongo?branchName=master)](https://dev.azure.com/councildevelopment/Dot%20Net%20Mongo/_build/latest?definitionId=5&branchName=master) 

|                  | Latest |
| :--:             |  :--:  |
|  .NET Mongo  |[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo)](https://www.nuget.org/packages/Tools.Net.Mongo)|
|  .NET Mongo Core |[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo.Core)](https://www.nuget.org/packages/Tools.Net.Mongo.Core)|

## Installation

This package contains a .NET Core Global Tool you can call from the shell/command line. To install use the following command:

```
dotnet tool install --global Tools.Net.Mongo
```

To specify the specific version, use the `--version` tag.

```
dotnet tool install --global Tools.Net.Mongo --version 1.0.0
```


## CLI Usage

```
Commands:
  
  --migrate <ARGUMENT>      Manages MongoDB migrations
  
Arguments:

  create <NAME>           Creates a new migration file. NAME is required to create a migration.
  up [-i|--uri]           Runs all migrations that have not been applied   
  down [-i|--uri]         Downgrades the database by undoing the last applied migration
  status [-i|--uri]       Prints the changelog of the database
  
Options:
  
  -i|--uri                The MongoDB connection string
  
```

## Getting Started

Start by creating a .NET Core or .NET Standard project. Once the project in created, change your shell/command line directory to the directory of the project file.

![Image of Project Directory](https://github.com/ndichiaro/images/blob/master/DotNetMongo-ProjectShell.png?raw=true)

### Migrations

.NET Mongo Migrations provides functionality to manage the state of a MongoDB instance using the `dotnet mongo migrate` command.

To create a new migration use the `dotnet mongo migrate create <NAME>` command. A migration file will be created in the Migrations directory of the project.

![Image of Create Command](https://github.com/ndichiaro/images/blob/master/DotNetMongo-Create.png?raw=true)

# .NET Mongo

## Overview

A Global Tool for the dotnet CLI to manage MongoDB databases in .NET.

[![Build Status](https://dev.azure.com/councildevelopment/Dot%20Net%20Mongo/_apis/build/status/ndichiaro.dotnet-mongo?branchName=master)](https://dev.azure.com/councildevelopment/Dot%20Net%20Mongo/_build/latest?definitionId=5&branchName=master) 

|                  | Latest |
| :--:             |  :--:  |
|  .NET Mongo  |![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo)|
|  .NET Mongo Core |![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Tools.Net.Mongo.Core)|

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

  create <NAME>           Creates a new migration file
  up [-i|--uri]           Runs all migrations that have not been applied   
  down [-i|--uri]         Downgrades the database by undoing the last applied migration
  status [-i|--uri]       Prints the changelog of the database
  
Options:
  
  -i|--uri                The MongoDB connection string
  
```

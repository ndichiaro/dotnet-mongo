using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace Tools.Net.Mongo.E2E.Test;

/// <summary>
/// End-to-end tests for the MongoDB migration tool running in Docker containers
/// </summary>
public class MigrationToolE2ETests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    private readonly string _connectionStringTemplate = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
            ?? "mongodb://root:password123@localhost:27017/{0}?authSource=admin";
    private readonly string _testDataPath = Path.Combine(AppContext.BaseDirectory, "TestData", "SampleProject");

    private string GetUniqueConnectionString()
    {
        var uniqueDbName = $"testdb_{Guid.NewGuid():N}";
        return string.Format(_connectionStringTemplate, uniqueDbName);
    }

    private async Task<(IMongoDatabase database, string connectionString)> SetupTestDatabaseAsync()
    {
        var connectionString = GetUniqueConnectionString();
        var mongoClient = new MongoClient(connectionString);
        var uri = new MongoUrl(connectionString);
        var database = mongoClient.GetDatabase(uri.DatabaseName);
        
        // Wait for MongoDB to be ready
        var retries = 30;
        while (retries > 0)
        {
            try
            {
                await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                _output.WriteLine("MongoDB is ready");
                break;
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Waiting for MongoDB... {ex.Message}");
                await Task.Delay(1000);
                retries--;
            }
        }

        if (retries == 0)
        {
            throw new TimeoutException("MongoDB did not become ready in time");
        }

        return (database, connectionString);
    }

    private async Task CleanupTestDatabaseAsync(IMongoDatabase database)
    {
        try
        {
            // Drop the entire database
            await database.Client.DropDatabaseAsync(database.DatabaseNamespace.DatabaseName);
            _output.WriteLine("Database cleaned up");
        }
        catch (Exception ex)
        {
            _output.WriteLine($"Error during cleanup: {ex.Message}");
        }
    }

    [Fact]
    public async Task CanRunMigrationUp_WithSuccessfulResult()
    {
        // Arrange
        var (database, connectionString) = await SetupTestDatabaseAsync();
        var workingDirectory = _testDataPath;

        try
        {
            // Act
            var result = await RunMigrationToolAsync(["migrate", "up", "--uri", connectionString], workingDirectory);

            // Assert
            result.ExitCode.Should().Be(0, $"Migration should succeed. Output: {result.Output}, Error: {result.Error}");
            result.Output.Should().NotBeEmpty("Migration should produce output");

            // Give a moment for the database operations to complete
            await Task.Delay(1000);
            
            // Debug: List all databases
            var allDatabases = await database.Client.ListDatabaseNamesAsync();
            var dbList = await allDatabases.ToListAsync();
            _output.WriteLine($"Available databases: {string.Join(", ", dbList)}");
            _output.WriteLine($"Using database: {database.DatabaseNamespace.DatabaseName}");

            // Verify database changes
            var collections = await database.ListCollectionNamesAsync();
            var collectionList = await collections.ToListAsync();
            _output.WriteLine($"Collections found: {string.Join(", ", collectionList)}");
            collectionList.Should().Contain("users", "Users collection should be created");

            // Verify migration changelog
            var changelogCollection = database.GetCollection<BsonDocument>("changelog");
            var changelogCount = await changelogCollection.CountDocumentsAsync(new BsonDocument());
            _output.WriteLine($"Changelog count: {changelogCount}");
            changelogCount.Should().Be(2, "Two migrations should be recorded");

            // Verify user data
            var usersCollection = database.GetCollection<BsonDocument>("users");
            var userCount = await usersCollection.CountDocumentsAsync(new BsonDocument());
            _output.WriteLine($"User count: {userCount}");
            userCount.Should().Be(3, "Three users should be inserted");
        }
        finally
        {
            await CleanupTestDatabaseAsync(database);
        }
    }

            [Fact]
    public async Task CanRunMigrationStatus_ShowsCorrectInformation()
    {
        // Arrange
        var (database, connectionString) = await SetupTestDatabaseAsync();

        try
        {
            // Run migrations first
            await RunMigrationToolAsync(["migrate", "up", "--uri", connectionString], _testDataPath);

            // Act
            var result = await RunMigrationToolAsync(["migrate", "status", "--uri", connectionString], _testDataPath);

            // Assert
            result.ExitCode.Should().Be(0, $"Status command should succeed. Output: {result.Output}, Error: {result.Error}");
            result.Output.Should().Contain("M202501160001_CreateUserCollection", "Should show first migration");
            result.Output.Should().Contain("M202501160002_AddSampleUsers", "Should show second migration");
            result.Output.Should().Contain("Applied At", "Should show when migrations were applied");
        }
        finally
        {
            await CleanupTestDatabaseAsync(database);
        }
    }

            [Fact]
    public async Task CanRunMigrationDown_RollsBackSuccessfully()
    {
        // Arrange
        var (database, connectionString) = await SetupTestDatabaseAsync();

        try
        {
            // First, run migrations up
            await RunMigrationToolAsync(["migrate", "up", "--uri", connectionString], _testDataPath);

            // Verify initial state
            var usersCollection = database.GetCollection<BsonDocument>("users");
            var initialUserCount = await usersCollection.CountDocumentsAsync(new BsonDocument());
            initialUserCount.Should().Be(3, "Should have 3 users after up migration");

            // Act - Run migration down
            var result = await RunMigrationToolAsync(["migrate", "down", "--uri", connectionString], _testDataPath);

            // Assert
            result.ExitCode.Should().Be(0, $"Down migration should succeed. Output: {result.Output}, Error: {result.Error}");
            result.Output.Should().Contain("Downgraded", "Should show downgrade message");

            // Verify rollback
            var changelogCollection = database.GetCollection<BsonDocument>("changelog");
            var changelogCount = await changelogCollection.CountDocumentsAsync(new BsonDocument());
            changelogCount.Should().Be(1, "One migration should remain in history after rollback");

            var userCountAfterDown = await usersCollection.CountDocumentsAsync(new BsonDocument());
            userCountAfterDown.Should().Be(0, "Users should be removed after rollback");
        }
        finally
        {
            await CleanupTestDatabaseAsync(database);
        }
    }

            [Fact]
    public async Task MigrationWithInvalidConnectionString_ReturnsError()
    {
        // Act
        var result = await RunMigrationToolAsync(["migrate", "up", "--uri", "mongodb://invalid:27017/testdb"], _testDataPath);

        // Assert
        result.ExitCode.Should().NotBe(0, "Migration with invalid connection should fail");
        result.Error.Should().Contain("TimeoutException", "Should indicate connection timeout");
    }

    [Fact]
    public async Task MigrationWithoutProjectFile_ReturnsError()
    {
        // Arrange
        var emptyDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(emptyDirectory);
        var connectionString = GetUniqueConnectionString();

        try
        {
            // Act
            var result = await RunMigrationToolAsync(["migrate", "up", "--uri", connectionString], emptyDirectory);

            // Assert
            result.ExitCode.Should().NotBe(0, "Migration without project file should fail");
            result.Error.Should().Contain("Could not execute because the specified command or file was not found", "Should indicate missing tool");
        }
        finally
        {
            Directory.Delete(emptyDirectory, true);
        }
    }

    [Fact]
    public async Task CanRunHelpCommand_ReturnsUsageInformation()
    {
        // Act
        var result = await RunMigrationToolAsync(["--help"], _testDataPath);

        // Assert
        result.ExitCode.Should().Be(0, "Help command should succeed");
        result.Output.Should().Contain("usage", "Help should contain usage information");
    }

    [Fact]
    public async Task CanRunVersionCommand_ReturnsVersionInformation()
    {
        // Act
        var result = await RunMigrationToolAsync(["--version"], _testDataPath);

        // Assert
        result.ExitCode.Should().Be(0, "Version command should succeed");
        result.Output.Should().NotBeEmpty("Version should return information");
    }

    private async Task<ProcessResult> RunMigrationToolAsync(string[] args, string workingDirectory)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"Tools.Net.Mongo.dll {string.Join(" ", args)}",
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var output = string.Empty;
        var error = string.Empty;

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                output += e.Data + Environment.NewLine;
                _output.WriteLine($"STDOUT: {e.Data}");
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                error += e.Data + Environment.NewLine;
                _output.WriteLine($"STDERR: {e.Data}");
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            Output = output.Trim(),
            Error = error.Trim()
        };
    }

    private class ProcessResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}

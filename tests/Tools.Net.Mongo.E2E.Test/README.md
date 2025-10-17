# End-to-End Tests for MongoDB Migration Tool

This directory contains comprehensive end-to-end tests for the MongoDB migration tool that run entirely in Docker containers.

## Overview

The E2E test suite validates the migration tool functionality in a containerized environment that closely mimics production deployments. It includes:

- **MongoDB Container**: A MongoDB 7.0 instance with authentication
- **Migration Tool Container**: The compiled migration tool ready for execution
- **Test Container**: XUnit tests that exercise the migration tool against the MongoDB instance

## Test Structure

### Core Test Classes

1. **`MigrationToolE2ETests`**: Primary test suite covering:
   - Migration up/down operations
   - Status reporting
   - Error handling scenarios
   - Help and version commands

2. **`DockerIntegrationTests`**: Container-specific tests covering:
   - MongoDB connectivity
   - CRUD operations
   - Data persistence
   - Container networking

### Test Data

- **`TestData/SampleProject/`**: A sample .NET project with migrations
  - `M202501160001_CreateUserCollection.cs`: Creates users collection with validation
  - `M202501160002_AddSampleUsers.cs`: Inserts sample user data

## Running the Tests

### Prerequisites

- Docker and Docker Compose installed
- Linux/macOS environment (or WSL on Windows)

### Quick Start

```bash
# From the project root directory
./scripts/run-e2e-tests.sh
```

### Manual Execution

```bash
# Build and start the environment
docker-compose -f docker/docker-compose.e2e.yml build
docker-compose -f docker/docker-compose.e2e.yml up -d mongodb migration-tool

# Wait for MongoDB to be ready
docker-compose -f docker/docker-compose.e2e.yml exec mongodb mongosh --eval "db.adminCommand('ping')"

# Run the tests
docker-compose -f docker/docker-compose.e2e.yml run --rm e2e-tests

# Cleanup
docker-compose -f docker/docker-compose.e2e.yml down -v
```

### Individual Test Execution

```bash
# Run specific test class
docker-compose -f docker/docker-compose.e2e.yml run --rm e2e-tests dotnet test --filter "ClassName=MigrationToolE2ETests"

# Run specific test method
docker-compose -f docker/docker-compose.e2e.yml run --rm e2e-tests dotnet test --filter "MethodName=CanRunMigrationUp_WithSuccessfulResult"
```

## Test Scenarios Covered

### Migration Operations
- ✅ Successful migration up
- ✅ Successful migration down (rollback)
- ✅ Migration status reporting
- ✅ Multiple migration files
- ✅ Migration history tracking

### Error Conditions
- ✅ Invalid connection string
- ✅ Missing project file
- ✅ Network connectivity issues
- ✅ Database authentication failures

### Tool Commands
- ✅ Help command (`--help`)
- ✅ Version command (`--version`)
- ✅ Invalid arguments handling

### Container Integration
- ✅ MongoDB container connectivity
- ✅ Data persistence across operations
- ✅ Container networking
- ✅ Environment variable configuration

## Configuration

### Environment Variables

- `MONGO_CONNECTION_STRING`: MongoDB connection string (default: `mongodb://root:password123@mongodb:27017/testdb?authSource=admin`)

### Docker Compose Services

- **`mongodb`**: MongoDB 7.0 with authentication
- **`migration-tool`**: Migration tool container
- **`e2e-tests`**: Test execution container

## CI/CD Integration

The E2E tests are designed to be easily integrated into CI/CD pipelines:

```yaml
# Example GitHub Actions step
- name: Run E2E Tests
  run: |
    chmod +x ./scripts/run-e2e-tests.sh
    ./scripts/run-e2e-tests.sh
```

## Troubleshooting

### Common Issues

1. **MongoDB not ready**: Increase timeout in the test runner script
2. **Port conflicts**: Ensure port 27017 is available
3. **Docker permissions**: Ensure Docker daemon is accessible
4. **Test failures**: Check container logs with `docker-compose logs`

### Debugging

```bash
# View MongoDB logs
docker-compose -f docker/docker-compose.e2e.yml logs mongodb

# View migration tool logs
docker-compose -f docker/docker-compose.e2e.yml logs migration-tool

# View test logs
docker-compose -f docker/docker-compose.e2e.yml logs e2e-tests

# Connect to MongoDB shell
docker-compose -f docker/docker-compose.e2e.yml exec mongodb mongosh -u root -p password123 --authenticationDatabase admin
```

## Benefits

1. **Isolation**: Tests run in isolated containers, preventing interference
2. **Consistency**: Same environment across development, CI, and production
3. **Reliability**: No dependency on local MongoDB installation
4. **Portability**: Runs identically on any Docker-enabled system
5. **Comprehensive**: Tests the complete migration workflow end-to-end

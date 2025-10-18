# MongoDB Migration Tool - End-to-End Testing Suite

This document describes the comprehensive end-to-end testing setup for the MongoDB migration tool using Docker containers.

## 🎯 Overview

The E2E testing suite validates the complete migration tool workflow in a containerized environment that closely mimics production deployments. This approach ensures reliable testing across different environments and eliminates dependencies on local MongoDB installations.

## 🏗️ Architecture

```
┌─────────────────┐    ┌─────────────────┐
│   E2E Tests     │───▶│    MongoDB      │
│   Container     │    │   Container     │
│                 │    │                 │
│ - XUnit Tests   │    │ - MongoDB 7.0   │
│ - Test Runner   │    │ - Authentication│
│ - MongoDB Client│    │ - Persistence   │
│ - Migration Tool│    └─────────────────┘
└─────────────────┘
```

### Components

1. **MongoDB Container**: MongoDB 7.0 with authentication enabled
2. **E2E Test Container**: XUnit tests with embedded migration tool that orchestrates and validates migrations
3. **Test Data**: Sample projects with migrations for testing scenarios

The migration tool runs directly within the test container, providing more realistic testing scenarios and simplified architecture.

## 🚀 Quick Start

### Prerequisites

- Docker and Docker Compose installed
- Linux/macOS environment (or WSL on Windows)
- Bash shell (for the test runner script)

### Running All Tests

```bash
# From the project root directory
./scripts/run-e2e-tests.sh
```

### Manual Execution

```bash
# Build the test environment
docker compose -f docker/docker-compose.e2e.yml build

# Start MongoDB container
docker compose -f docker/docker-compose.e2e.yml up -d mongodb

# Wait for MongoDB to be ready
timeout 60 bash -c 'until docker compose -f docker/docker-compose.e2e.yml exec mongodb mongosh --eval "db.adminCommand(\"ping\")" > /dev/null 2>&1; do sleep 1; done'

# Run the tests
docker compose -f docker/docker-compose.e2e.yml run --rm e2e-tests

# Clean up
docker compose -f docker/docker-compose.e2e.yml down -v
```

## 🧪 Test Coverage

### Core Migration Scenarios

- ✅ **Migration Up**: Apply all pending migrations
- ✅ **Migration Down**: Rollback the last migration
- ✅ **Migration Status**: Display migration history and status
- ✅ **Multiple Migrations**: Handle sequential migration files
- ✅ **Migration History**: Track applied migrations in `__migrationhistory`

### Error Handling

- ✅ **Invalid Connection String**: Graceful failure with error message
- ✅ **Missing Project File**: Detection and reporting of missing .csproj
- ✅ **Network Issues**: Timeout and connection error handling
- ✅ **Authentication Failures**: MongoDB auth error handling

### Tool Commands

- ✅ **Help Command**: `--help` displays usage information
- ✅ **Version Command**: `--version` shows tool version
- ✅ **Create Command**: `create <name>` generates new migration files
- ✅ **Invalid Arguments**: Proper error messages for invalid input

### Container Integration

- ✅ **MongoDB Connectivity**: Container-to-container networking
- ✅ **Data Persistence**: Verify data survives across operations
- ✅ **Environment Variables**: Configuration via env vars
- ✅ **Migration Tool Execution**: Direct execution within test container

## 📁 Project Structure

```
tests/Tools.Net.Mongo.E2E.Test/
├── Tools.Net.Mongo.E2E.Test.csproj     # Test project file
├── Tools.Net.Mongo.E2E.sln             # Isolated E2E solution
├── MigrationToolE2ETests.cs             # Core E2E test suite
├── README.md                            # Test documentation
└── TestData/
    └── SampleProject/                   # Sample project for testing
        ├── SampleProject.csproj         # Sample project file
        └── Migrations/                  # Test migration files

docker/
├── Dockerfile.e2e-tests                # E2E test container
└── docker-compose.e2e.yml              # Docker orchestration

scripts/
└── run-e2e-tests.sh                    # Test runner script
```
└── TestData/
    └── SampleProject/
        ├── SampleProject.csproj          # Test project
        └── Migrations/
            ├── M202501160001_CreateUserCollection.cs
            └── M202501160002_AddSampleUsers.cs

docker/
├── Dockerfile.migration-tool            # Migration tool container
├── Dockerfile.e2e-tests                # Test runner container
└── docker-compose.e2e.yml              # Orchestration config

scripts/
└── run-e2e-tests.sh                    # Test runner script

.github/workflows/
├── e2e-tests.yml                        # E2E test workflow
└── ci.yml                               # Complete CI/CD pipeline
```

## 🔧 Configuration

### Environment Variables

| Variable | Default Value | Description |
|----------|---------------|-------------|
| `MONGO_CONNECTION_STRING` | `mongodb://root:password123@mongodb:27017/testdb?authSource=admin` | MongoDB connection string |

### Docker Compose Services

#### MongoDB Service
- **Image**: `mongo:7.0`
- **Authentication**: Enabled with root user
- **Port**: `27017` (internal)
- **Health Check**: Uses `mongosh` ping command
- **Persistence**: Named volume `mongodb_data`

#### E2E Tests Service
- **Build**: Uses `Dockerfile.e2e-tests`
- **Dependencies**: Requires MongoDB health check
- **Migration Tool**: Embedded within test container
- **Entry Point**: Executes XUnit tests with detailed logging
- **Solution**: Uses isolated `Tools.Net.Mongo.E2E.sln`

## 🔍 Debugging

### View Container Logs

```bash
# MongoDB logs
docker compose -f docker/docker-compose.e2e.yml logs mongodb

# Test execution logs
docker compose -f docker/docker-compose.e2e.yml logs e2e-tests
```

### Connect to MongoDB Shell

```bash
docker compose -f docker/docker-compose.e2e.yml exec mongodb mongosh -u root -p password123 --authenticationDatabase admin
```

### Run Individual Tests

```bash
# Run specific test class
docker compose -f docker/docker-compose.e2e.yml run --rm e2e-tests dotnet test --filter "ClassName=MigrationToolE2ETests"

# Run specific test method
docker compose -f docker/docker-compose.e2e.yml run --rm e2e-tests dotnet test --filter "MethodName=CanRunMigrationUp_WithSuccessfulResult"
```

### Interactive Container Shell

```bash
# Access test container shell
docker compose -f docker/docker-compose.e2e.yml run --rm --entrypoint /bin/bash e2e-tests
```

## 🔄 CI/CD Integration

### GitHub Actions

The E2E tests are integrated into the CI/CD pipeline with two workflows:

1. **E2E Tests Workflow** (`e2e-tests.yml`):
   - Runs on push/PR to main/develop branches
   - Executes E2E tests in isolation
   - Uploads test results as artifacts

2. **Complete CI/CD Pipeline** (`ci.yml`):
   - Runs unit tests first
   - Executes E2E tests after unit tests pass
   - Packages the tool on successful test completion
   - Provides comprehensive test reporting

### Local CI Simulation

```bash
# Run the same commands as CI
./scripts/run-e2e-tests.sh

# Check exit code
echo $?  # Should be 0 for success
```

## 📊 Benefits

### 🏃‍♂️ Development Benefits

1. **Fast Feedback**: Catch integration issues early in development
2. **Reliable Testing**: Consistent environment across all machines
3. **No Dependencies**: No need for local MongoDB installation
4. **Parallel Development**: Multiple developers can run tests simultaneously

### 🚀 CI/CD Benefits

1. **Automated Validation**: Every PR is tested against real database
2. **Environment Parity**: Test environment matches production containers
3. **Regression Detection**: Catch breaking changes immediately
4. **Release Confidence**: Comprehensive validation before deployment

### 🔒 Production Benefits

1. **Migration Validation**: Test migrations before production deployment
2. **Rollback Testing**: Verify rollback procedures work correctly
3. **Performance Baseline**: Establish migration performance expectations
4. **Documentation**: Live examples of migration tool usage

## 🛠️ Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| MongoDB not starting | Check port 27017 availability, increase timeout |
| Test failures | Verify MongoDB connection string and credentials |
| Container build errors | Ensure Docker daemon is running, check Dockerfile syntax |
| Permission denied | Ensure test runner script is executable (`chmod +x`) |
| Network connectivity | Verify Docker network configuration |

### Performance Tuning

```bash
# Increase MongoDB startup timeout
# Edit docker-compose.e2e.yml:
healthcheck:
  interval: 10s
  timeout: 10s
  retries: 10  # Increase retries

# Optimize Docker build caching
# Use .dockerignore to exclude unnecessary files
echo "node_modules" >> .dockerignore
echo "bin/" >> .dockerignore
echo "obj/" >> .dockerignore
```

## 🎯 Future Enhancements

### Planned Improvements

1. **Performance Tests**: Add migration performance benchmarks
2. **Stress Testing**: Test with large datasets and many migrations
3. **Multi-Database**: Test against different MongoDB versions
4. **Cloud Testing**: Integrate with MongoDB Atlas for cloud testing
5. **Visual Reports**: Generate HTML test reports with charts
6. **Notification**: Slack/email notifications for test failures

### Contributing

To add new E2E test scenarios:

1. Create test methods in `MigrationToolE2ETests.cs`
2. Add sample migrations to `TestData/SampleProject/Migrations/`
3. Update test documentation
4. Ensure tests cleanup after execution
5. Add appropriate assertions for validation

---

*This E2E testing suite ensures the MongoDB migration tool works reliably in containerized environments, providing confidence for production deployments and enabling rapid development cycles.*

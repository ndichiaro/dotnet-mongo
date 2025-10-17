#!/bin/bash

# E2E Test Runner Script
# This script runs the migration tool end-to-end tests in Docker containers

set -e

echo "🚀 Starting Migration Tool E2E Tests"

# Change to the project root directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$PROJECT_ROOT"

echo "📁 Project root: $PROJECT_ROOT"

# Function to cleanup containers
cleanup() {
    echo "🧹 Cleaning up containers..."
    docker compose -f docker/docker-compose.e2e.yml down -v --remove-orphans
}

# Set trap to cleanup on exit
trap cleanup EXIT

# Build and start the test environment
echo "🔨 Building test environment..."
docker compose -f docker/docker-compose.e2e.yml build

echo "🐳 Starting MongoDB and migration tool containers..."
docker compose -f docker/docker-compose.e2e.yml up -d mongodb migration-tool

# Wait for MongoDB to be ready
echo "⏳ Waiting for MongoDB to be ready..."
retries=60
while [ $retries -gt 0 ]; do
    if docker compose -f docker/docker-compose.e2e.yml exec mongodb mongosh -u root -p password123 --authenticationDatabase admin --eval "db.adminCommand('ping')" > /dev/null 2>&1; then
        echo "✅ MongoDB is ready"
        break
    fi
    echo "Waiting for MongoDB... ($retries retries left)"
    sleep 1
    retries=$((retries - 1))
done

if [ $retries -eq 0 ]; then
    echo "❌ MongoDB failed to start within timeout"
    exit 1
fi

# Run the E2E tests
echo "🧪 Running E2E tests..."
docker compose -f docker/docker-compose.e2e.yml run --rm e2e-tests

# Check the exit code
if [ $? -eq 0 ]; then
    echo "✅ All E2E tests passed!"
else
    echo "❌ E2E tests failed!"
    exit 1
fi

echo "🎉 E2E test run completed successfully!"

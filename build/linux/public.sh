#!/bin/bash

# This is a build script for the project (Linux version)
# Execute this to build the project

# Configuration
PROJECT_NAME="bookShop"
SOLUTION_FILE="src.sln"
DIR_SOLUTION_FILE="src"
CONFIGURATION_MODE="Release"
CURRENT_PATH=$(pwd)

# Function to find the project root directory
find_project_root() {
    local current_dir="$1"
    while [ "$current_dir" != "/" ]; do
        parent_dir=$(dirname "$current_dir")
        last_folder=$(basename "$parent_dir")
        if [ "$last_folder" == "$PROJECT_NAME" ]; then
            echo "$parent_dir"
            return
        fi
        current_dir="$parent_dir"
    done
    echo ""
}

# Determine the project root path
project_root=$(find_project_root "$(dirname "$(realpath "$0")")")
if [ -z "$project_root" ]; then
    echo "Error: No solution file (.sln) found in the directory hierarchy." >&2
    exit 1
fi

echo "Project root path determined: $project_root"
cd "$project_root/$DIR_SOLUTION_FILE" || exit

# Publish the project
echo "Publishing project..."
dotnet publish -c Release --no-build
if [ $? -ne 0 ]; then
    echo "Error: dotnet publish failed" >&2
    exit 1
fi

# Run unit tests
echo "Running unit tests..."
dotnet test --no-build --verbosity normal
if [ $? -ne 0 ]; then
    echo "Error: dotnet test failed" >&2
    exit 1
fi

# Generate deployment artifacts (optional step)
echo "Generating deployment artifacts..."
DEPLOYMENT_DIR="$project_root/deployment"
mkdir -p "$DEPLOYMENT_DIR"
cp -r "$project_root/$DIR_SOLUTION_FILE/bin/Release/net*" "$DEPLOYMENT_DIR"
if [ $? -ne 0 ]; then
    echo "Error: Failed to copy deployment artifacts" >&2
    exit 1
fi

echo "Build and publish process completed successfully!"
cd "$CURRENT_PATH" || exit

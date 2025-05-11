#!/bin/bash
# This is a build script for the project (Linux version)
# Execute this to build the project

# Change this to your project name
PROJECT_NAME="bookShop"
SOLUTION_FILE="src.sln"
DIR_SOLUTION_FILE="src"

# Set error handling
set -e  # Stop the script when an error occurs

# Constants
CONFIGURATION_MODE="Release"  # Build mode (Release or Debug)
CURRENT_PATH=$(pwd)  # Save the current directory to return later

# Function to find the root directory containing the solution file
find_project_root() {
    local current_dir=$1

    while true; do
        # Find the parent directory of the current directory
        local parent_dir=$(dirname "$current_dir")

        # Get the last folder name from the parent directory path
        local last_folder=$(basename "$parent_dir")

        # If the last folder name matches the project name, return the path
        if [ "$last_folder" == "$PROJECT_NAME" ]; then
            echo "$parent_dir"
            return
        fi

        # If not, move up one directory in the folder tree
        current_dir=$parent_dir

        # Break if we've reached the root directory
        if [ "$current_dir" == "/" ]; then
            echo ""
            return
        fi
    done
}

# Determine the project root path containing the solution file
PROJECT_ROOT=$(find_project_root "$(dirname "$(realpath "$0")")")
if [ -z "$PROJECT_ROOT" ]; then
    echo "Error: No solution file (.sln) found in the directory hierarchy." >&2
    exit 1
fi

# Set to working directory to project root
echo "Project root path determined: $PROJECT_ROOT"
cd "$PROJECT_ROOT/$DIR_SOLUTION_FILE"

# Publish the project
echo "Publishing project..."
dotnet publish -c Release --no-build
if [ $? -ne 0 ]; then
    echo "Error: dotnet publish failed" >&2
    exit 1
fi

# Set back to original directory
cd "$CURRENT_PATH"

#!/bin/bash

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
    local currentDir="$1"
    
    while true; do
        # Find the parent directory of the current directory
        parentDir=$(dirname "$currentDir")
        
        # Get the last folder name from the parent directory path
        lastFolder=$(basename "$parentDir")

        # If the last folder name matches the project name, return the path
        if [[ "$lastFolder" == "$PROJECT_NAME" ]]; then
            echo "$parentDir"
            return
        fi

        # If not, move up one directory in the folder tree
        currentDir="$parentDir"
    done

    return 1  # Return 1 if the project root folder is not found
}

# Determine the project root path containing the solution file
# Call the find_project_root function to find the root directory containing the .sln file
projectRoot=$(find_project_root "$(dirname "$(readlink -f "$0")")")
if [[ -z "$projectRoot" ]]; then
    echo "No solution file (.sln) found in the directory hierarchy."
    exit 1
fi

# Set working directory to project root
echo "Project root path determined: $projectRoot"
cd "$projectRoot/$DIR_SOLUTION_FILE"  # Change to the directory containing the solution file

# Restore project dependencies
echo "Restore project..."
dotnet restore ./$SOLUTION_FILE  # Run the dotnet restore command to restore dependencies
if [[ $? -ne 0 ]]; then
    echo "dotnet restore failed"
    exit 1
fi

# Build the project
echo "Build project..."
dotnet build --no-restore -c "$CONFIGURATION_MODE" "$SOLUTION_FILE"  # Run the dotnet build command with the Release configuration
if [[ $? -ne 0 ]]; then
    echo "dotnet build failed"
    exit 1
fi

# Set back to original directory
cd "$CURRENT_PATH"  # Change back to the original directory

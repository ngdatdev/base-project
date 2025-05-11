# This is build script for the project (Windows version)
# Execute this to build the project

# Change this to your project name
$PROJECT_NAME = 'bookShop'
$SOLUTION_FILE = "src.sln" 
$DIR_SOLUTION_FILE = "src"

# Set error handling
$ErrorActionPreference = "Stop"  
# Stop the script when an error occurs

# Constants
$CONFIGURATION_MODE = 'Release'  
# Build mode (Release or Debug)
$CURRENT_PATH = Get-Location  
# Save the current directory to return later

# Function to find the root directory containing the solution file
function Find-ProjectRoot {
    param (
        [string] $currentDir  # Parameter for the current directory
    )

    while ($true) {
        # Find the parent directory of the current directory
        $parentDir = Split-Path -Path $currentDir -Parent
        
        # Get the last folder name from the parent directory path
        $lastFolder = Split-Path -Path $parentDir -Leaf

        # If the last folder name matches the project name, return the path
        if ($lastFolder -eq $PROJECT_NAME) {
            return $parentDir
        }

        # If not, move up one directory in the folder tree
        $currentDir = $parentDir
    }

    return $null  # Return null if the project root folder is not found
}

# Determine the project root path containing the solution file
# Call the Find-ProjectRoot function to find the root directory containing the .sln file
$projectRoot = Find-ProjectRoot -currentDir $(Split-Path -Path $MyInvocation.MyCommand.Path -Parent)
if (-not $projectRoot) {
    Write-Error "No solution file (.sln) found in the directory hierarchy."  
    # If no solution file is found
    exit 1  
    # Exit with error code 1
}

# Set to working directory to project root
Write-Output "Project root path determined: $projectRoot"  
# Output the determined project root path
Set-Location "$projectRoot\$DIR_SOLUTION_FILE"  
# Change to the directory containing the solution file

# Clean project dependencies
Write-Output "Clean project..."
dotnet clean -c $CONFIGURATION_MODE $SOLUTION_FILE
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet format failed"
    exit $LASTEXITCODE
}

# Set back to original directory
Set-Location $CURRENT_PATH

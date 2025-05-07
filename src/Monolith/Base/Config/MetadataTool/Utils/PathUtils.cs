using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MetadataTool.Utils
{
    /// <summary>
    /// Provides utility methods for working with file and directory paths
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        /// Gets the full path from a relative or absolute path
        /// </summary>
        /// <param name="path">The path to convert</param>
        /// <returns>The absolute path</returns>
        public static string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Gets the normalized full path with correct directory separators for the current platform
        /// </summary>
        /// <param name="path">The path to normalize</param>
        /// <returns>Normalized path</returns>
        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            path = Path.GetFullPath(path);

            // Ensure correct directory separators for the platform
            if (Path.DirectorySeparatorChar == '\\')
                return path.Replace('/', '\\');
            else
                return path.Replace('\\', '/');
        }

        /// <summary>
        /// Combines multiple path segments and returns the full path
        /// </summary>
        /// <param name="paths">Path segments to combine</param>
        /// <returns>Combined full path</returns>
        public static string CombineAndGetFullPath(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                throw new ArgumentException("At least one path segment is required", nameof(paths));

            string combined = Path.Combine(paths);
            return Path.GetFullPath(combined);
        }

        /// <summary>
        /// Makes a path relative to a base path
        /// </summary>
        /// <param name="path">The path to convert</param>
        /// <param name="basePath">The base path</param>
        /// <returns>Relative path</returns>
        public static string MakeRelativePath(string path, string basePath)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentException("Base path cannot be null or empty", nameof(basePath));

            // Get full paths
            path = Path.GetFullPath(path);
            basePath = Path.GetFullPath(basePath);

            // Add trailing separator to base path if needed
            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                basePath += Path.DirectorySeparatorChar;

            // Check if path starts with base path
            if (path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
                return path.Substring(basePath.Length);

            // If not, find common root and build relative path with ".."
            Uri baseUri = new Uri(basePath);
            Uri pathUri = new Uri(path);
            Uri relativeUri = baseUri.MakeRelativeUri(pathUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            // Replace forward slashes with the correct directory separator
            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Checks if a path is absolute
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>True if the path is absolute</returns>
        public static bool IsAbsolutePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return Path.IsPathRooted(path)
                && (
                    path.StartsWith(Path.DirectorySeparatorChar.ToString())
                    || Regex.IsMatch(path, @"^[a-zA-Z]:\\")
                );
        }

        /// <summary>
        /// Ensures a path has a trailing directory separator
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>Path with trailing separator</returns>
        public static string EnsureTrailingDirectorySeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path + Path.DirectorySeparatorChar;

            return path;
        }

        /// <summary>
        /// Gets a path without the trailing directory separator
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>Path without trailing separator</returns>
        public static string RemoveTrailingDirectorySeparator(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            return path.TrimEnd(Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Gets the common root path of multiple paths
        /// </summary>
        /// <param name="paths">Collection of paths</param>
        /// <returns>Common root path</returns>
        public static string GetCommonPath(IEnumerable<string> paths)
        {
            if (paths == null || !paths.Any())
                return string.Empty;

            // Get full paths and normalize separators
            string[] normalizedPaths = paths.Select(p => NormalizePath(p)).ToArray();

            // Find the shortest path
            int shortestLength = normalizedPaths.Min(p => p.Length);

            // Find the common prefix
            int prefixLength = 0;
            for (int i = 0; i < shortestLength; i++)
            {
                char c = normalizedPaths[0][i];
                if (normalizedPaths.All(p => p[i] == c))
                    prefixLength++;
                else
                    break;
            }

            // No common prefix
            if (prefixLength == 0)
                return string.Empty;

            // Find the last directory separator in the common prefix
            string commonPrefix = normalizedPaths[0].Substring(0, prefixLength);
            int lastSeparatorPos = commonPrefix.LastIndexOf(Path.DirectorySeparatorChar);

            if (lastSeparatorPos <= 0)
                return string.Empty;

            return commonPrefix.Substring(0, lastSeparatorPos + 1);
        }

        /// <summary>
        /// Gets a temporary file path with the specified extension
        /// </summary>
        /// <param name="extension">File extension (with or without the dot)</param>
        /// <returns>Temporary file path</returns>
        public static string GetTempFilePath(string extension = null)
        {
            string tempPath = Path.GetTempPath();
            string fileName = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(extension))
            {
                if (!extension.StartsWith("."))
                    extension = "." + extension;

                fileName += extension;
            }

            return Path.Combine(tempPath, fileName);
        }

        /// <summary>
        /// Creates a temporary directory
        /// </summary>
        /// <returns>Path to the created temporary directory</returns>
        public static string CreateTempDirectory()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        /// <summary>
        /// Checks if a file path is valid (without checking if it exists)
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>True if the path is valid</returns>
        public static bool IsValidFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            try
            {
                // Check if path contains invalid characters
                string fileName = Path.GetFileName(path);
                string directory = Path.GetDirectoryName(path);

                // Check for reserved device names in Windows
                if (Path.DirectorySeparatorChar == '\\')
                {
                    string[] reservedNames =
                    {
                        "CON",
                        "PRN",
                        "AUX",
                        "NUL",
                        "COM1",
                        "COM2",
                        "COM3",
                        "COM4",
                        "COM5",
                        "COM6",
                        "COM7",
                        "COM8",
                        "COM9",
                        "LPT1",
                        "LPT2",
                        "LPT3",
                        "LPT4",
                        "LPT5",
                        "LPT6",
                        "LPT7",
                        "LPT8",
                        "LPT9"
                    };

                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);
                    if (
                        reservedNames.Contains(fileNameWithoutExt, StringComparer.OrdinalIgnoreCase)
                    )
                        return false;
                }

                // Try to get full path (catches most invalid paths)
                string fullPath = Path.GetFullPath(path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Resolves relative paths in a path string (like "." and "..")
        /// </summary>
        /// <param name="path">The path to resolve</param>
        /// <returns>Resolved path</returns>
        public static string ResolvePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // Replace all forward slashes with back slashes for uniformity
            path = path.Replace('/', Path.DirectorySeparatorChar);

            // Split the path into segments
            string[] segments = path.Split(Path.DirectorySeparatorChar);
            List<string> resolvedSegments = new List<string>();

            foreach (string segment in segments)
            {
                if (segment == ".")
                    continue; // Current directory, ignore
                else if (segment == "..")
                {
                    // Parent directory, remove last segment if possible
                    if (resolvedSegments.Count > 0)
                        resolvedSegments.RemoveAt(resolvedSegments.Count - 1);
                }
                else if (!string.IsNullOrEmpty(segment))
                {
                    resolvedSegments.Add(segment);
                }
            }

            // Check if the original path started with directory separator
            string prefix = path.StartsWith(Path.DirectorySeparatorChar.ToString())
                ? Path.DirectorySeparatorChar.ToString()
                : "";

            // Check for Windows drive letter
            if (segments.Length > 0 && segments[0].EndsWith(":"))
                prefix = segments[0] + Path.DirectorySeparatorChar;

            // Join the segments back
            return prefix + string.Join(Path.DirectorySeparatorChar.ToString(), resolvedSegments);
        }

        /// <summary>
        /// Gets a unique file name in the specified directory
        /// </summary>
        /// <param name="directory">The directory</param>
        /// <param name="fileName">Base file name</param>
        /// <returns>Unique file name</returns>
        public static string GetUniqueFileName(string directory, string fileName)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("Directory cannot be null or empty", nameof(directory));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            // Ensure directory exists
            Directory.CreateDirectory(directory);

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string filePath = Path.Combine(directory, fileName);
            int counter = 1;

            while (File.Exists(filePath))
            {
                string newFileName = $"{fileNameWithoutExt}({counter}){extension}";
                filePath = Path.Combine(directory, newFileName);
                counter++;
            }

            return filePath;
        }

        /// <summary>
        /// Gets the file path with a different extension
        /// </summary>
        /// <param name="filePath">Original file path</param>
        /// <param name="newExtension">New extension (with or without the dot)</param>
        /// <returns>File path with new extension</returns>
        public static string ChangeExtension(string filePath, string newExtension)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            if (string.IsNullOrEmpty(newExtension))
                return Path.ChangeExtension(filePath, null);

            if (!newExtension.StartsWith("."))
                newExtension = "." + newExtension;

            return Path.ChangeExtension(filePath, newExtension);
        }

        /// <summary>
        /// Checks if a directory is a subdirectory of another directory
        /// </summary>
        /// <param name="parentDir">Potential parent directory</param>
        /// <param name="childDir">Potential child directory</param>
        /// <returns>True if childDir is a subdirectory of parentDir</returns>
        public static bool IsSubdirectoryOf(string parentDir, string childDir)
        {
            if (string.IsNullOrEmpty(parentDir) || string.IsNullOrEmpty(childDir))
                return false;

            string normalizedParent = NormalizePath(parentDir);
            string normalizedChild = NormalizePath(childDir);

            normalizedParent = EnsureTrailingDirectorySeparator(normalizedParent);

            return normalizedChild.StartsWith(normalizedParent, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the directory depth from the root
        /// </summary>
        /// <param name="path">The path to analyze</param>
        /// <returns>Depth of the directory</returns>
        public static int GetPathDepth(string path)
        {
            if (string.IsNullOrEmpty(path))
                return 0;

            string fullPath = Path.GetFullPath(path);
            string[] segments = fullPath.Split(
                Path.DirectorySeparatorChar,
                StringSplitOptions.RemoveEmptyEntries
            );

            // Windows drive letter doesn't count as a segment
            if (segments.Length > 0 && segments[0].EndsWith(":"))
                return segments.Length - 1;

            return segments.Length;
        }

        /// <summary>
        /// Computes the relative path from one location to another
        /// </summary>
        /// <param name="fromPath">Source path</param>
        /// <param name="toPath">Target path</param>
        /// <returns>Relative path</returns>
        public static string GetRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath))
                throw new ArgumentException("From path cannot be null or empty", nameof(fromPath));
            if (string.IsNullOrEmpty(toPath))
                throw new ArgumentException("To path cannot be null or empty", nameof(toPath));

            fromPath = Path.GetFullPath(fromPath);
            toPath = Path.GetFullPath(toPath);

            // In .NET Core 2.0+ and .NET 5+, use built-in method
            if (
                System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(
                    ".NET Core"
                )
            )
            {
                return Path.GetRelativePath(fromPath, toPath);
            }

            Uri fromUri = new Uri(AppendDirectorySeparatorIfNeeded(fromPath));
            Uri toUri = new Uri(AppendDirectorySeparatorIfNeeded(toPath));

            if (fromUri.Scheme != toUri.Scheme)
                return toPath;

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Appends a directory separator character to a path if needed
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>Path with directory separator</returns>
        private static string AppendDirectorySeparatorIfNeeded(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path + Path.DirectorySeparatorChar;

            return path;
        }

        /// <summary>
        /// Gets the directory name from a full path
        /// </summary>
        /// <param name="path">Full path</param>
        /// <returns>Directory name (not full path)</returns>
        public static string GetDirectoryName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            string dirPath = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dirPath))
                return string.Empty;

            return new DirectoryInfo(dirPath).Name;
        }

        /// <summary>
        /// Gets the parent directory path
        /// </summary>
        /// <param name="path">Current path</param>
        /// <returns>Parent directory path</returns>
        public static string GetParentDirectoryPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Gets the parent directory path at a specific level
        /// </summary>
        /// <param name="path">Current path</param>
        /// <param name="levels">Number of levels to go up</param>
        /// <returns>Parent directory path</returns>
        public static string GetParentDirectoryPath(string path, int levels)
        {
            if (string.IsNullOrEmpty(path) || levels <= 0)
                return path;

            string result = path;
            for (int i = 0; i < levels; i++)
            {
                result = Path.GetDirectoryName(result);
                if (string.IsNullOrEmpty(result))
                    break;
            }

            return result;
        }

        /// <summary>
        /// Converts a file size in bytes to a human-readable string
        /// </summary>
        /// <param name="bytes">File size in bytes</param>
        /// <returns>Human-readable file size</returns>
        public static string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            int counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }

            return $"{number:n1} {suffixes[counter]}";
        }
    }
}

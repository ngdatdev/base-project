using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConfigTool.MetadataTool.Utils
{
    /// <summary>
    /// Provides utility methods for working with directories
    /// </summary>
    public static class DirectoryUtils
    {
        /// <summary>
        /// Creates a directory if it doesn't exist
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <returns>DirectoryInfo of the created or existing directory</returns>
        public static DirectoryInfo EnsureDirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Directory path cannot be null or empty", nameof(path));

            if (!Directory.Exists(path))
                return Directory.CreateDirectory(path);

            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Creates a directory and all parent directories if they don't exist
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <returns>DirectoryInfo of the created directory</returns>
        public static DirectoryInfo CreateDirectoryRecursive(string path)
        {
            return Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Gets the size of a directory in bytes
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="includeSubdirectories">Whether to include subdirectories</param>
        /// <returns>Size in bytes</returns>
        public static long GetDirectorySize(string directoryPath, bool includeSubdirectories = true)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return GetDirectorySize(directoryInfo, includeSubdirectories);
        }

        /// <summary>
        /// Gets the size of a directory in bytes
        /// </summary>
        /// <param name="directoryInfo">The directory info</param>
        /// <param name="includeSubdirectories">Whether to include subdirectories</param>
        /// <returns>Size in bytes</returns>
        public static long GetDirectorySize(
            DirectoryInfo directoryInfo,
            bool includeSubdirectories = true
        )
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            long size = 0;

            // Add size of all files
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                size += file.Length;
            }

            // Add size of all subdirectories if requested
            if (includeSubdirectories)
            {
                DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    size += GetDirectorySize(subDirectory, true);
                }
            }

            return size;
        }

        /// <summary>
        /// Gets the number of files in a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="searchPattern">Optional search pattern</param>
        /// <param name="includeSubdirectories">Whether to include subdirectories</param>
        /// <returns>Number of files</returns>
        public static int GetFileCount(
            string directoryPath,
            string searchPattern = "*",
            bool includeSubdirectories = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            SearchOption option = includeSubdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(directoryPath, searchPattern, option).Length;
        }

        /// <summary>
        /// Copies a directory and its contents to another location
        /// </summary>
        /// <param name="sourceDirectory">Source directory path</param>
        /// <param name="targetDirectory">Target directory path</param>
        /// <param name="overwrite">Whether to overwrite existing files</param>
        public static void CopyDirectory(
            string sourceDirectory,
            string targetDirectory,
            bool overwrite = false
        )
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException(
                    $"Source directory not found: {sourceDirectory}"
                );

            // Create the target directory if it doesn't exist
            Directory.CreateDirectory(targetDirectory);

            // Copy all files
            foreach (string filePath in Directory.GetFiles(sourceDirectory))
            {
                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(targetDirectory, fileName);
                File.Copy(filePath, targetFilePath, overwrite);
            }

            // Copy all subdirectories
            foreach (string subDirectoryPath in Directory.GetDirectories(sourceDirectory))
            {
                string subDirectoryName = Path.GetFileName(subDirectoryPath);
                string targetSubDirectoryPath = Path.Combine(targetDirectory, subDirectoryName);
                CopyDirectory(subDirectoryPath, targetSubDirectoryPath, overwrite);
            }
        }

        /// <summary>
        /// Moves a directory and its contents to another location
        /// </summary>
        /// <param name="sourceDirectory">Source directory path</param>
        /// <param name="targetDirectory">Target directory path</param>
        /// <param name="overwrite">Whether to overwrite existing files</param>
        public static void MoveDirectory(
            string sourceDirectory,
            string targetDirectory,
            bool overwrite = false
        )
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException(
                    $"Source directory not found: {sourceDirectory}"
                );

            // If the target directory exists and overwrite is true, delete it first
            if (Directory.Exists(targetDirectory))
            {
                if (overwrite)
                    Directory.Delete(targetDirectory, true);
                else
                    throw new IOException($"Target directory already exists: {targetDirectory}");
            }

            // Use built-in Move method if possible
            try
            {
                Directory.Move(sourceDirectory, targetDirectory);
            }
            catch (IOException)
            {
                // If Move failed (e.g., across drives), do copy and delete
                CopyDirectory(sourceDirectory, targetDirectory, overwrite);
                Directory.Delete(sourceDirectory, true);
            }
        }

        /// <summary>
        /// Safely deletes a directory and its contents
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="recursive">Whether to delete subdirectories and files</param>
        /// <returns>True if directory was deleted, false if it didn't exist</returns>
        public static bool SafeDeleteDirectory(string directoryPath, bool recursive = true)
        {
            if (!Directory.Exists(directoryPath))
                return false;

            try
            {
                Directory.Delete(directoryPath, recursive);
                return true;
            }
            catch (IOException)
            {
                // Try to force deletion by making all files not read-only
                if (recursive)
                {
                    foreach (
                        string filePath in Directory.GetFiles(
                            directoryPath,
                            "*",
                            SearchOption.AllDirectories
                        )
                    )
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                    }
                }

                Directory.Delete(directoryPath, recursive);
                return true;
            }
        }

        /// <summary>
        /// Cleans a directory by deleting all files and subdirectories but keeps the directory itself
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        public static void CleanDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            // Delete all files
            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                try
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                catch (Exception)
                { /* Continue with next file */
                }
            }

            // Delete all subdirectories
            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath))
            {
                try
                {
                    Directory.Delete(subDirectoryPath, true);
                }
                catch (Exception)
                { /* Continue with next directory */
                }
            }
        }

        /// <summary>
        /// Creates a backup of a directory with timestamp
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Path to the created backup directory</returns>
        public static string CreateDirectoryBackup(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            string parentDirectory = Path.GetDirectoryName(directoryPath);
            string directoryName = Path.GetFileName(directoryPath);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupPath = Path.Combine(
                parentDirectory,
                $"{directoryName}_backup_{timestamp}"
            );

            CopyDirectory(directoryPath, backupPath);
            return backupPath;
        }

        /// <summary>
        /// Gets all files matching a pattern recursively
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="searchPattern">Search pattern</param>
        /// <param name="includeSubdirectories">Whether to include subdirectories</param>
        /// <returns>Collection of file paths</returns>
        public static IEnumerable<string> GetFilesRecursive(
            string directoryPath,
            string searchPattern = "*",
            bool includeSubdirectories = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            SearchOption option = includeSubdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(directoryPath, searchPattern, option);
        }

        /// <summary>
        /// Gets all files modified after a specific date
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="date">The date to compare against</param>
        /// <param name="includeSubdirectories">Whether to include subdirectories</param>
        /// <returns>Collection of file paths</returns>
        public static IEnumerable<string> GetFilesModifiedAfter(
            string directoryPath,
            DateTime date,
            bool includeSubdirectories = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            SearchOption option = includeSubdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            return Directory
                .GetFiles(directoryPath, "*", option)
                .Where(file => File.GetLastWriteTime(file) > date);
        }

        /// <summary>
        /// Gets all files larger than a specific size
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="minSizeInBytes">Minimum size in bytes</param>
        /// <param name="includeSubdirectories">Whether to include subdirectories</param>
        /// <returns>Collection of file paths</returns>
        public static IEnumerable<string> GetFilesLargerThan(
            string directoryPath,
            long minSizeInBytes,
            bool includeSubdirectories = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            SearchOption option = includeSubdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            return Directory
                .GetFiles(directoryPath, "*", option)
                .Where(file => new FileInfo(file).Length > minSizeInBytes);
        }

        /// <summary>
        /// Gets the creation time of a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Directory creation time</returns>
        public static DateTime GetDirectoryCreationTime(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            return Directory.GetCreationTime(directoryPath);
        }

        /// <summary>
        /// Gets the last write time of a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Directory last write time</returns>
        public static DateTime GetDirectoryLastWriteTime(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            return Directory.GetLastWriteTime(directoryPath);
        }

        /// <summary>
        /// Gets the last access time of a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Directory last access time</returns>
        public static DateTime GetDirectoryLastAccessTime(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            return Directory.GetLastAccessTime(directoryPath);
        }

        /// <summary>
        /// Sets directory attributes
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="attributes">Directory attributes</param>
        public static void SetDirectoryAttributes(string directoryPath, FileAttributes attributes)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            File.SetAttributes(directoryPath, attributes);
        }

        /// <summary>
        /// Gets directory attributes
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Directory attributes</returns>
        public static FileAttributes GetDirectoryAttributes(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            return File.GetAttributes(directoryPath);
        }

        /// <summary>
        /// Sets a directory as read-only
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="recursive">Whether to set all files and subdirectories as read-only</param>
        public static void SetDirectoryReadOnly(string directoryPath, bool recursive = false)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            // Set the directory as read-only
            FileAttributes attributes = File.GetAttributes(directoryPath);
            File.SetAttributes(directoryPath, attributes | FileAttributes.ReadOnly);

            if (recursive)
            {
                // Set all files as read-only
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    File.SetAttributes(
                        filePath,
                        File.GetAttributes(filePath) | FileAttributes.ReadOnly
                    );
                }

                // Set all subdirectories as read-only
                foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath))
                {
                    SetDirectoryReadOnly(subDirectoryPath, true);
                }
            }
        }

        /// <summary>
        /// Removes the read-only attribute from a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="recursive">Whether to remove from all files and subdirectories</param>
        public static void RemoveDirectoryReadOnly(string directoryPath, bool recursive = false)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            // Remove read-only attribute from the directory
            FileAttributes attributes = File.GetAttributes(directoryPath);
            File.SetAttributes(directoryPath, attributes & ~FileAttributes.ReadOnly);

            if (recursive)
            {
                // Remove read-only attribute from all files
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    File.SetAttributes(
                        filePath,
                        File.GetAttributes(filePath) & ~FileAttributes.ReadOnly
                    );
                }

                // Remove read-only attribute from all subdirectories
                foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath))
                {
                    RemoveDirectoryReadOnly(subDirectoryPath, true);
                }
            }
        }

        /// <summary>
        /// Checks if a directory is empty
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>True if the directory is empty</returns>
        public static bool IsDirectoryEmpty(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            return !Directory.EnumerateFileSystemEntries(directoryPath).Any();
        }

        /// <summary>
        /// Gets subdirectories created after a specific date
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="date">The date to compare against</param>
        /// <param name="recursive">Whether to search recursively</param>
        /// <returns>Collection of directory paths</returns>
        public static IEnumerable<string> GetDirectoriesCreatedAfter(
            string directoryPath,
            DateTime date,
            bool recursive = false
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            SearchOption option = recursive
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            return Directory
                .GetDirectories(directoryPath, "*", option)
                .Where(dir => Directory.GetCreationTime(dir) > date);
        }

        /// <summary>
        /// Gets the directory tree as a hierarchical dictionary
        /// </summary>
        /// <param name="directoryPath">The root directory path</param>
        /// <returns>Dictionary representing the directory tree</returns>
        public static Dictionary<string, object> GetDirectoryTree(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            var result = new Dictionary<string, object>();

            // Get files in this directory
            var files = Directory.GetFiles(directoryPath).Select(Path.GetFileName).ToList();

            if (files.Any())
                result["Files"] = files;

            // Get subdirectories
            var subdirectories = Directory.GetDirectories(directoryPath);
            if (subdirectories.Any())
            {
                var subdirsDict = new Dictionary<string, object>();
                foreach (var subdir in subdirectories)
                {
                    string name = Path.GetFileName(subdir);
                    subdirsDict[name] = GetDirectoryTree(subdir);
                }
                result["Directories"] = subdirsDict;
            }

            return result;
        }

        /// <summary>
        /// Flattens a directory structure by moving all files to the root directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="searchPattern">Optional search pattern</param>
        /// <param name="handleDuplicates">How to handle duplicate file names</param>
        public static void FlattenDirectory(
            string directoryPath,
            string searchPattern = "*",
            DuplicateFileHandling handleDuplicates = DuplicateFileHandling.Rename
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            // Get all files in subdirectories
            var files = Directory
                .GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories)
                .Where(file =>
                    !Path.GetDirectoryName(file)
                        .Equals(directoryPath, StringComparison.OrdinalIgnoreCase)
                );

            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                string targetPath = Path.Combine(directoryPath, fileName);

                if (File.Exists(targetPath))
                {
                    switch (handleDuplicates)
                    {
                        case DuplicateFileHandling.Skip:
                            continue;

                        case DuplicateFileHandling.Overwrite:
                            File.Copy(file, targetPath, true);
                            break;

                        case DuplicateFileHandling.Rename:
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                            string extension = Path.GetExtension(fileName);
                            int counter = 1;

                            while (File.Exists(targetPath))
                            {
                                string newFileName = $"{fileNameWithoutExt}_{counter}{extension}";
                                targetPath = Path.Combine(directoryPath, newFileName);
                                counter++;
                            }

                            File.Copy(file, targetPath);
                            break;
                    }
                }
                else
                {
                    File.Copy(file, targetPath);
                }

                // Optionally, delete the original file
                // File.Delete(file);
            }
        }

        /// <summary>
        /// Enum for handling duplicate files when flattening directories
        /// </summary>
        public enum DuplicateFileHandling
        {
            Skip,
            Overwrite,
            Rename
        }

        /// <summary>
        /// Synchronizes two directories (one-way sync from source to target)
        /// </summary>
        /// <param name="sourceDirectory">Source directory</param>
        /// <param name="targetDirectory">Target directory</param>
        /// <param name="deleteExtra">Whether to delete files in target that don't exist in source</param>
        /// <returns>Sync summary with counts of added, updated, and deleted files</returns>
        public static SyncSummary SynchronizeDirectories(
            string sourceDirectory,
            string targetDirectory,
            bool deleteExtra = false
        )
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException(
                    $"Source directory not found: {sourceDirectory}"
                );

            // Create target directory if it doesn't exist
            Directory.CreateDirectory(targetDirectory);

            var summary = new SyncSummary();

            // Copy files from source to target
            foreach (
                string sourceFile in Directory.GetFiles(
                    sourceDirectory,
                    "*",
                    SearchOption.AllDirectories
                )
            )
            {
                string relativePath = sourceFile
                    .Substring(sourceDirectory.Length)
                    .TrimStart(Path.DirectorySeparatorChar);
                string targetFile = Path.Combine(targetDirectory, relativePath);

                // Create target subdirectory if needed
                string targetSubDir = Path.GetDirectoryName(targetFile);
                if (!Directory.Exists(targetSubDir))
                    Directory.CreateDirectory(targetSubDir);

                if (!File.Exists(targetFile))
                {
                    // File doesn't exist in target, copy it
                    File.Copy(sourceFile, targetFile);
                    summary.AddedFiles++;
                }
                else
                {
                    // File exists, compare file sizes and modification dates
                    FileInfo sourceInfo = new FileInfo(sourceFile);
                    FileInfo targetInfo = new FileInfo(targetFile);

                    if (
                        sourceInfo.Length != targetInfo.Length
                        || sourceInfo.LastWriteTime > targetInfo.LastWriteTime
                    )
                    {
                        // Source file is different, update target
                        File.Copy(sourceFile, targetFile, true);
                        summary.UpdatedFiles++;
                    }
                }
            }

            // Delete extra files in target if requested
            if (deleteExtra)
            {
                foreach (
                    string targetFile in Directory.GetFiles(
                        targetDirectory,
                        "*",
                        SearchOption.AllDirectories
                    )
                )
                {
                    string relativePath = targetFile
                        .Substring(targetDirectory.Length)
                        .TrimStart(Path.DirectorySeparatorChar);
                    string sourceFile = Path.Combine(sourceDirectory, relativePath);

                    if (!File.Exists(sourceFile))
                    {
                        // File exists in target but not in source, delete it
                        File.Delete(targetFile);
                        summary.DeletedFiles++;
                    }
                }

                // Delete empty directories in target
                DeleteEmptyDirectories(targetDirectory);
            }

            return summary;
        }

        /// <summary>
        /// Summary of directory synchronization
        /// </summary>
        public class SyncSummary
        {
            public int AddedFiles { get; set; }
            public int UpdatedFiles { get; set; }
            public int DeletedFiles { get; set; }

            public int TotalChanges => AddedFiles + UpdatedFiles + DeletedFiles;
        }

        /// <summary>
        /// Deletes empty directories recursively
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <returns>Number of directories deleted</returns>
        public static int DeleteEmptyDirectories(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                return 0;

            int count = 0;

            // Process all subdirectories first
            foreach (string subdir in Directory.GetDirectories(directoryPath))
            {
                count += DeleteEmptyDirectories(subdir);
            }

            // Check if this directory is now empty
            if (!Directory.EnumerateFileSystemEntries(directoryPath).Any())
            {
                try
                {
                    Directory.Delete(directoryPath);
                    count++;
                }
                catch (IOException)
                { /* Ignore if can't delete */
                }
            }

            return count;
        }

        /// <summary>
        /// Gets a temporary directory path
        /// </summary>
        /// <param name="prefix">Optional prefix for the directory name</param>
        /// <returns>Path to a temporary directory (not created yet)</returns>
        public static string GetTempDirectoryPath(string prefix = null)
        {
            string tempPath = Path.GetTempPath();
            string dirName = (prefix ?? string.Empty) + Guid.NewGuid().ToString();
            return Path.Combine(tempPath, dirName);
        }

        /// <summary>
        /// Creates a temporary directory
        /// </summary>
        /// <param name="prefix">Optional prefix for the directory name</param>
        /// <returns>DirectoryInfo of the created directory</returns>
        public static DirectoryInfo CreateTempDirectory(string prefix = null)
        {
            string path = GetTempDirectoryPath(prefix);
            return Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Calculates MD5 hash for all files in a directory
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="relativePaths">Whether to use relative paths as keys</param>
        /// <returns>Dictionary mapping file paths to MD5 hashes</returns>
        public static Dictionary<string, string> CalculateDirectoryMD5(
            string directoryPath,
            bool relativePaths = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            var result = new Dictionary<string, string>();

            foreach (
                string filePath in Directory.GetFiles(
                    directoryPath,
                    "*",
                    SearchOption.AllDirectories
                )
            )
            {
                string key = filePath;
                if (relativePaths)
                {
                    key = filePath
                        .Substring(directoryPath.Length)
                        .TrimStart(Path.DirectorySeparatorChar);
                }

                string hash = CalculateFileMD5(filePath);
                result[key] = hash;
            }

            return result;
        }

        /// <summary>
        /// Calculates MD5 hash for a file
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>MD5 hash as a hex string</returns>
        private static string CalculateFileMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("x2"));
                    }

                    return sb.ToString();
                }
            }
        }

        /// <summary>
        /// Watches a directory for changes and executes a callback when changes occur
        /// </summary>
        /// <param name="directoryPath">The directory to watch</param>
        /// <param name="callback">The callback to execute</param>
        /// <param name="filter">The file filter</param>
        /// <param name="includeSubdirectories">Whether to watch subdirectories</param>
        /// <returns>FileSystemWatcher instance (caller should dispose)</returns>
        public static FileSystemWatcher WatchDirectory(
            string directoryPath,
            Action<FileSystemEventArgs> callback,
            string filter = "*.*",
            bool includeSubdirectories = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = directoryPath,
                Filter = filter,
                NotifyFilter =
                    NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                IncludeSubdirectories = includeSubdirectories
            };

            // Add event handlers
            watcher.Changed += (sender, e) => callback(e);
            watcher.Created += (sender, e) => callback(e);
            watcher.Deleted += (sender, e) => callback(e);
            watcher.Renamed += (sender, e) => callback(e);

            // Start watching
            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        /// <summary>
        /// Lists all empty directories
        /// </summary>
        /// <param name="directoryPath">The root directory path</param>
        /// <param name="includeSubdirectories">Whether to check subdirectories</param>
        /// <returns>Collection of empty directory paths</returns>
        public static IEnumerable<string> GetEmptyDirectories(
            string directoryPath,
            bool includeSubdirectories = true
        )
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            List<string> emptyDirs = new List<string>();

            // Check if the root directory is empty
            if (!Directory.EnumerateFileSystemEntries(directoryPath).Any())
            {
                emptyDirs.Add(directoryPath);
            }
            else if (includeSubdirectories)
            {
                // Check subdirectories
                foreach (string subdir in Directory.GetDirectories(directoryPath))
                {
                    emptyDirs.AddRange(GetEmptyDirectories(subdir, true));
                }
            }

            return emptyDirs;
        }
    }
}

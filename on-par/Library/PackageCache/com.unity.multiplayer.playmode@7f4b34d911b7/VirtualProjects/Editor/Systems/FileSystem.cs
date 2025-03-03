using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Unity.Multiplayer.Playmode.VirtualProjects.Editor
{
    struct FileSystemDelegates
    {   // This simply represents the static methods of a FileSystem
        public delegate string GetParentPath(string path);
        public delegate void CreateDirectory(string projectPath);
        public delegate void DeleteDirectory(string path);
        public delegate string[] GetDirectoryNames(string path);
        public delegate bool IsPathValid(string path);
        public delegate bool SymlinkFile(string sourcePath, string destination, out CreateAPIErrorInfo info);
        public delegate void CopyFile(string sourcePath, string destinationPath);
        public delegate string ReadFile(string path);
        public delegate void WriteFile(string path, string content);
        public delegate bool ExistsFile(string path);
        public delegate void DeleteFile(string path);
        public delegate string LastFileWriteTime(string path);
        public delegate byte[] ReadBytes(string path);
        public delegate void WriteBytes(string path, byte[] content);

        public GetParentPath GetParentPathFunc;
        public CreateDirectory CreateDirectoryFunc;
        public DeleteDirectory DeleteDirectoryFunc;
        public GetDirectoryNames GetDirectoryNamesFunc;
        public IsPathValid IsPathValidFunc;
        public SymlinkFile SymlinkFileFunc;
        public CopyFile CopyFileFunc;
        public ReadFile ReadFileFunc;
        public WriteFile WriteFileFunc;
        public ExistsFile ExistsFileFunc;
        public DeleteFile DeleteFileFunc;
        public LastFileWriteTime LastFileWriteTimeFunc;
        public ReadBytes ReadBytesFunc;
        public WriteBytes WriteBytesFunc;
    }

    static class FileSystem
    {
        // We subtract longest file length found in this package from the max length supported by Windows
        // to account for additional path length applied to subdirectories (i.e 259 minus 77 chars)
        const int k_MaxPathLengthForWindows = 182;
        const string k_WindowsExtendedPathLengthPrefix = "\\\\?\\";

        public static FileSystemDelegates Delegates { get; } = new FileSystemDelegates
        {
            GetParentPathFunc = GetParentPath,
            CreateDirectoryFunc = CreateDirectory,
            DeleteDirectoryFunc = DeleteDirectory,
            GetDirectoryNamesFunc = GetDirectoryNames,
            IsPathValidFunc = IsPathValid,
            SymlinkFileFunc = SymlinkFile,
            CopyFileFunc = CopyFile,
            ReadFileFunc = ReadFile,
            WriteFileFunc = WriteFile,
            ExistsFileFunc = ExistsFile,
            DeleteFileFunc = DeleteFile,
            LastFileWriteTimeFunc = LastFileWriteTime,
            ReadBytesFunc = ReadBytes,
            WriteBytesFunc = WriteBytes,
        };

        static string GetParentPath(string path)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(path), $"Could not extract parent path. {nameof(path)}");
            return new DirectoryInfo(path).Parent?.FullName ?? string.Empty;
        }

        internal static bool IsPathValid(string path) =>
            !string.IsNullOrWhiteSpace(path) &&
            !(Application.platform == RuntimePlatform.WindowsEditor && path.Length > k_MaxPathLengthForWindows);

        static void CopyFile(string sourcePath, string destinationPath)
        {
            File.Copy(sourcePath, destinationPath);
        }

        static bool SymlinkFile(string source, string destination, out CreateAPIErrorInfo errorState)
        {
            // build the symlink command and then run it for every folder in our required folders
            var (buildLinkCommand, filename, arguments) = BuildLinkCommand(source, destination);
            if (!ProcessSystem.TryRunProcessWaitForExit(filename, arguments, out _, out var error))
            {
                errorState = new CreateAPIErrorInfo
                {
                    Error = CreateAPIError.SymLinkUnableToBePerformed,
                    ShellCommand = buildLinkCommand,
                    ShellError = error,
                };
                return false;
            }

            errorState = default;
            return true;
        }

        static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        static void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        static bool ExistsFile(string path)
        {
            return File.Exists(path);
        }

        static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        static string LastFileWriteTime(string path)
        {
            return File.GetLastWriteTime(path).ToLongTimeString();
        }
        static byte[] ReadBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        static void WriteBytes(string path, byte[] content)
        {
            File.WriteAllBytes(path, content);
        }

        static void CreateDirectory(string projectPath)
        {
            if (Directory.Exists(projectPath))
            {
                return;
            }

            Directory.CreateDirectory(projectPath);
        }

        static string[] GetDirectoryNames(string path)
        {
            if (!Directory.Exists(path))
            {
                return Array.Empty<string>();
            }

            var directories = Directory.GetDirectories(path);
            var select = new List<string>();
            foreach (var id in directories)
            {
                select.Add(Path.GetFileName(id));
            }
            return select.ToArray();
        }

        static void DeleteDirectory(string path)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                // Deleting on windows works better with bash OS cmd call
                var (removeDirectoryCommand, filename, arguments) = WindowsBuildRemoveDirectoryCommand(path);
                if (!ProcessSystem.TryRunProcessWaitForExit(filename, arguments, out _, out var error))
                {
                    Debug.LogError($"Command Failed: {removeDirectoryCommand}{Environment.NewLine}Error:{Environment.NewLine}{error}");
                }
            }
            else
            {
                Directory.Delete(path, true);
            }
        }

        static (string buildLinkCommand, string filename, string arguments) BuildLinkCommand(string sourcePath, string destinationPath)
        {
            string buildLinkCommand;
            switch (Application.platform)
            {
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.OSXEditor:
                    sourcePath = sourcePath.Replace("'", "'\\''");
                    destinationPath = destinationPath.Replace("'", "'\\''");
                    buildLinkCommand = $"ln -s '{sourcePath}' '{destinationPath}'";
                    break;
                case RuntimePlatform.WindowsEditor:
                    buildLinkCommand =
                        $"mklink /J \"{k_WindowsExtendedPathLengthPrefix}{destinationPath}\" \"{k_WindowsExtendedPathLengthPrefix}{sourcePath}\"";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Application.platform));
            }

            var filename = "/bin/bash";
            var argumentPrefix = "-c";

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                filename = "cmd.exe";
                argumentPrefix = "/C";
            }

            var arguments = $"{argumentPrefix} \"{buildLinkCommand}\"";
            return (buildLinkCommand, filename, arguments);
        }

        static (string removeDirectoryCommand, string filename, string arguments) WindowsBuildRemoveDirectoryCommand(string path)
        {
            // Windows has many issues deleting folders containing symlinks,
            // and will most of the time throw an UnauthorizedAccessException
            // when attempting to delete with Directory.Delete.
            var removeDirectoryCommand = $"rmdir /s /q \"{path}\"";
            var filename = "cmd.exe";
            var argumentPrefix = "/C";
            var arguments = $"{argumentPrefix} \"{removeDirectoryCommand}\"";

            return (removeDirectoryCommand, filename, arguments);
        }
    }
}

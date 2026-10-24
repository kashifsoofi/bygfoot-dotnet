using System.Diagnostics;
using Bygfoot.Cli.Models;

namespace Bygfoot.Cli;

/// <summary>
/// Provides utility methods for handling file system operations, 
/// configuration loading/saving, and archives (ZIP).
/// Corresponds to the functionality found in src/file.h and src/file.c.
/// </summary>
public static class FileHelper
{
    // Constants and shared resources from file.h/file.c context:
    private const int WriteBufferSize = 8192;
    private const int MaxFilename = 256;

    // --- Application Startup & Path Management ---

    /// <summary>
    /// Adds a definition directory.
    /// Corresponds to C: add_definitions_directory(const gchar *directory).
    /// </summary>
    /// <param name="directory">The full path of the directory to be added.</param>
    public static void AddDefinitionsDirectory(string directory)
    {
        string[] dirSplitUp = directory.Split(Path.DirectorySeparatorChar);
        if (dirSplitUp.Length > 0 && dirSplitUp[dirSplitUp.Length - 1] == "definitions")
        {
            Variables.RootDefinitionsDirectories.Insert(0, directory);
        }
        if (Misc.QueryMiscStringStartsWith(directory, Variables.RootDefinitionsDirectories))
        {
            Variables.DefinitionsDirectories.Insert(0, directory);
        }
    }

    /// <summary>
    /// Add the specified directory to the list of directories FindSupportFile()
    /// searches for support files.
    /// Any subdirectories are added recursively.
    /// Corresponds to C: file_add_support_directory_recursive(const gchar *directory).
    /// </summary>
    /// <param name="directory">The full path of the directory to be added.</param>
    public static void FileAddSupportDirectoryRecursive(string directory)
    {
        if (!Directory.Exists(directory))
            return;

        /* Ignore .svn directories */
        if (directory.Contains(".svn") || directory.Contains(".git"))
            return;

        AddDefinitionsDirectory(directory);
        // Migration of add_pixmap_directory(const gchar *directory) - not yet converted (src/support.c).
        Variables.SupportDirectories.Insert(0, directory);

        foreach (string subdirectory in Directory.EnumerateDirectories(directory))
            FileAddSupportDirectoryRecursive(subdirectory);
    }

    /// <summary>
    /// Search the list of support directories for a given file and return
    /// the full path name.
    /// Corresponds to C: file_find_support_file(const Gchar *filename, gboolean warning).
    /// </summary>
    /// <param name="filename">The name of the file we look for (without path).</param>
    /// <param name="warning">Whether to show a warning if we don't find the file.</param>
    /// <returns>The full path of the file or null if we didn't find the file.</returns>
    public static string FindSupportFile(string filename, bool warning)
    {
        foreach (string directory in Variables.SupportDirectories)
        {
            string pathname = Path.Combine(directory, filename);
            if (File.Exists(pathname))
                return pathname;
        }

        if (warning)
            Debug.DebugPrintMessage($"file_find_support_file: file '{filename}' not found.");

        return null;
    }

    /// <summary>
    /// Execute command with 'system' and give a warning if return value is -1.
    /// Corresponds to C: file_my_system(const GString *command).
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>True on success, false otherwise.</returns>
    public static bool MySystem(string command)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Variables.OsIsUnix ? "/bin/sh" : "cmd.exe",
                Arguments = Variables.OsIsUnix ? $"-c \"{command}\"" : $"/c {command}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            Debug.DebugPrintMessage($"file_my_system: system returned -1 when executing '{command}'.");

            if (!Variables.OsIsUnix)
            {
                Debug.DebugPrintMessage("Press RETURN to try to continue.");
                Console.ReadKey();
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// A custom function opening files.
    /// Corresponds to C: file_my_fopen(const gchar *filename, gchar *bits, FILE **fil, gboolean abort_program).
    /// </summary>
    /// <param name="filename">The full path to the file or a file name from the support files.</param>
    /// <param name="bits">The mode we use, e.g. "r" or "w". See fopen().</param>
    /// <param name="fil">The stream that will point to the opened file.</param>
    /// <param name="abortProgram">Whether to abort the program if we encounter an error.</param>
    /// <returns>True on success, false otherwise.</returns>
    public static bool MyFopen(string filename, string bits, out Stream fil, bool abortProgram)
    {
        fil = Fopen(filename, bits);

        if (fil != null)
            return true;

        string supportFile = FindSupportFile(filename, false);
        fil = Fopen(supportFile, bits);

        if (fil != null)
            return true;

        Debug.DebugPrintMessage($"Could not open file '{filename}' in mode '{bits}'.");

        if (abortProgram)
            Environment.Exit(3); // Corresponds to EXIT_FILE_OPEN_FAILED (3)

        return false;
    }

    /// <summary>
    /// Creates a $HOME/.bygfoot directory and other stuff if necessary.
    /// </summary>
    public static void CheckHomeDirCreateDirs()
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string[] dirs = new string[]
        {
            Path.Combine(home, ".bygfoot"),
            Path.Combine(home, ".bygfoot", "definitions"),
            Path.Combine(home, ".bygfoot", "saves")
        };

        foreach (string dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }

    /// <summary>
    /// Adds the basic config files to the list of files to copy
    /// into the user home dir.
    /// </summary>
    public static void CheckHomeDirGetConfFiles(out List<string> filesToCopy, out List<string> targetPaths)
    {
        filesToCopy = new List<string>();
        targetPaths = new List<string>();
        string[] confFiles = new string[] { "bygfoot.conf", "bygfoot_user.conf" };
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        foreach (string fileName in confFiles)
        {
            string fullPath = Path.Combine(home, ".bygfoot", fileName);
            if (!File.Exists(fullPath))
            {
                string supportFile = FindSupportFile(fileName, true);
                if (supportFile != null)
                {
                    filesToCopy.Add(supportFile);
                    targetPaths.Add(fullPath);
                }
            }
        }
    }

    /// <summary>
    /// Step through the support dirs to find xml definition files
    /// that get copied into the home dir.
    /// </summary>
    public static void CheckHomeDirGetDefinitionFiles(out List<string> sourceFiles, out List<string> targetPaths)
    {
        sourceFiles = new List<string>();
        targetPaths = new List<string>();
        // This is a complex recursive process in C. For now, we mark it as not implemented.
        throw new NotImplementedException("CheckHomeDirGetDefinitionFiles is not yet implemented.");
    }

    /// <summary>
    /// Step through the support dirs to find xml definition files
    /// that get copied into the home dir.
    /// </summary>
    public static void CheckHomeDirCopyFiles(List<string> sourceFiles, List<string> targetPaths)
    {
        if (sourceFiles.Count == 0)
            return;

        for (int i = 0; i < sourceFiles.Count; i++)
        {
            Console.WriteLine($"Copying {Path.GetFileName(sourceFiles[i])} to {targetPaths[i]}...");
            File.Copy(sourceFiles[i], targetPaths[i], true);
        }
    }

    /// <summary>
    /// Step through the support dirs to find xml definition files
    /// that get copied into the home dir.
    /// </summary>
    public static void CheckHomeDir()
    {
        CheckHomeDirCreateDirs();
        CheckHomeDirGetConfFiles(out var filesToCopy, out var targetPaths);
        CheckHomeDirGetDefinitionFiles(out var definitionFiles, out var definitionTargets);
        
        var allSources = filesToCopy;
        allSources.AddRange(definitionFiles);
        var allTargets = targetPaths;
        allTargets.AddRange(definitionTargets);

        CheckHomeDirCopyFiles(allSources, allTargets);
    }

    /// <summary>
    /// Retrieve those files in the given directory
    /// that start with the given prefix and suffix.
    /// </summary>
    public static List<string> GetDirectoryContents(string directoryName, string prefix = "", string suffix = "")
    {
        List<string> contents = new List<string>();
        if (!Directory.Exists(directoryName))
            return contents;

        foreach (string file in Directory.GetFiles(directoryName))
        {
            string name = Path.GetFileName(file);
            if (string.IsNullOrEmpty(prefix) || name.StartsWith(prefix))
            {
                if (string.IsNullOrEmpty(suffix) || name.EndsWith(suffix))
                {
                    contents.Add(file);
                }
            }
        }
        return contents;
    }

    /// <summary>
    /// Return the country definition files found in the support dirs.
    /// </summary>
    public static List<string> GetCountryFiles()
    {
        throw new NotImplementedException("GetCountryFiles is not yet implemented.");
    }

    /// <summary>
    /// Get the next option line from a file.
    /// </summary>
    private static bool GetNextOptLine(StreamReader reader, out string optName, out string optValue)
    {
        optName = "";
        optValue = "";
        string line = reader.ReadLine();
        if (line == null) return false;

        line = line.TrimStart();
        if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
            return GetNextOptLine(reader, out optName, out optValue);

        int commentIdx = line.IndexOf('#');
        if (commentIdx != -1)
            line = line.Substring(0, commentIdx).TrimEnd();

        string[] parts = line.Split(new[] { ' ', '\t' }, 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            optName = parts[0];
            optValue = parts.Length > 2 ? parts[2] : "";
            return true;
        }

        return false;
    }

    /// <summary>
    /// Saves an optionlist to a file.
    /// </summary>
    public static void SaveOptFile(string filename, OptionList optionList)
    {
        throw new NotImplementedException("SaveOptFile is not yet implemented.");
    }

    /// <summary>
    /// Load a file containing name - value pairs into
    /// the specified array.
    /// </summary>
    public static void LoadOptFile(string filename, OptionList optionList, bool sort)
    {
        throw new NotImplementedException("LoadOptFile is not yet implemented.");
    }

    /// <summary>
    /// Load the appropriate hints file.
    /// </summary>
    public static void LoadHintsFile()
    {
        throw new NotImplementedException("LoadHintsFile is not yet implemented.");
    }

    /// <summary>
    /// Load the options at the beginning of a new game from
    /// the configuration files.
    /// </summary>
    public static void LoadConfigFiles()
    {
        throw new NotImplementedException("LoadConfigFiles is not yet implemented.");
    }

    /// <summary>
    /// Load a user-specific conf file.
    /// </summary>
    public static void LoadUserConfFile(User user)
    {
        throw new NotImplementedException("LoadUserConfFile is not yet implemented.");
    }

    /// <summary>
    /// Returns the path to the user's home directory for Bygfoot.
    /// </summary>
    public static string GetBygfootDir()
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (OsIsUnix)
            return Path.Combine(home, ".bygfoot");
        else
            return Directory.GetCurrentDirectory();
    }

    /// <summary>
    /// Returns the first support directory which has the given suffix.
    /// </summary>
    public static string GetFirstSupportDirSuffix(string suffix)
    {
        foreach (var dir in SupportDirectories)
        {
            if (dir.EndsWith(suffix))
                return dir;
        }
        return null;
    }

    /// <summary>
    /// Compresses the files starting with the prefix.
    /// </summary>
    public static void CompressFiles(string destZipFile, string prefix)
    {
        throw new NotImplementedException("CompressFiles is not yet implemented.");
    }

    /// <summary>
    /// Extracts files from a specified archive.
    /// </summary>
    public static void Decompress(string filename)
    {
        throw new NotImplementedException("Decompress is not yet implemented.");
    }

    /// <summary>
    /// Removes the files/directories specified.
    /// </summary>
    public static void RemoveFiles(List<string> filesToRemove)
    {
        foreach (var file in filesToRemove)
        {
            if (File.Exists(file))
                File.Delete(file);
            else if (Directory.Exists(file))
                Directory.Delete(file, true);
        }
    }

    /// <summary>
    /// Copies a file from a source path to a destination path.
    /// </summary>
    public static void CopyFile(string sourceFile, string destFile)
    {
        File.Copy(sourceFile, destFile, true);
    }

    /// <summary>
    /// Stores text data into the designated saves directory.
    /// </summary>
    public static void StoreTextInSaves(string filename, string text)
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string saveDir;
        if (OsIsUnix)
            saveDir = Path.Combine(home, ".bygfoot", "saves");
        else
            saveDir = Path.Combine(Directory.GetCurrentDirectory(), "saves");

        if (!Directory.Exists(saveDir))
            Directory.CreateDirectory(saveDir);

        string fullPath = Path.Combine(saveDir, filename);
        File.WriteAllText(fullPath, text);
    }

    /// <summary>
        /// Loads text data from the designated saves directory.
    /// </summary>
    public static string LoadTextFromSaves(string filename)
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string saveDir;
        if (OsIsUnix)
            saveDir = Path.Combine(home, ".bygfoot", "saves");
        else
            saveDir = Path.Combine(Directory.GetCurrentDirectory(), "saves");

        string fullPath = Path.Combine(saveDir, filename);
        if (File.Exists(fullPath))
            return File.ReadAllText(fullPath);

        return null;
    }

    /// <summary>
    /// A custom function opening files.
    /// </summary>
    private static Stream Fopen(string filename, string mode)
    {
        if (string.IsNullOrEmpty(filename))
            return null;

        try
        {
            return mode switch
            {
                "r" => new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare. Read),
                "r+" => new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare. Read),
                "w" => new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare. Read),
                "w+" => new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare. Read),
                "a" => new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare. Read),
                "a+" => new FileStream(filename, FileMode.Append, FileAccess.ReadWrite, FileShare. Read),
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }
}
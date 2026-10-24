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

    /// <summary>Exit when a file could not be opened.
    /// Corresponds to EXIT_FILE_OPEN_FAILED in the ExitCodes enum in bygfoot.h.</summary>
    private const int ExitFileOpenFailed = 3;

    // --- Application Startup & Path Management ---

    /// <summary>
    /// Adds a definition directory.
    /// Corresponds to C: add_definitions_directory(const gchar *directory).
    /// </summary>
    /// <param name="directory">The full path of the directory to be added.</param>
    public static void AddDefinitionsDirectory(string directory)
    {
        string[] dirSplitUp = directory.Split(Path.DirectorySeparatorChar);
        if (dirSplitUp[dirSplitUp.Length - 1] == "definitions")
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
    /// Corresponds to C: file_find_support_file(const gchar *filename, gboolean warning).
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
            Environment.Exit(ExitFileOpenFailed);

        return false;
    }

    /// <summary>
    /// Gets the primary supported directory path (e.g., user/Bygfoot/).
    /// </summary>
    public static string GetFirstSupportDirectory()
    {
        // Migration of file_get_first_support_dir(void)
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Placeholder path equivalent
    }

    // --- Configuration and Data Persistence ---

    /// <summary>
    /// Loads game options from a specified configuration file (e.g., bygfoot.conf).
    /// </summary>
    /// <param name="filename">Path to the configuration file.</param>
    /// <param name="optionList">The OptionList object to populate with data.</param>
    /// <param name="sort">Whether options should be sorted after loading.</param>
    public static void LoadOptionFile(string filename, OptionList optionList, bool sort)
    {
        // Migration of file_load_opt_file(const gchar *filename, OptionList *optionlist, gboolean sort)
        Console.WriteLine($"Loading option file: {filename}");
    }

    /// <summary>
    /// Saves the current game options to a specified file.
    /// </summary>
    public static void SaveOptionFile(string filename, OptionList optionList)
    {
        // Migration of file_save_opt_file(const gchar *filename, OptionList *optionlist)
        Console.WriteLine($"Saving options to file: {filename}");
    }

    /// <summary>
    /// Retrieves the contents of a directory, filtering by prefix and suffix.
    /// </summary>
    /// <param name="directoryName">The directory path.</param>
    /// <param name="prefix">Optional file prefix to match.</param>
    /// <param name="suffix">Optional file suffix to match.</param>
    /// <returns>A list of matching filenames/paths.</returns>
    public static List<string> GetDirectoryContents(string directoryName, string prefix = null, string suffix = null)
    {
        // Migration of file_dir_get_contents(const gchar *dir_name, const gchar *prefix, const gchar *suffix)
        return new List<string>(); // Placeholder
    }

    // --- Game Data Flow Methods ---

    /// <summary>
    /// Loads configuration files needed at startup (e.g., default settings).
    /// </summary>
    public static void LoadConfigFiles()
    {
        // Migration of file_load_conf_files(void)
        Console.WriteLine("Loading initial configuration files.");
    }

    /// <summary>
    /// Loads a specific user's configuration file.
    /// </summary>
    public static void LoadUserConfFile(User user)
    {
        // Migration of file_load_user_conf_file(User *user)
        Console.WriteLine($"Loading user configuration for {user.Name}.");
    }

    // --- Text I/O (Saves) ---

    /// <summary>
    /// Saves plain text data into the designated saves directory.
    /// </summary>
    public static void StoreTextInSaves(string filename, string text)
    {
        // Migration of file_store_text_in_saves(const gchar *filename, const gchar *text)
        Console.WriteLine($"Storing text data for {filename} in saves.");
    }

    /// <summary>
    /// Loads plain text data from the designated saves directory.
    /// </summary>
    /// <returns>The loaded text, or null if the file could not be found/read.</returns>
    public static string LoadTextFromSaves(string filename)
    {
        // Migration of file_load_text_from_saves(const gchar *filename)
        Console.WriteLine($"Loading text data from {filename}.");
        return null; // Placeholder
    }


    // --- Advanced File Operations (Advanced/Less Frequently Used) ---

    /// <summary>
    /// Compresses files in a directory structure into a single archive.
    /// </summary>
    public static void CompressFiles(string destZipFile, string prefix)
    {
        // Migration of file_compress_files(const gchar *destfile, const gchar *prefix)
        Console.WriteLine($"Compressing files into {destZipFile}...");
    }

    /// <summary>
    /// Extracts files from a specified archive.
    /// </summary>
    public static void Decompress(string filename)
    {
        // Migration of file_decompress(const gchar *filename)
        Console.WriteLine($"Decompressing archive: {filename}");
    }

    /// <summary>
    /// Copies a file from a source path to a destination path. Handles necessary directory creation if needed.
    /// </summary>
    public static void CopyFile(string sourceFile, string destFile)
    {
        // Migration of file_copy_file(const gchar *source_file, const gchar *dest_file)
        Console.WriteLine($"Copying {sourceFile} to {destFile}");
    }

    /// <summary>
    /// Removes files or directories referenced in the array.
    /// </summary>
    public static void RemoveFiles(List<string> filesToRemove)
    {
        // Migration of file_remove_files(const GPtrArray *files)
        Console.WriteLine($"Removing files/directories...");
    }

    /// <summary>Open a file in the specified mode (fopen).
    /// Corresponds to C: fopen(const char *filename, const char *mode).</summary>
    /// <param name="filename">The name of the file to open (may be null).</param>
    /// <param name="bits">The mode we use, e.g. "r" or "w". See fopen().</param>
    /// <returns>The opened stream or null if the file could not be opened.</returns>
    private static Stream Fopen(string filename, string bits)
    {
        if (filename == null)
            return null;

        try
        {
            switch (bits)
            {
                case "r":
                    return new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                case "r+":
                    return new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                case "w":
                    return new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
                case "w+":
                    return new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                case "a":
                    return new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read);
                case "a+":
                    return new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read);
                default:
                    return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}
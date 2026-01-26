using Bygfoot.Domain;
using Bygfoot.Store.Extensions;

namespace Bygfoot.Store;

public class FileHelper : IFileHelper
{
    const string HomeDirName = ".bygfoot";
    private static string CurrentDir => Directory.GetCurrentDirectory();
    
    public string PackageDataDir => @"../../../.."; //ConfigurationManager.AppSettings["PACKAGE_DATA_DIR"];
    private readonly string PackageLocaleDir = CurrentDir;
    private readonly List<string> _supportDirectories = [];
    private readonly List<string> _rootDefinitionsDirectories = [];
    private readonly List<string> _definitionsDirectories = [];
    
    private bool OsIsUnix => Environment.OSVersion.Platform == PlatformID.Unix;
    private static string HomeDir => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    
    public string GetBygfootDir()
    {
        if (OsIsUnix)
        {
            return Path.Combine(HomeDir, HomeDirName);
        }

        return CurrentDir;
    }

    public FileHelper()
    {
        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PackageDataDir);
        baseDir = Path.GetFullPath(baseDir);
        string supportFilesDir = Path.Combine(baseDir, "support_files");

        AddSupportDirectoryRecursive(supportFilesDir);
        AddSupportDirectoryRecursive(GetBygfootDir());
    }
    
    private void AddSupportDirectoryRecursive(string directory)
    {
        if (!Directory.Exists(directory))
            return;

        /* Ignore .svn directories */
        if (directory.Contains(".svn") || directory.Contains(".git"))
            return;

        AddDefinitionsDirectory(directory);
        //Support.AddPixmapDirectory(directory);
        _supportDirectories.Add(directory);
        foreach (string subdir in Directory.EnumerateDirectories(directory))
        {
            AddSupportDirectoryRecursive(subdir);
        }
    }

    private void AddDefinitionsDirectory(string directory)
    {
        // _logger.Debug("FileHelper.AddDefinitionsDirectory");

        string[] dirNames = directory.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        if (dirNames[dirNames.Length - 1].ToLower().Equals("definitions"))
        {
            _rootDefinitionsDirectories.Add(directory);
        }
        if (_rootDefinitionsDirectories.FindAll(delegate (string s) {
                return s.StartsWith(directory);
            }).Count > 0)
        {
            _definitionsDirectories.Add(directory);
        }
    }
    
    public bool Exists(string path)
    {
        return File.Exists(path);
    }
    
    public string? FindSupportDirectory(string directoryName, bool warning)
    {
        // _logger.Debug("FileHelper.FindSupportDirectory");

        directoryName = Path.GetFileName(directoryName);
        foreach (string directory in _supportDirectories)
        {
            string directoryPath = Path.Combine(directory, directoryName);
            if (Directory.Exists(directoryPath))
                return directoryPath;
        }

        //if (warning)
        //  Debug.PrintMessage("FileHelper.FindSupportDirectory: directory '{0}' not found.", directoryName);
        return null;
    }
    
    public string FindSupportFile(string filename, bool warning)
    {
        // _logger.Debug("FileHelper.FindSupportFile");

        filename = Path.GetFileName(filename);
        foreach (string directory in _supportDirectories)
        {
            string filepath = Path.Combine(directory, filename);
            if (File.Exists(filepath))
                return filepath;
        }

        //if (warning)
        //  Debug.PrintMessage("FileHelper.FindSupportFile: file '{0}' not found.", filename);
        return null;
    }
    
    public OptionsList LoadOptionsFile(string filename, bool sort)
    {
        var supportFile = FindSupportFile(filename, false);
        var lines = File.ReadAllLines(supportFile);

        var optionsList = new OptionsList();
        foreach (var line in lines)
        {
            var (optionName, optionValue, found) = line.ParseOptionLine();
            if (!found)
            {
                continue;
            }

            var option = new Option(optionName);
            if (optionName.StartsWith("string_"))
            {
                option.StringValue = optionValue;
            }
            else
            {
                option.Value = int.Parse(optionValue);
            }
            optionsList.Add(option);

            if ((OsIsUnix && optionName.EndsWith("_unix")) ||
                (!OsIsUnix && optionName.EndsWith("_win32")))
            {
                optionName = optionName.Remove(optionName.Length - (OsIsUnix ? 5 : 6));
                var option2 = new Option(optionName)
                {
                    StringValue = optionValue
                };
                optionsList.Add(option2);
            }
        }

        if (sort)
        {
            optionsList.Sort();
        }

        return optionsList;
    }
}
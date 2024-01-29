using Bygfoot.Domain;

namespace Bygfoot.Store;

public class FileStore : IStore
{
    const string HomeDirName = ".bygfoot";

    private readonly string PackageDataDir = @"../../../.."; //ConfigurationManager.AppSettings["PACKAGE_DATA_DIR"];
    private readonly string PackageLocaleDir = CurrentDir;
    private readonly List<string> _supportDirectories = new List<string>();
    private readonly List<string> _rootDefinitionsDirectories = new List<string>();
    private readonly List<string> _definitionsDirectories = new List<string>();

    public IHelpStore HelpStore { get; }
    public IHintsStore HintsStore { get; }

    public FileStore()
    {
        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PackageDataDir);
        baseDir = Path.GetFullPath(baseDir);
        string supportFilesDir = Path.Combine(baseDir, "support_files");

        AddSupportDirectoryRecursive(supportFilesDir);
        AddSupportDirectoryRecursive(GetBygfootDir());

        HelpStore = new HelpStore(this);
        HintsStore = new HintsStore(this);
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

    public static bool Exists(string path)
    {
        return File.Exists(path);
    }

    public static string HomeDir =>
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public static string CurrentDir => Directory.GetCurrentDirectory();

    public static bool OsIsUnix => Environment.OSVersion.Platform == PlatformID.Unix;

    public static string GetBygfootDir()
    {
        if (OsIsUnix)
        {
            return Path.Combine(HomeDir, HomeDirName);
        }

        return CurrentDir;
    }

    public string? FindSupportFile(string filename, bool warning)
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
        //  Debug.PrintMessage("FindSupportFile: file '{0}' not found.", filename);
        return null;
    }

    /** Return the country definition files found in the support dirs.
      * The files are appended with the directories*/
    public string[] GetCountryFiles()
    {
        // _logger.Debug("FileHelper.GetCountryFiles");

        List<string> countryFiles = new List<string>();
        foreach (string dir in _definitionsDirectories)
        {
            string[] dirContents = Directory.GetFiles(dir, "country_*.xml", SearchOption.AllDirectories);

            foreach (string item in dirContents)
            {
                string countryFile = item.Substring(dir.Length + (dir.EndsWith(Path.DirectorySeparatorChar.ToString()) ? 0 : 1));
                if (countryFiles.IndexOf(countryFile) == -1)
                    countryFiles.Add(countryFile);
            }
        }

        return countryFiles.ToArray();
    }

    public (string, string, bool) ParseOptionLine(string line)
    {
        if (line.IndexOf('#') >= 0)
            line = line.Substring(0, line.IndexOf('#'));
        if (string.IsNullOrEmpty(line))
        {
            return ("", "", false);
        }

        return line.Cut(" ");
    }

    public OptionsList LoadOptionsFile(string filename, bool sort)
    {
        var supportFile = FindSupportFile(filename, false);
        var lines = File.ReadAllLines(supportFile);

        var optionsList = new OptionsList();
        foreach (var line in lines)
        {
            var (optionName, optionValue, found) = ParseOptionLine(line);
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

    /** Load the options at the beginning of a new game from
      * the configuration files. */
    public void LoadConfFiles()
    {
        // _logger.Debug("FileHelper.LoadConfFiles");

        var confFile = FindSupportFile("bygfoot.conf", true);
        Variables.Options = LoadOptionsFile(confFile, false);
        /*//TODO 
        LoadOptFile(Option.OptStr("string_opt_constants_file"), ref Variables.Constants, true);
        LoadOptFile(Option.OptStr("string_opt_appearance_file"), ref Variables.ConstantsApp, true);
        LoadOptFile("bygfoot_tokens", Variables.Tokens, false);
        LoadHintsFile();

        for (int i = 0; i < Variables.Tokens.list.Count; i++)
        {
            OptionStruct option = ((OptionStruct)(Variables.Tokens.list[i]));
            option.value = i;
        }
        */
    }

    public string? LoadTextFromSaves(string filename)
    {
        // _logger.Debug("FileHelper.LoadTextFromSaves");

        string? filepath = null;
        if (Variables.os_is_unix)
        {
            string home = HomeDir;
            //TODO filepath = string.Format ("{0}{1}{2}{1}saves{1}{3}", home, Path.DirectorySeparatorChar, Bygfoot.HOMEDIRNAME, filename);
        }
        else
        {
            string pwd = CurrentDir;
            filepath = string.Format("{0}{1}saves{1}{2}", pwd, Path.DirectorySeparatorChar, filename);
        }

        if (!File.Exists(filepath))
            return null;
        return File.ReadAllText(filepath);
    }
}

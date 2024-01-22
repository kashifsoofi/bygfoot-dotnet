namespace Bygfoot.Domain;

public class FileHelper
{
    // private static Logger _logger = new LoggerConfiguration().CreateLogger();

    private static string PACKAGE_DATA_DIR = @"..\..\.."; //ConfigurationManager.AppSettings["PACKAGE_DATA_DIR"];
    private static string PACKAGE_LOCALE_DIR = GetCurrentDir();
    private static string HOMEDIRNAME = ".bygfoot";

    private static List<string> _supportDirectories = null;
    private static List<string> _rootDefinitionsDirectories = null;
    private static List<string> _definitionsDirectories = null;

    static FileHelper()
    {
        _rootDefinitionsDirectories = new List<string>();
        _definitionsDirectories = new List<string>();

        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PACKAGE_DATA_DIR);
        baseDir = Path.GetFullPath(baseDir);
        string supportFilesDir = Path.Combine(baseDir, "support_files");
        AddSupportDirectoryRecursive(supportFilesDir);
        AddSupportDirectoryRecursive(Path.Combine(GetHomeDir(), HOMEDIRNAME));
    }

    /**
      * Adds a definition directory
      **/
    public static void AddDefinitionsDirectory(string directory)
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

    public static void AddSupportDirectoryRecursive(string directory)
    {
        // _logger.Debug("FileHelper.AddSupportDirectoryRecursive");

        if (!Directory.Exists(directory))
            return;

        /* Ignore .svn directories */
        if (directory.Contains(".svn"))
            return;

        AddDefinitionsDirectory(directory);
        //Support.AddPixmapDirectory(directory);
        if (_supportDirectories == null)
            _supportDirectories = new List<string>();
        _supportDirectories.Add(directory);
        foreach (string subdir in Directory.EnumerateDirectories(directory))
        {
            AddSupportDirectoryRecursive(subdir);
        }
    }

    public static string FindSupportFile(string filename, bool warning)
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

    public static void CheckHomeDir()
    {
        // _logger.Debug("FileHelper.CheckHomeDir");
    }

    public static void GetOptFromLine(string line, ref string optName, ref string optValue)
    {
        // _logger.Debug("FileHelper.GetOptFromLine");

        if (line.IndexOf('#') >= 0)
            line = line.Substring(0, line.IndexOf('#'));

    }

    public static bool ParseOptLine(string line, ref string optName, ref string optValue)
    {
        optName = null;
        optValue = null;
        if (line.IndexOf('#') >= 0)
            line = line.Substring(0, line.IndexOf('#'));
        if (string.IsNullOrEmpty(line))
            return false;

        int valueIndex = line.IndexOfAny(new char[] { ' ', '\t' });
        if (valueIndex == -1)
            valueIndex = line.Length;
        optName = line.Substring(0, valueIndex);
        if (valueIndex < line.Length)
            optValue = line.Substring(valueIndex + 1);
        return true;
    }

    /** Return the country definition files found in the support dirs.
        * The files are appended with the directories*/
    public static string[] GetCountryFiles()
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

    /** Load a file containing name - value pairs into
      * the specified array. */
    public static OptionsList LoadOptFile(string filename, bool sort)
    {
        // _logger.Debug("FileHelper.LoadOptFile");

        var optionsList = new OptionsList();
        string path = FindSupportFile(filename, false);
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            string optName = null, optValue = null;
            if (ParseOptLine(line, ref optName, ref optValue))
            {
                optionsList.Add(new Option(optName, optValue));

                if ((optName.EndsWith("_unix") && Variables.os_is_unix) ||
                    (optName.EndsWith("_win32") && !Variables.os_is_unix))
                {
                    optName = optName.Remove(optName.IndexOf(Variables.os_is_unix ? "_unix" : "_win32"));
                    optionsList.Add(new Option(optName, optValue));
                }
            }
        }

        if (sort)
        {
            optionsList.Sort();
        }

        return optionsList;
    }

    public static string GetHomeDir()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }

    public static string GetCurrentDir()
    {
        return Directory.GetCurrentDirectory();
    }

    /** Find out where the Bygfoot directory we can write to resides
      * and write the location into the string. */
    public static string GetBygfootDir()
    {
        // _logger.Debug("FileHelper.GetBygfootDir");

        string homePath = GetHomeDir();
        string currentPath = string.Format("{0}{1}", GetCurrentDir(), Path.DirectorySeparatorChar);
        if (Variables.os_is_unix)
        {
            string appPath = Path.Combine(homePath, HOMEDIRNAME);
            return File.Exists(appPath) ? appPath : currentPath;
        }
        else
        {
            return currentPath;
        }
    }

    /** Load the appropriate hints file. */
    public static OptionsList LoadHintsFile()
    {
        // _logger.Debug("FileHelper.LoadHintsFile");

        string langCode = Language.GetCode();
        string hintsFile = string.Format("bygfoot_hints_{0}", langCode);
        string hintsPath = FindSupportFile(hintsFile, false);
        if (string.IsNullOrEmpty(hintsPath))
            hintsFile = "bygfoot_hints_en";

        Variables.hints = LoadOptFile(hintsFile, false);
        return Variables.hints;
    }

    /** Load the options at the beginning of a new game from
      * the configuration files. */
    public static void LoadConfFiles()
    {
        // _logger.Debug("FileHelper.LoadConfFiles");

        string confFile = FindSupportFile("bygfoot.conf", true);
        Variables.Options = LoadOptFile(confFile, false);
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

    public static string LoadTextFromSaves(string filename)
    {
        // _logger.Debug("FileHelper.LoadTextFromSaves");

        string filepath = null;
        if (Variables.os_is_unix)
        {
            string home = GetHomeDir();
            //TODO filepath = string.Format ("{0}{1}{2}{1}saves{1}{3}", home, Path.DirectorySeparatorChar, Bygfoot.HOMEDIRNAME, filename);
        }
        else
        {
            string pwd = GetCurrentDir();
            filepath = string.Format("{0}{1}saves{1}{2}", pwd, Path.DirectorySeparatorChar, filename);
        }

        if (!File.Exists(filepath))
            return null;
        return File.ReadAllText(filepath);
    }
}

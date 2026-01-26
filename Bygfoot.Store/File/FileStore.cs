using Bygfoot.Domain;

namespace Bygfoot.Store;

public class FileStore : IStore
{
    public IHelpStore HelpStore { get; }
    public IHintsStore HintsStore { get; }
    public IDefinitionsStore DefinitionsStore { get; }

    public FileStore(IFileHelper fileHelper)
    {
        HelpStore = new HelpStore(fileHelper);
        HintsStore = new HintsStore(fileHelper);
        DefinitionsStore = new DefinitionsStore(fileHelper);
    }
    
    public static string CurrentDir => Directory.GetCurrentDirectory();

    public static bool OsIsUnix => Environment.OSVersion.Platform == PlatformID.Unix;
    
    /** Return the country definition files found in the support dirs.
      * The files are appended with the directories*/
    // public string[] GetCountryFiles()
    // {
    //     // _logger.Debug("FileHelper.GetCountryFiles");
    //
    //     List<string> countryFiles = new List<string>();
    //     foreach (string dir in _definitionsDirectories)
    //     {
    //         string[] dirContents = Directory.GetFiles(dir, "country_*.xml", SearchOption.AllDirectories);
    //
    //         foreach (string item in dirContents)
    //         {
    //             string countryFile = item.Substring(dir.Length + (dir.EndsWith(Path.DirectorySeparatorChar.ToString()) ? 0 : 1));
    //             if (countryFiles.IndexOf(countryFile) == -1)
    //                 countryFiles.Add(countryFile);
    //         }
    //     }
    //
    //     return countryFiles.ToArray();
    // }
    
    /** Load the options at the beginning of a new game from
      * the configuration files. */
//     public void LoadConfFiles()
//     {
//         // _logger.Debug("FileHelper.LoadConfFiles");
//
//         var confFile = FindSupportFile("bygfoot.conf", true);
//         Variables.Options = LoadOptionsFile(confFile, false);
//         /*//TODO 
//         LoadOptFile(Option.OptStr("string_opt_constants_file"), ref Variables.Constants, true);
//         LoadOptFile(Option.OptStr("string_opt_appearance_file"), ref Variables.ConstantsApp, true);
//         LoadOptFile("bygfoot_tokens", Variables.Tokens, false);
//         LoadHintsFile();
//
//         for (int i = 0; i < Variables.Tokens.list.Count; i++)
//         {
//             OptionStruct option = ((OptionStruct)(Variables.Tokens.list[i]));
//             option.value = i;
//         }
//         */
//     }

    // public string? LoadTextFromSaves(string filename)
    // {
    //     // _logger.Debug("FileHelper.LoadTextFromSaves");
    //
    //     string? filepath = null;
    //     if (Variables.os_is_unix)
    //     {
    //         string home = HomeDir;
    //         //TODO filepath = string.Format ("{0}{1}{2}{1}saves{1}{3}", home, Path.DirectorySeparatorChar, Bygfoot.HOMEDIRNAME, filename);
    //     }
    //     else
    //     {
    //         string pwd = CurrentDir;
    //         filepath = string.Format("{0}{1}saves{1}{2}", pwd, Path.DirectorySeparatorChar, filename);
    //     }
    //
    //     if (!File.Exists(filepath))
    //         return null;
    //     return File.ReadAllText(filepath);
    // }
}

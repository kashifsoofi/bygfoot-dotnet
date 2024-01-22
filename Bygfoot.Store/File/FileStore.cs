using Bygfoot.Domain;

namespace Bygfoot.Store;

public class FileStore : IStore
{
    const string HomeDirName = ".bygfoot";

    private readonly IHintsStore _hintsStore;

    public FileStore()
    {
        _hintsStore = new HintsStore();
    }

    public IHintsStore HintsStore => _hintsStore;

    public static bool Exists(string path)
    {
        return File.Exists(path);
    }

    public static string HomeDir =>
        Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

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

    public static string FindSupportFile(string filename)
    {
        return "";
    }

    public static OptionsList LoadOptionsFile(string filename)
    {
        //var supportFile = FindSupportFile(filename)
        var supportFile = filename;
        var lines = File.ReadAllLines(supportFile);

        var optionsList = new OptionsList();
        foreach (var line in lines)
        {
            var (name, value, found) = line.Cut(" ");
            if (!found)
            {
                continue;
            }

            Option option;
            if (name.StartsWith("string_"))
            {
                option = new Option(name, value);
            }
            else
            {
                option = new Option(name, int.Parse(value));
            }
            optionsList.Add(option);

            if ((OsIsUnix && name.EndsWith("_unix")) ||
                (!OsIsUnix && name.EndsWith("_win32")))
            {
                name = name.Remove(name.Length - (OsIsUnix ? 5 : 6));
                var option2 = new Option(name, value);
                optionsList.Add(option2);
            }
        }

        return optionsList;
    }
}

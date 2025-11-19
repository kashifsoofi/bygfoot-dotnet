namespace Bygfoot.Store;

public class DefinitionsStore(FileStore fileStore) : IDefinitionsStore
{
    private const string DefinitionsName = "definitions";

    public string[] GetContinents()
    {
        var definitionsPath = fileStore.FindSupportDirectory(DefinitionsName, false);
        if (definitionsPath == null)
        {
            return [];
        }
        
        return Directory.GetDirectories(definitionsPath).Select(x => Path.GetFileName(x!)).ToArray();
    }

    public string[] GetCountryNames(string continent)
    {
        var continentPath = fileStore.FindSupportDirectory(continent, false);
        if (continentPath == null)
        {
            return [];
        }
        
        return Directory.GetDirectories(continentPath).Select(x => Path.GetFileName(x!)).ToArray();
    }
}

using Bygfoot.Domain;

namespace Bygfoot.Store;

public class DefinitionsStore(IFileHelper fileHelper) : IDefinitionsStore
{
    private const string DefinitionsName = "definitions";

    public string[] GetContinents()
    {
        var definitionsPath = fileHelper.FindSupportDirectory(DefinitionsName, false);
        if (definitionsPath == null)
        {
            return [];
        }
        
        return Directory.GetDirectories(definitionsPath).Select(x => Path.GetFileName(x!)).ToArray();
    }

    public string[] GetCountryNames(string continent)
    {
        var continentPath = fileHelper.FindSupportDirectory(continent, false);
        if (continentPath == null)
        {
            return [];
        }
        
        return Directory.GetDirectories(continentPath).Select(x => Path.GetFileName(x!)).ToArray();
    }
}


using Bygfoot.Domain;

namespace Bygfoot.Store;

public class HintsStore : IHintsStore
{
    private string HintNumPath => Path.Combine($"{FileStore.GetBygfootDir()}", "hint_num");

    private readonly FileStore _fileStore;

    public HintsStore(FileStore fileStore)
    {
        _fileStore = fileStore;
    }

    public List<string> GetHints()
    {
        var optionsList = LoadHintsFile();
        var hints = new List<string>();
        for (var i = 0; i < optionsList.Count; i++)
        {
            hints.Add(optionsList[i].StringValue);
        }
        return hints;
    }

    public int LoadHintNumber()
    {
        var hintNumPath = HintNumPath;
        if (!FileStore.Exists(HintNumPath))
        {
            return 0;
        }

        var hintNumStr = File.ReadAllText(hintNumPath);
        if (!int.TryParse(hintNumStr, out var hintNum))
        {
            return 0;
        }

        return hintNum;
    }

    public void SaveHintNumber(int hintNum)
    {
        var hintNumPath = HintNumPath;
        File.WriteAllText(hintNumPath, hintNum.ToString());
    }

    private string GetLanguageCode()
    {
        return "en";
    }

    private OptionsList LoadHintsFile()
    {
        var hintsFileName = $"bygfoot_hints_{GetLanguageCode()}";
        var hintsFilePath = _fileStore.FindSupportFile(hintsFileName, false);
        if (hintsFilePath == null)
        {
            hintsFileName = "bygfoot_hints_en";
        }

        return _fileStore.LoadOptionsFile(hintsFileName, false);
    }
}

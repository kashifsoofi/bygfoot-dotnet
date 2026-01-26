
using Bygfoot.Domain;

namespace Bygfoot.Store;

public class HelpStore : IHelpStore
{
    private const string HelpFileName = "bygfoot_help";

    private readonly IFileHelper _fileHelper;

    public HelpStore(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    public List<Contributor> GetContributors()
    {
        var contributors = new List<Contributor>();

        var helpFilePath = _fileHelper.FindSupportFile(HelpFileName, true);
        if (helpFilePath == null)
        {
            // TODO game_gui_show_warning (probably in caller)
            return contributors;
        }

        var optionsList = _fileHelper.LoadOptionsFile(helpFilePath, false);
        for (var i = 0; i < optionsList.Count; i++)
        {
            if (optionsList[i].Name == "string_contrib_title")
            {
                contributors.Add(new Contributor(optionsList[i]?.StringValue));
            }
            else
            {
                contributors[contributors.Count - 1].Entries.Add(optionsList[i]?.StringValue);
            }
        }
        return contributors;
    }

    private OptionsList? LoadHelpFile()
    {
        var helpFilePath = _fileHelper.FindSupportFile(HelpFileName, true);
        if (helpFilePath == null)
        {
            // TODO game_gui_show_warning (probably in caller)
            return null;
        }

        return _fileHelper.LoadOptionsFile(helpFilePath!, false);
    }
}

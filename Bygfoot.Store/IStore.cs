namespace Bygfoot.Store;

public interface IStore
{
    IHintsStore HintsStore { get; }   
    IHelpStore HelpStore { get; }
}

public interface IHintsStore
{
    int LoadHintNumber();
    void SaveHintNumber(int hintNum);
    List<string> GetHints();
}

public record Contributor(string Title)
{
    public List<string> Entries { get; set; } = new List<string>();
}

public interface IHelpStore
{
    List<Contributor>? GetContributors();
}


namespace Bygfoot.Store;

public interface IStore
{
    IHintsStore HintsStore { get; }   
}

public interface IHintsStore
{
    int LoadHintNumber();
    void SaveHintNumber(int hintNum);
    List<string> GetHints();
}

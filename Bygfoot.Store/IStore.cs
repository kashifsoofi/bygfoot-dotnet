namespace Bygfoot.Store;

public interface IStore
{
    
}

public interface IHintsStore
{
    int LoadHintNumber();
    void SaveHintNumber();
    List<string> GetHints();
}

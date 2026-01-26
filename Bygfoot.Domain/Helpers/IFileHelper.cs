namespace Bygfoot.Domain;

public interface IFileHelper
{
    string PackageDataDir { get; }
    bool Exists(string path);
    string GetBygfootDir();
    string? FindSupportDirectory(string directoryName, bool warning);
    string FindSupportFile(string filename, bool warning);

    OptionsList LoadOptionsFile(string filename, bool sort);
}
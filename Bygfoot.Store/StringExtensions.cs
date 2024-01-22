namespace Bygfoot.Store;

public static class StringExtensions
{
    public static (string, string, bool) Cut(this string str, string sep)
    {
        var index = str.IndexOf(sep);
        if (index == -1)
        {
            return ("", "", false);
        }

        var before = str.Substring(0, index);
        var after = str.Substring(index + sep.Length);
        return (before, after, true);
    }
}

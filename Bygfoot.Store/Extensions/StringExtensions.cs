namespace Bygfoot.Store.Extensions;

public static class StringExtensions
{
    extension(string str)
    {
        private (string, string, bool) Cut(string sep)
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

        public (string, string, bool) ParseOptionLine()
        {
            if (str.IndexOf('#') >= 0)
                str = str.Substring(0, str.IndexOf('#'));
            if (string.IsNullOrEmpty(str))
            {
                return ("", "", false);
            }

            return str.Cut(" ");
        }
    }
}
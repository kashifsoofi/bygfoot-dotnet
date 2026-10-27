namespace Bygfoot.Cli.Models;

/// <summary>
/// Language helper functions.
/// Corresponds to the C functions in src/language.c and the macros in src/language.h.
/// </summary>
public static class Language
{
    /// <summary>
    /// Set the game language to the specified one.
    /// Corresponds to C: language_set.
    /// </summary>
    /// <param name="index">The language index.</param>
    public static void LanguageSet(int index)
    {
        // Implementation depends on the not yet converted window and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the index of the given code in the codes array.
    /// Corresponds to C: language_get_code_index.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <returns>The code index.</returns>
    public static int LanguageGetCodeIndex(string code)
    {
        string localCode = code;
        if (code == "en")
            localCode = "C";

        List<string> codes = Misc.MiscSeparateStrings(Variables.Options.GetString("string_language_codes"));
        int returnValue = -1;

        for (int i = 0; i < codes.Count; i++)
        {
            if (localCode == codes[i])
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Compare country file names based on a preferred one (which should get moved to the start).
    /// Corresponds to C: language_compare_country_files.
    /// </summary>
    /// <param name="a">The first country file.</param>
    /// <param name="b">The second country file.</param>
    /// <param name="data">The preferred country file.</param>
    /// <returns>The comparison result.</returns>
    public static int LanguageCompareCountryFiles(object a, object b, object data)
    {
        string def1 = a as string;
        string def2 = b as string;
        string prefdef = data as string;
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        int len1 = def1?.Length ?? 0;
        int len2 = def2?.Length ?? 0;
        int lenmin = Math.Min(len1, len2);
        int returnValue = 0;

        if (string.Equals(def1, def2))
        {
            returnValue = 0;
        }
        else if (prefdef != null && prefdef == def1)
        {
            returnValue = -1;
        }
        else if (prefdef != null && prefdef == def2)
        {
            returnValue = 1;
        }
        else
        {
            int i = 0;
            while (i < lenmin && i < def1.Length && i < def2.Length && def1[i] == def2[i])
                i++;

            if (i < lenmin)
            {
                int pos1 = alphabet.IndexOf(def1[i]);
                int pos2 = alphabet.IndexOf(def2[i]);

                if (pos1 == -1 && pos2 == -1)
                    returnValue = 0;
                else if (pos1 == -1)
                    returnValue = 1;
                else if (pos2 == -1)
                    returnValue = -1;
                else
                    returnValue = pos1 < pos2 ? -1 : 1;
            }
            else
                returnValue = len1 < len2 ? -1 : (len1 > len2 ? 1 : 0);
        }

        return returnValue;
    }

    /// <summary>
    /// Put the country matching the local language to the beginning of the array if possible.
    /// Corresponds to C: language_pick_country.
    /// </summary>
    /// <param name="countryFiles">The country files.</param>
    public static void LanguagePickCountry(List<string> countryFiles)
    {
        List<string> codes = Misc.MiscSeparateStrings(Variables.Options.GetString("string_language_codes"));
        List<string> defs = Misc.MiscSeparateStrings(Variables.Options.GetString("string_language_defs"));
        string prefdef = null;
        string lang = Environment.GetEnvironmentVariable("LANGUAGE");

        if (string.IsNullOrEmpty(lang))
            lang = Environment.GetEnvironmentVariable("LANG");

        if (string.IsNullOrEmpty(lang))
            lang = Environment.GetEnvironmentVariable("LC_ALL");

        if (lang != null)
        {
            for (int i = 0; i < codes.Count; i++)
            {
                if ((lang.StartsWith("en") && codes[i] == "C") || lang.StartsWith(codes[i]))
                {
                    if (i < defs.Count && defs[i] != "NONE")
                    {
                        for (int j = 0; j < countryFiles.Count; j++)
                        {
                            if (countryFiles[j].EndsWith(defs[i]))
                            {
                                prefdef = countryFiles[j];
                                break;
                            }
                        }
                    }

                    break;
                }
            }
        }

        countryFiles.Sort((a, b) => LanguageCompareCountryFiles(a, b, prefdef));
    }

    /// <summary>
    /// Find out which language to use (e.g. for live game commentary).
    /// Write the code (en, de etc.) into the buffer.
    /// Corresponds to C: language_get_code.
    /// </summary>
    /// <param name="buf">The buffer.</param>
    public static void LanguageGetCode(ref string buf)
    {
        string optLanguageCode = Variables.Options.GetString("string_opt_language_code");

        if (optLanguageCode == "C")
        {
            buf = "en";
        }
        else if (optLanguageCode == "" && Environment.GetEnvironmentVariable("LANG") != null)
        {
            string curLocale = Environment.GetEnvironmentVariable("LANG");
            buf = curLocale.Length > 0 ? curLocale.Substring(0, 2) : "en";
        }
        else
        {
            buf = optLanguageCode;
        }
    }
}

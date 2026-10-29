namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// A list of first names and last names from a file. 
/// Corresponds to C struct NameList.
/// </summary>
public class NameList
{
    /// <summary>The file id (the part between 'player_names_' and '.xml'). Corresponds to C: gchar *sid.</summary>
    public string Sid { get; set; }

    /// <summary>Arrays of strings holding the names. Corresponds to C: GPtrArray *first_names, *last_names.</summary>
    public List<string> FirstNames { get; set; } = new List<string>();

    /// <summary>Arrays of strings holding the names. Corresponds to C: GPtrArray *first_names, *last_names.</summary>
    public List<string> LastNames { get; set; } = new List<string>();

    /// <summary>
    /// Get a random player name from the given names list. If the names list is not found, create
    /// it from the file. If the file can't be found, either, make some fuss and take one from the
    /// general names.
    /// Corresponds to C: name_get.
    /// </summary>
    /// <param name="namesFile">The names file.</param>
    /// <returns>A newly created player.</returns>
    public static string Get(string namesFile)
    {
        if (Variables.NameLists.Count > 1 && (float)MathHelper.Rnd(0, 1) < Variables.Constants.GetFloat("float_name_random_list_prob"))
            return GetFromRandomList();

        foreach (NameList nameList in Variables.NameLists)
        {
            if (nameList.Sid == namesFile)
                return GetFromList(nameList);
        }

        NameList newNameList = new NameList();
        NameXml.XmlNameRead(namesFile, newNameList);

        if (newNameList.Sid == null)
            return Get(Variables.Options.GetString("string_opt_player_names_file"));

        ShortenList(newNameList);
        Variables.NameLists.Add(newNameList);

        return GetFromList(Variables.NameLists[Variables.NameLists.Count - 1]);
    }

    /// <summary>
    /// Return a newly allocated string with a randomly picked combined name from the list.
    /// Corresponds to C: name_get_from_list.
    /// </summary>
    /// <param name="namelist">The name list.</param>
    /// <returns>The new name.</returns>
    public static string GetFromList(NameList namelist)
    {
        return $"{GetRandomFirstName(namelist)} {GetRandomLastName(namelist)}";
    }

    /// <summary>
    /// Find the namelist with the given sid. Corresponds to C: name_get_list_from_sid.
    /// </summary>
    /// <param name="sid">The sid of the names file.</param>
    /// <returns>The name list.</returns>
    public static NameList GetListFromSid(string sid)
    {
        foreach (NameList nameList in Variables.NameLists)
        {
            if (nameList.Sid == sid)
                return nameList;
        }

        NameList newNameList = new NameList();
        NameXml.XmlNameRead(sid, newNameList);

        if (newNameList.Sid == null)
            throw new Exception($"name_get_list_from_sid: namelist with sid {sid} not found");

        ShortenList(newNameList);
        Variables.NameLists.Add(newNameList);

        return Variables.NameLists[Variables.NameLists.Count - 1];
    }

    /// <summary>
    /// Get a name from a randomly picked name list. Corresponds to C: name_get_from_random_list.
    /// </summary>
    /// <returns>The new name.</returns>
    public static string GetFromRandomList()
    {
        return GetFromList(Variables.NameLists[MathHelper.Rndi(0, Variables.NameLists.Count - 1)]);
    }

    /// <summary>
    /// Shorten a name list so that it doesn't occupy too much memory. Corresponds to C: name_shorten_list.
    /// </summary>
    /// <param name="namelist">The name list.</param>
    public static void ShortenList(NameList namelist)
    {
        while (namelist.FirstNames.Count * namelist.LastNames.Count > Variables.Constants.GetInt("int_name_max_product"))
        {
            if ((float)namelist.FirstNames.Count / (float)namelist.LastNames.Count > Variables.Constants.GetFloat("float_name_first_last_ratio"))
            {
                int idx = MathHelper.Rndi(0, namelist.FirstNames.Count - 1);
                namelist.FirstNames.RemoveAt(idx);
            }
            else
            {
                int idx = MathHelper.Rndi(0, namelist.LastNames.Count - 1);
                namelist.LastNames.RemoveAt(idx);
            }
        }
    }

    /// <summary>
    /// Get a random first name from the given name list. Corresponds to the C macro name_get_random_first_name.
    /// </summary>
    /// <param name="namelist">The name list.</param>
    /// <returns>The first name.</returns>
    private static string GetRandomFirstName(NameList namelist)
    {
        int idx = MathHelper.Rndi(0, namelist.FirstNames.Count - 1);
        return namelist.FirstNames[idx];
    }

    /// <summary>
    /// Get a random last name from the given name list. Corresponds to the C macro name_get_random_last_name.
    /// </summary>
    /// <param name="namelist">The name list.</param>
    /// <returns>The last name.</returns>
    private static string GetRandomLastName(NameList namelist)
    {
        int idx = MathHelper.Rndi(0, namelist.LastNames.Count - 1);
        return namelist.LastNames[idx];
    }
}

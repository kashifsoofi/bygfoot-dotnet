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
}

namespace Bygfoot.Cli.Models;

/// <summary>
/// A struct representing an option or a constant. 
/// Corresponds to C struct Option.
/// </summary>
public class Option
{
    /// <summary>The name of the option. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>The string value of the option. Corpos to C: gchar *string_value.</summary>
    public string StringValue { get; set; }

    /// <summary>The integer value of the option. Corresponds to C: gint value.</summary>
    public int Value { get; set; }
}

/// <summary>
/// An array of name-value pairs with an associated keyed data list for quick access. 
/// Corresponds to C struct OptionList.
/// </summary>
public class OptionList
{
    /// <summary>The list of options. Corresponds to C: GArray *list.</summary>
    public List<Option> List { get; set; } = new List<Option>();

    /// <summary>The data list for quick access. Corresponds to C: GData *datalist.</summary>
    public Dictionary<string, object> DataList { get; set; } = new Dictionary<string, object>();
}

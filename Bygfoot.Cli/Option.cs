namespace Bygfoot.Cli;

/// <summary>
/// A class representing an option or a constant. 
/// Corresponds to C struct Option.
/// </summary>
public class Option
{
    /// <summary>The name of the option. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The string value of the option. Corresponds to C: gchar *string_value.</summary>
    public string? StringValue { get; set; }

    /// <summary>The integer value of the option. Corresponds to C: gint value.</summary>
    public int Value { get; set; }
}

/// <summary>
/// An array of name-value pairs with an associated keyed data list for quick access. 
/// Corresponds to C struct OptionList.
/// </summary>
public class OptionList
{
    private const int FloatDivisor = 100000;

    /// <summary>The list of options. Corresponds to C: GArray *list.</summary>
    public List<Option> List { get; set; } = new List<Option>();

    /// <summary>The lookup dictionary for quick access. Corresponds to C: GData *datalist.</summary>
    private readonly Dictionary<string, Option> _lookup = new();

    /// <summary>
    /// Removes all options from the list and resets the lookup dictionary.
    /// Corresponds to freeing an option list in C.
    /// </summary>
    public void Clear()
    {
        List.Clear();
        _lookup.Clear();
    }

    public void Add(string name, int intValue, string? stringValue = null)
    {
        if (_lookup.TryGetValue(name, out var existing))
        {
            existing.Value = intValue;
            existing.StringValue = stringValue;
        }
        else
        {
            var newOption = new Option
            {
                Name = name,
                Value = intValue,
                StringValue = stringValue
            };
            List.Add(newOption);
            _lookup[name] = newOption;
        }
    }

    public int GetInt(string name)
    {
        if (_lookup.TryGetValue(name, out var option))
            return option.Value;

        throw new System.Exception($"Option named {name} not found");
    }

    public int GetIntPointer(string name)
    {
        // In C, this returns a pointer. In C#, we'll just return the value or the option.
        // But since we want to mimic the behavior, let's just return the value.
        // Actually, option_int_pointer is used in C.
        return GetInt(name);
    }

    public string GetString(string name)
    {
        if (_lookup.TryGetValue(name, out var option))
            return option.StringValue ?? string.Empty;

        throw new System.Exception($"Option named {name} not found");
    }

    public string? GetStringPointer(string name)
    {
        if (_lookup.TryGetValue(name, out var option))
            return option.StringValue;

        throw new System.Exception($"Option named {name} not found");
    }

    public float GetFloat(string name)
    {
        if (_lookup.TryGetValue(name, out var option))
            return (float)option.Value / FloatDivisor;

        throw new System.Exception($"Option named {name} not found");
    }

    public void SetInt(string name, int newValue)
    {
        if (_lookup.TryGetValue(name, out var option))
            option.Value = newValue;
        else
            throw new System.Exception($"Option named {name} not found");
    }

    public void SetString(string name, string? newValue)
    {
        if (_lookup.TryGetValue(name, out var option))
            option.StringValue = newValue;
        else
            throw new System.Exception($"Option named {name} not found");
    }

    public void SetFloat(string name, float newValue)
    {
        SetInt(name, (int)Math.Round(newValue * FloatDivisor));
    }
}

public static class OptionHelper
{
    public static string OptStr(string name) => Variables.Options.GetString(name);
    public static string? OptStrp(string name) => Variables.Options.GetStringPointer(name);
    public static int OptInt(string name) => Variables.Options.GetInt(name);
    public static int? OptIntp(string name) => Variables.Options.GetIntPointer(name);
    public static float OptFloat(string name) => Variables.Options.GetFloat(name);
    
    public static void OptSetInt(string name, int newValue) => Variables.Options.SetInt(name, newValue);
    public static void OptSetStr(string name, string newValue) => Variables.Options.SetString(name, newValue);
    public static void OptSetFloat(string name, float newValue) => Variables.Options.SetFloat(name, newValue);
    
    public static string OptUserStr(string name) => Variables.Users[Variables.CurUser].Options.GetString(name);
    public static string? OptUserStrp(string name) => Variables.Users[Variables.CurUser].Options.GetStringPointer(name);
    public static int OptUserInt(string name) => Variables.Users[Variables.CurUser].Options.GetInt(name);
    public static int? OptUserIntp(string name) => Variables.Users[Variables.CurUser].Options.GetIntPointer(name);
    public static float OptUserFloat(string name) => Variables.Users[Variables.CurUser].Options.GetFloat(name);
    
    public static void OptUserSetInt(string name, int newValue) => Variables.Users[Variables.CurUser].Options.SetInt(name, newValue);
    public static void OptUserSetStr(string name, string newValue) => Variables.Users[Variables.CurUser].Options.SetString(name, newValue);
    public static void OptUserSetFloat(string name, float newValue) => Variables.Users[Variables.CurUser].Options.SetFloat(name, newValue);
    
    public static string ConstApp(string name) => Variables.ConstantsApp.GetString(name);
    public static string ConstStr(string name) => Variables.Constants.GetString(name);
    public static int ConstInt(string name) => Variables.Constants.GetInt(name);
    public static float ConstFloat(string name) => Variables.Constants.GetFloat(name);
    
    public static int SettInt(string name) => Variables.Settings.GetInt(name); 
    public static void SettSetInt(string name, int newValue) => Variables.Settings.SetInt(name, newValue);
}
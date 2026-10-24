namespace Bygfoot.Cli.Models;

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

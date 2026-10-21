namespace Bygfoot.Cli;

/// <summary>
/// Represents a single configurable option, mirroring the C Option struct.
/// This holds the data for one setting (name, value, string representation).
/// </summary>
public class Option
{
    /// <summary>
    /// The unique name of the option. (Corresponds to gchar *name)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The primary integer value used for internal storage. (Corresponds to gint value)
    /// </summary>
    public int IntegerValue { get; set; } 

    /// <summary>
    /// The textual representation of the option. (Corresponds to gchar *string_value)
    /// </summary>
    public string StringValue { get; set; } 

    // Constructor used when initializing a new option.
    public Option(string name, int initialValue)
    {
        Name = name;
        IntegerValue = initialValue;
        StringValue = null; 
    }

    // Private constructor for internal use.
    private Option() { }
}

/// <summary>
/// Manages a collection of configurable options, mirroring the behavior and functionality of OptionList.
/// It provides high-level methods to safely interact with options, including scaling for floats.
/// </summary>
public class OptionsManager
{
    // Using a Dictionary provides O(1) lookup time, which is far superior to the underlying C array implementation.
    private readonly Dictionary<string, Option> optionsDictionary = new Dictionary<string, Option>();

    // The C code used OPTION_FLOAT_DIVISOR = 100000. This constant must be maintained for correct scaling.
    private const int FloatDivisor = 100000;

    /// <summary>
    /// Adds a new option or updates an existing one.
    /// Corresponds to option_add in src/option.c.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <param name="initialValue">The initial integer value (scaled).</param>
    /// <param name="stringValue">An optional initial string description/value.</param>
    public void AddOption(string name, int initialValue, string stringValue = null)
    {
        if (optionsDictionary.ContainsKey(name))
        {
            // If the option exists, update its values (matching C behavior).
            var existingOption = optionsDictionary[name];
            existingOption.IntegerValue = initialValue;
            existingOption.StringValue = stringValue; 
        }
        else
        {
            // If it's new, add it.
            var newOption = new Option(name, initialValue);
            newOption.StringValue = stringValue;
            optionsDictionary[name] = newOption;
        }
    }

    // --- Retrieval Methods (Read Operations) ---

    /// <summary>
    /// Retrieves the integer value for a named option. Throws KeyNotFoundException if missing, mirroring fatal error checks in C code.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <returns>The integer value stored for the option.</returns>
    public int GetIntOption(string name)
    {
        if (!optionsDictionary.TryGetValue(name, out var option))
        {
            throw new KeyNotFoundException($"Option '{name}' not found in the configuration.");
        }
        return option.IntegerValue;
    }

    /// <summary>
    /// Retrieves the string value for a named option. Throws if missing.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <returns>The string value associated with the option.</returns>
    public string GetStringOption(string name)
    {
        if (!optionsDictionary.TryGetValue(name, out var option))
        {
            throw new KeyNotFoundException($"Option '{name}' not found in the configuration.");
        }
        return option.StringValue;
    }

    /// <summary>
    /// Retrieves the floating point value for a named option. (Scales internal integer back to float).
    /// Corresponds to option_float in src/option.c.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <returns>The actual floating point value.</returns>
    public double GetFloatOption(string name)
    {
        if (!optionsDictionary.TryGetValue(name, out var option))
        {
            throw new KeyNotFoundException($"Option '{name}' not found in the configuration.");
        }
        // C reads integer value and divides by OPTION_FLOAT_DIVISOR (100000).
        return (double)option.IntegerValue / FloatDivisor;
    }

    // --- Setting Methods (Write Operations) ---

    /// <summary>
    /// Sets the integer value for a named option. Throws if the option doesn't exist in the list.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <param name="value">The new integer value to store (scaled).</param>
    public void SetIntOption(string name, int value)
    {
        if (!optionsDictionary.ContainsKey(name))
        {
            throw new KeyNotFoundException($"Cannot set option '{name}'. It does not exist.");
        }
        optionsDictionary[name].IntegerValue = value;
    }

    /// <summary>
    /// Sets the string value for a named option. Throws if the option doesn't exist in the list.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <param name="newValue">The new string value.</param>
    public void SetStringOption(string name, string newValue)
    {
        if (!optionsDictionary.ContainsKey(name))
        {
            throw new KeyNotFoundException($"Cannot set option '{name}'. It does not exist.");
        }
        optionsDictionary[name].StringValue = newValue;
    }

    /// <summary>
    /// Sets the floating point value for a named option. Multiplies input by the scaling factor (100000) before storing it as an integer.
    /// </summary>
    /// <param name="name">The unique name of the option.</param>
    /// <param name="floatValue">The desired floating point value.</param>
    public void SetFloatOption(string name, double floatValue)
    {
        if (!optionsDictionary.ContainsKey(name))
        {
            throw new KeyNotFoundException($"Cannot set option '{name}'. It does not exist.");
        }
        var option = optionsDictionary[name];
        // Store value scaled by FloatDivisor (100000).
        option.IntegerValue = (int)Math.Round(floatValue * FloatDivisor);
    }
}
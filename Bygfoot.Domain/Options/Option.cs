namespace Bygfoot.Domain;

public class Option
{
    const float OPTION_FLOAT_DIVISOR = (float)100000.0;

    public string Name { get; set; }
    public string StringValue { get; set; }
    public int Value { get; set;}

    private Option(string name, string stringValue, int value)
    {
        Name = name;
        StringValue = stringValue;
        Value = value;
    }

    public Option(string name, string stringValue)
        : this(name, stringValue, -1)
    { }

    public Option(string name, int value)
        : this(name, "", value)
    { }
}

public class OptionNameComparer : IComparer<Option>
{
    public int Compare(Option x, Option y)
    {
        return x.Name.CompareTo(y.Name);
    }
}

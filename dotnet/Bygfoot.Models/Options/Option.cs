using System;
using System.Collections.Generic;

namespace Bygfoot.Options
{
	public class Option
	{
		const float OPTION_FLOAT_DIVISOR = (float)100000.0;

		public string Name { get; set; }
		public string Value { get; set; }

		public Option(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}

	public class OptionNameComparer : IComparer<Option>
	{
		public int Compare(Option x, Option y)
		{
			return x.Name.CompareTo(y.Name);
		}
	}
}

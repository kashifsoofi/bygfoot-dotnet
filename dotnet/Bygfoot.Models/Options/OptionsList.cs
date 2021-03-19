using System;
using System.Collections.Generic;

namespace Bygfoot.Options
{
	public class OptionsList
	{
		private List<Option> _lstOptions;
		private Dictionary<string, Option> _dictOptions;

		public OptionsList()
		{
			_lstOptions = new List<Option>();
			_dictOptions = new Dictionary<string, Option>();
		}

		public void Add(string name, string value)
		{
			Add(new Option(name, value));
		}

		public void Add(Option option)
		{
			_lstOptions.Add(option);
			if (!_dictOptions.ContainsKey(option.Name))
				_dictOptions.Add(option.Name, option);
		}

		public void Remove(string name)
		{
			if (_dictOptions.ContainsKey(name))
				_dictOptions.Remove(name);
		}

		public void Remove(Option option)
		{
			Remove(option.Name);
		}

		public Option this[string name]
		{
			get
			{
				if (_dictOptions.ContainsKey(name))
					return _dictOptions[name];
				return null;
			}
		}

		public Option this[int index]
		{
			get { return _lstOptions[index]; }
		}

		//public dynamic this[string name, Type type]
		//{
		//	get
		//	{
		//		if (_dictOptions.ContainsKey(name))
		//			return _dictOptions[name].GetValue(type);
		//		return null;
		//	}
		//}

		public int Count
		{
			get { return _lstOptions.Count; }
		}

		public void Sort()
		{
			var comparer = new OptionNameComparer();
			_lstOptions.Sort(comparer);
		}
	}
}

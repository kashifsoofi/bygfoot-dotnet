using System;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	/** A list of first names and last names
     * from a file. */
	public class NameList
	{
		/** The file id (the part between 'player_names_' and '.xml'). */
		public string sid;
		/** Arrays of strings holding the names. */
		public List<string> firstNames;
		public List<string> lastNames;
	}
}

using System;
namespace Bygfoot.Helpers
{
	public class Language
	{
		/** Set the game language to the specified one. */
		public static void Set(int index)
		{
#if DEBUG
			Console.WriteLine("Language.Set");
#endif
		}

		/** Get the index of the given code in the codes
         * array. */
		public static int GetCodeIndex(string code)
		{
#if DEBUG
			Console.WriteLine("Language.GetCodeIndex");
#endif
			int returnValue = -1;
			/* //TODO 
            string[] codes = Misc.SeparateStrings(Option.ConstStr ("string_language_codes"));

            string localCode = code;
            if (string.Compare (code, "en", true) == 0)
                localCode = "C";

            for (int i = 0; i < codes.Length; i++) {
                if (string.Compare (localCode, codes [i], true) == 0) {
                    returnValue = i;
                    break;
                }
            }
            */
			return returnValue;
		}

		/** Find out which language to use (e.g. for live game commentary).
         * Write the code (en, de etc.) into the buffer. */
		public static string GetCode()
		{
#if DEBUG
			Console.WriteLine("Language.GetCode");
#endif
			string currentLocale = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			string localeOption = "C";//TODO Option.OptStr("string_opt_language_code");
			if (string.Compare(localeOption, "C") == 0)
				return "en";
			else if (string.IsNullOrEmpty(localeOption) && !string.IsNullOrEmpty(currentLocale))
				return currentLocale;
			else
				return localeOption;
		}

		/** Put the country matching the local language to the
         * begining of the array if possible */
		public static void PickCountry(string[] countryFiles)
		{
#if DEBUG
			Console.WriteLine("Language.PickCountry");
#endif
			string[] codes = new string[] { }; //TODO Option.ConstStr ("string_language_codes").Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			string[] defs = new string[] { }; //TODO Option.ConstStr ("string_language_defs").Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			string prefdef = string.Empty;
			string lang = System.Globalization.CultureInfo.CurrentCulture.Name;

			if (!string.IsNullOrEmpty(lang))
			{
				for (int i = 0; i < codes.Length; i++)
				{
					if (((lang.ToLower().StartsWith("en") && codes[i] == "C") ||
						lang.ToLower().StartsWith(codes[i].ToLower())) &&
						defs[i] != "NONE")
					{
						for (int j = 0; j < countryFiles.Length; j++)
						{
							if (countryFiles[j].ToLower().EndsWith(defs[i].ToLower()))
							{
								prefdef = countryFiles[j];
								break;
							}
						}
					}
				}
			}
			//TODO
			//g_ptr_array_sort_with_data(country_files, (GCompareDataFunc)language_compare_country_files, prefdef);
			Array.Sort(countryFiles, delegate (string defA, string defB) {
				int returnValue = 0;
				if (defA.CompareTo(defB) == 0)
					returnValue = 0;
				else if (!string.IsNullOrEmpty(prefdef) && prefdef.CompareTo(defA) == 0)
					returnValue = -1;
				else if (!string.IsNullOrEmpty(prefdef) && prefdef.CompareTo(defB) == 0)
					returnValue = 1;
				else
				{
					int lenmin = Math.Min(defA.Length, defB.Length);
					int i = 0;
					for (i = 0; i < lenmin; i++)
					{
						if (defA[i] != defB[i])
							break;
					}

					if (i == lenmin)
						returnValue = defB.Length.CompareTo(defA.Length);
					else
					{
						char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
							'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
						int j = 0;
						for (j = 0; j < alphabet.Length; j++)
						{
							if (defA[i] == alphabet[j] || defB[i] == alphabet[j])
								break;
						}

						if (j == alphabet.Length)
						{
							//TODO Debug.PrintMessage("Language.CompareCountryFiles: chars {0} and {1} not comparable", defA[i], defB[i]);
							returnValue = 0;
						}
						else
							returnValue = defA[i] == alphabet[j] ? -1 : 1;
					}
				}
				return returnValue;
			});
		}
	}
}

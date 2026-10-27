namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of leagues and cups XML files.
/// Corresponds to the C module src/xml_loadsave_leagues_cups.h and src/xml_loadsave_leagues_cups.c
/// (xml_loadsave_leagues_cups_read, xml_loadsave_leagues_cups_write and the
/// xml_loadsave_leagues_cups_start_element, xml_loadsave_leagues_cups_end_element and
/// xml_loadsave_leagues_cups_text functions).
/// </summary>
public static class LeaguesCupsXmlLoadSave
{
    // Tags. Corresponds to the tag enum of the leagues/cups module in src/xml_loadsave_leagues_cups.c.
    private const int TagLeaguesCups = 12000;
    private const int TagLeagueFile = 12001;
    private const int TagCupFile = 12002;
    private const int TagEnd = 12003;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the leagues/cups module.
    /// </summary>
    public enum XmlLeaguesCupsStates
    {
        TagLeaguesCups = 12000,
        TagLeagueFile = 12001,
        TagCupFile = 12002,
        TagEnd = 12003
    }

    private static XmlLeaguesCupsStates State = XmlLeaguesCupsStates.TagLeaguesCups;
    private static string Dir = "";
    private static List<League> Leagues = new List<League>();
    private static List<Cup> Cups = new List<Cup>();

    /// <summary>
    /// Function reading a leagues and cups xml file and loading the
    /// leagues and cups it refers to.
    /// @param dirname The directory the leagues and cups xml file is located in.
    /// @param prefix The prefix of the leagues and cups xml file (e.g. 'user_0').
    /// </summary>
    /// <param name="dirname">The directory the leagues and cups xml file is located in.</param>
    /// <param name="prefix">The prefix of the leagues and cups xml file.</param>
    public static void LeaguesCupsXmlLoadSaveRead(string dirname, string prefix)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(prefix))
            return;

        State = XmlLeaguesCupsStates.TagLeaguesCups;
        Dir = dirname;
        Leagues.Clear();
        Cups.Clear();

        string filename = Path.Combine(dirname, prefix + "___leagues_cups.xml");
        if (!File.Exists(filename))
            return;

        using (var reader = XmlReader.Create(filename))
        {
            reader.MoveToContent();
            int depth = 0;

            while (true)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.IsEmptyElement)
                        {
                            LeaguesCupsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            LeaguesCupsXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            LeaguesCupsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        LeaguesCupsXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        LeaguesCupsXmlLoadSaveReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void LeaguesCupsXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
    {
        StartElement(elementName, attributeName => reader.GetAttribute(attributeName));
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="attributeNames">The attributes of the opening tag.</param>
    public static void LeaguesCupsXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the leagues and cups XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);
        bool validTag = false;

        switch (tag)
        {
            case TagLeaguesCups:
                State = XmlLeaguesCupsStates.TagLeaguesCups;
                validTag = true;
                break;
            case TagLeagueFile:
                State = XmlLeaguesCupsStates.TagLeagueFile;
                validTag = true;
                break;
            case TagCupFile:
                State = XmlLeaguesCupsStates.TagCupFile;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_leagues_cups_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_leagues_cups_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the leagues/cups module
    /// are the numbers following the underscore in the tag names (e.g. '_12000').
    /// </summary>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The tag.</returns>
    private static int GetTagFromName(string elementName)
    {
        string tagName = elementName.Substring(1);

        return (int)(double.TryParse(tagName, out double tagValue) ? tagValue : 0);
    }

    /// <summary>
    /// Get the value of an attribute from a dictionary of attributes.
    /// </summary>
    /// <param name="attributeNames">The attributes.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The value of the attribute, or null if it is not set.</returns>
    private static string GetAttribute(IDictionary<string, string> attributeNames, string attributeName)
    {
        return attributeNames.TryGetValue(attributeName, out string value) ? value : null;
    }

    /// <summary>
    /// The function called by the reader when a closing tag is read.
    /// The state variable is changed in this function.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void LeaguesCupsXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagLeagueFile || tag == TagCupFile)
        {
            State = XmlLeaguesCupsStates.TagLeaguesCups;
        }
        else if (tag != TagLeaguesCups)
        {
            Debug.DebugPrintMessage($"xml_loadsave_leagues_cups_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for loading the leagues and cups
    /// the file refers to.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void LeaguesCupsXmlLoadSaveReadText(string text)
    {
        if (State == XmlLeaguesCupsStates.TagLeagueFile)
        {
            LeagueXml.XmlLeagueRead(text, Leagues);
        }
        else if (State == XmlLeaguesCupsStates.TagCupFile)
        {
            CupXml.XmlCupRead(text, Cups);
        }
    }

    /// <summary>
    /// Write the leagues and cups into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the leagues and cups xml file (e.g. 'user_0').</param>
    public static void LeaguesCupsXmlLoadSaveWrite(string prefix)
    {
        string filename = prefix + "___leagues_cups.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_12000>");
            foreach (League league in Leagues)
            {
                LeagueXmlLoadSave.LeagueXmlLoadSaveWrite(prefix, league);
                string leagueFileName = Path.GetFileName(prefix) + $"___league_{league.Id}.xml";
                fil.WriteLine($"\t<_{TagLeagueFile}>{leagueFileName}</_{TagLeagueFile}>");
            }
            foreach (Cup cup in Cups)
            {
                // The cup files are written via the cup module, which has not been
                // converted yet.
                string cupFileName = Path.GetFileName(prefix) + $"___cup_{cup.Id}.xml";
                fil.WriteLine($"\t<_{TagCupFile}>{cupFileName}</_{TagCupFile}>");
            }
            fil.WriteLine("</_12000>");
        }
    }
}

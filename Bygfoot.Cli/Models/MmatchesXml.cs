namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the reading of memorable matches XML files.
/// Corresponds to the C module src/xml_mmatches.h and src/xml_mmatches.c
/// (xml_mmatches_read, xml_mmatches_read_start_element, xml_mmatches_read_end_element
/// and xml_mmatches_read_text).
/// </summary>
public static class MmatchesXml
{
    private const int TagMmMatches = 0;
    private const int TagMatch = 1;
    private const int TagMmMatchesCountryName = 2;
    private const int TagMmMatchesCompName = 3;
    private const int TagMmMatchesNeutral = 4;
    private const int TagMmMatchesUserTeam = 5;
    private const int TagMmMatchesLgFile = 6;
    private const int TagEnd = 7;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the memorable matches module.
    /// </summary>
    public enum XmlMmatchesStates
    {
        TagMmMatches = 0,
        TagMatch = 1,
        TagMmMatchesCountryName = 2,
        TagMmMatchesCompName = 3,
        TagMmMatchesNeutral = 4,
        TagMmMatchesUserTeam = 5,
        TagMmMatchesLgFile = 6,
        TagEnd = 7
    }

    private static XmlMmatchesStates State = XmlMmatchesStates.TagMmMatches;
    private static MemMatch NewMatch = new MemMatch();
    private static string Dirname = string.Empty;
    private static List<MemMatch> MmArray = new List<MemMatch>();

    /// <summary>
    /// Function reading a memorable matches file and adding the matches
    /// to the given array.
    /// @param filename Name of the xml file (e.g. 'user_0___mmatches') to be read.
    /// @param mmatches The array we add the matches to.
    /// </summary>
    /// <param name="filename">The memorable matches xml file to read.</param>
    /// <param name="mmatches">The array to add the matches to.</param>
    public static void MmatchesRead(string filename, List<MemMatch> mmatches)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlMmatchesStates.TagMmMatches;
        Dirname = string.IsNullOrEmpty(filename) ? string.Empty : Path.GetDirectoryName(filename);
        MmArray = mmatches;

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
                            MmatchesReadStartElement(reader, reader.LocalName);
                            MmatchesReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            MmatchesReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        MmatchesReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        MmatchesReadText(reader.ReadString());
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
    public static void MmatchesReadStartElement(XmlReader reader, string elementName)
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
    public static void MmatchesReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the memorable matches XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);

        switch (tag)
        {
            case TagMmMatches:
                State = XmlMmatchesStates.TagMmMatches;
                break;
            case TagMatch:
                State = XmlMmatchesStates.TagMatch;
                NewMatch.CountryName = string.Empty;
                break;
            case TagMmMatchesCountryName:
                State = XmlMmatchesStates.TagMmMatchesCountryName;
                break;
            case TagMmMatchesCompName:
                State = XmlMmatchesStates.TagMmMatchesCompName;
                break;
            case TagMmMatchesNeutral:
                State = XmlMmatchesStates.TagMmMatchesNeutral;
                break;
            case TagMmMatchesUserTeam:
                State = XmlMmatchesStates.TagMmMatchesUserTeam;
                break;
            case TagMmMatchesLgFile:
                State = XmlMmatchesStates.TagMmMatchesLgFile;
                break;
            default:
                Debug.DebugPrintMessage($"xml_mmatches_read_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the memorable matches
    /// module are the numbers following the underscore in the tag names (e.g. '_0').
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
    public static void MmatchesReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagMatch)
        {
            State = XmlMmatchesStates.TagMmMatches;
            MmArray.Add(NewMatch);
        }
        else if (tag == TagMmMatchesCompName ||
                 tag == TagMmMatchesCountryName ||
                 tag == TagMmMatchesNeutral ||
                 tag == TagMmMatchesUserTeam ||
                 tag == TagMmMatchesLgFile)
        {
            State = XmlMmatchesStates.TagMatch;
        }
        else if (tag != TagMmMatches)
        {
            Debug.DebugPrintMessage($"xml_mmatches_read_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the memorable match variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void MmatchesReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlMmatchesStates.TagMmMatchesCompName:
                NewMatch.CompetitionName = text;
                break;
            case XmlMmatchesStates.TagMmMatchesCountryName:
                NewMatch.CountryName = text;
                break;
            case XmlMmatchesStates.TagMmMatchesNeutral:
                NewMatch.Neutral = intValue != 0;
                break;
            case XmlMmatchesStates.TagMmMatchesUserTeam:
                NewMatch.UserTeam = intValue;
                break;
            case XmlMmatchesStates.TagMmMatchesLgFile:
                string lgFile = Path.Combine(Dirname, text);
                // The live game gets reset and read from the file. The actual reading
                // is handled by the live game module (LiveGameXml).
                // live_game_reset(&NewMatch.LiveGame, NULL, FALSE);
                // xml_loadsave_live_game_read(lgFile, &NewMatch.LiveGame);
                // NewMatch.LiveGame.Fix = null;
                // NewMatch.LiveGame.FixId = -1;
                break;
        }
    }
}

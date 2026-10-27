namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of misc XML files.
/// Corresponds to the C module src/xml_loadsave_misc.h and src/xml_loadsave_misc.c
/// (xml_loadsave_misc_read, xml_loadsave_misc_write and the
/// xml_loadsave_misc_write_bets functions).
/// </summary>
public static class MiscXmlLoadSave
{
    // Common tags used in the misc xml files. Corresponds to the tag enum of src/xml.h.
    private const int TagName = 0;
    private const int TagSymbol = 1;
    private const int TagSid = 2;

    // Tags. Corresponds to the tag enum of the misc module in src/xml_loadsave_misc.c.
    private const int TagMisc = 1000;
    private const int TagMiscRating = 1001;
    private const int TagMiscSeason = 1002;
    private const int TagMiscWeek = 1003;
    private const int TagMiscWeekRound = 1004;
    private const int TagMiscCounter = 1005;
    private const int TagMiscAllcup = 1006;
    private const int TagMiscBet0 = 1007;
    private const int TagMiscBet1 = 1008;
    private const int TagMiscBetFixId = 1009;
    private const int TagMiscBetOdd = 1010;
    private const int TagMiscVersion = 1011;
    private const int TagMiscCurrentInterest = 1012;
    private const int TagEnd = 1013;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the misc module.
    /// </summary>
    public enum XmlMiscStates
    {
        TagName = 0,
        TagSymbol = 1,
        TagSid = 2,
        TagMisc = 1000,
        TagMiscRating = 1001,
        TagMiscSeason = 1002,
        TagMiscWeek = 1003,
        TagMiscWeekRound = 1004,
        TagMiscCounter = 1005,
        TagMiscAllcup = 1006,
        TagMiscBet0 = 1007,
        TagMiscBet1 = 1008,
        TagMiscBetFixId = 1009,
        TagMiscBetOdd = 1010,
        TagMiscVersion = 1011,
        TagMiscCurrentInterest = 1012,
        TagEnd = 1013
    }

    private static XmlMiscStates State = XmlMiscStates.TagMisc;
    private static BetMatch NewBet = new BetMatch();
    private static int Countidx = 0;
    private static int Oddidx = 0;
    private static int Betidx = 0;
    private static List<int> Acps = new List<int>();
    private static readonly List<BetMatch>[] Bets = new List<BetMatch>[] { new List<BetMatch>(), new List<BetMatch>() };
    private static float CurrentInterest = 0;

    /// <summary>
    /// Function reading a misc xml file.
    /// @param dirname The directory the misc xml file is located in.
    /// @param basename The basename of the misc xml file (e.g. 'user_0').
    /// </summary>
    /// <param name="dirname">The directory the misc xml file is located in.</param>
    /// <param name="basename">The basename of the misc xml file.</param>
    public static void MiscXmlLoadSaveRead(string dirname, string basename)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(basename))
            return;

        State = XmlMiscStates.TagMisc;
        NewBet = new BetMatch();
        Countidx = 0;
        Oddidx = 0;
        Betidx = 0;
        Acps.Clear();
        for (int i = 0; i < 2; i++)
        {
            Bets[i].Clear();
        }

        string filename = Path.Combine(dirname, basename + "___misc.xml");
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
                            MiscXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            MiscXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            MiscXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        MiscXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        MiscXmlLoadSaveReadText(reader.ReadString());
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
    public static void MiscXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void MiscXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the misc XML parser, dispatching
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
            case TagMisc:
                State = XmlMiscStates.TagMisc;
                validTag = true;
                break;
            case TagMiscRating:
                State = XmlMiscStates.TagMiscRating;
                validTag = true;
                break;
            case TagMiscSeason:
                State = XmlMiscStates.TagMiscSeason;
                validTag = true;
                break;
            case TagMiscWeek:
                State = XmlMiscStates.TagMiscWeek;
                validTag = true;
                break;
            case TagMiscWeekRound:
                State = XmlMiscStates.TagMiscWeekRound;
                validTag = true;
                break;
            case TagMiscCounter:
                State = XmlMiscStates.TagMiscCounter;
                validTag = true;
                break;
            case TagMiscAllcup:
                State = XmlMiscStates.TagMiscAllcup;
                validTag = true;
                break;
            case TagMiscBet0:
                State = XmlMiscStates.TagMiscBet0;
                validTag = true;
                break;
            case TagMiscBet1:
                State = XmlMiscStates.TagMiscBet1;
                validTag = true;
                break;
            case TagMiscBetFixId:
                State = XmlMiscStates.TagMiscBetFixId;
                validTag = true;
                break;
            case TagMiscBetOdd:
                State = XmlMiscStates.TagMiscBetOdd;
                validTag = true;
                break;
            case TagMiscVersion:
                State = XmlMiscStates.TagMiscVersion;
                validTag = true;
                break;
            case TagMiscCurrentInterest:
                State = XmlMiscStates.TagMiscCurrentInterest;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_misc_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (tag == TagMisc)
            Countidx = 0;

        if (tag == TagMiscBet0 || tag == TagMiscBet1)
        {
            Oddidx = 0;
            Betidx = (tag == TagMiscBet1) ? 1 : 0;
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_misc_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the misc module
    /// are the numbers following the underscore in the tag names (e.g. '_1000').
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
    public static void MiscXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagName ||
            tag == TagSymbol ||
            tag == TagSid ||
            tag == TagMiscVersion ||
            tag == TagMiscCurrentInterest ||
            tag == TagMiscRating ||
            tag == TagMiscAllcup ||
            tag == TagMiscCounter ||
            tag == TagMiscSeason ||
            tag == TagMiscWeek ||
            tag == TagMiscWeekRound ||
            tag == TagMiscBet0 ||
            tag == TagMiscBet1)
        {
            State = XmlMiscStates.TagMisc;

            if (tag == TagMiscCounter)
                Countidx++;
            else if (tag == TagMiscBet0 || tag == TagMiscBet1)
                Bets[Betidx].Add(NewBet);
        }
        else if (tag == TagMiscBetOdd || tag == TagMiscBetFixId)
        {
            State = (Betidx == 0) ? XmlMiscStates.TagMiscBet0 : XmlMiscStates.TagMiscBet1;

            if (tag == TagMiscBetOdd)
                Oddidx++;
        }
        else if (tag != TagMisc)
        {
            Debug.DebugPrintMessage($"xml_loadsave_misc_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the misc variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void MiscXmlLoadSaveReadText(string text)
    {
        double tagValue = 0;
        int intValue = (int)(double.TryParse(text, out tagValue) ? tagValue : 0);
        float floatValue = (float)(double.TryParse(text, out tagValue) ? tagValue : 0) / 10000;

        switch (State)
        {
            case XmlMiscStates.TagName:
                Variables.Country.Name = text;
                break;
            case XmlMiscStates.TagMiscRating:
                Variables.Country.Rating = intValue;
                break;
            case XmlMiscStates.TagSymbol:
                Variables.Country.Symbol = text;
                break;
            case XmlMiscStates.TagSid:
                Variables.Country.SId = text;
                break;
            case XmlMiscStates.TagMiscSeason:
                Variables.Season = intValue;
                break;
            case XmlMiscStates.TagMiscWeek:
                Variables.Week = intValue;
                break;
            case XmlMiscStates.TagMiscWeekRound:
                Variables.WeekRound = intValue;
                break;
            case XmlMiscStates.TagMiscCounter:
                Variables.Counters[Countidx] = intValue;
                break;
            case XmlMiscStates.TagMiscAllcup:
                Acps.Add(intValue);
                break;
            case XmlMiscStates.TagMiscBetOdd:
                NewBet.Odds[Oddidx] = floatValue;
                break;
            case XmlMiscStates.TagMiscBetFixId:
                NewBet.FixId = intValue;
                break;
            case XmlMiscStates.TagMiscCurrentInterest:
                CurrentInterest = floatValue;
                break;
        }
    }

    /// <summary>
    /// Write the misc data into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the misc xml file (e.g. 'user_0').</param>
    public static void MiscXmlLoadSaveWrite(string prefix)
    {
        string filename = prefix + "___misc.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_1000>");
            fil.WriteLine("<_1011>" + Bygfoot.Version + "</_1011>");
            fil.WriteLine("<_1012>" + (int)(CurrentInterest * 10000) + "</_1012>");
            fil.WriteLine("<_0>" + Variables.Country.Name + "</_0>");
            fil.WriteLine("<_1>" + Variables.Country.Symbol + "</_1>");
            fil.WriteLine("<_2>" + Variables.Country.SId + "</_2>");
            fil.WriteLine("<_1001>" + Variables.Country.Rating + "</_1001>");
            fil.WriteLine("<_1002>" + Variables.Season + "</_1002>");
            fil.WriteLine("<_1003>" + Variables.Week + "</_1003>");
            fil.WriteLine("<_1004>" + Variables.WeekRound + "</_1004>");
            for (int i = 0; i < Variables.Counters.Length; i++)
            {
                fil.WriteLine("<_1005>" + Variables.Counters[i] + "</_1005>");
            }
            for (int i = 0; i < Acps.Count; i++)
            {
                fil.WriteLine("<_1006>" + Acps[i] + "</_1006>");
            }
            MiscXmlLoadSaveWriteBets(fil);
            fil.WriteLine("</_1000>");
        }
    }

    /// <summary>
    /// Write the bets arrays into the file.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    public static void MiscXmlLoadSaveWriteBets(TextWriter fil)
    {
        for (int i = 0; i < 2; i++)
        {
            int betTag = (i == 0) ? TagMiscBet0 : TagMiscBet1;
            fil.WriteLine($"\t<_{betTag}>");
            foreach (BetMatch bet in Bets[i])
            {
                fil.WriteLine($"\t\t<_1009>" + bet.FixId + "</_1009>");
                for (int k = 0; k < 3; k++)
                {
                    fil.WriteLine($"\t\t<_1010>" + (int)(bet.Odds[k] * 10000) + "</_1010>");
                }
            }
            fil.WriteLine($"\t</_{betTag}>");
        }
    }
}

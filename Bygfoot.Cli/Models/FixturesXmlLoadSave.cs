namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of fixture XML files.
/// Corresponds to the C module src/xml_loadsave_fixtures.h and src/xml_loadsave_fixtures.c
/// (xml_loadsave_fixtures_read, xml_loadsave_fixtures_write and the
/// xml_loadsave_fixtures_start_element, xml_loadsave_fixtures_end_element and
/// xml_loadsave_fixtures_text functions).
/// </summary>
public static class FixturesXmlLoadSave
{
    // Common tags used in the fixtures xml files. Corresponds to the tag enum of src/xml.h.
    private const int TagName = 0;
    private const int TagId = 4;
    private const int TagRound = 13;
    private const int TagTeamId = 10;

    // Tags. Corresponds to the tag enum of the fixtures module in src/xml_loadsave_fixtures.c.
    private const int TagFixtures = 5000;
    private const int TagFixture = 5001;
    private const int TagFixtureReplayNumber = 5002;
    private const int TagFixtureId = 5003;
    private const int TagFixtureWeekNumber = 5004;
    private const int TagFixtureWeekRoundNumber = 5005;
    private const int TagFixtureResult = 5006;
    private const int TagFixtureHomeAdvantage = 5007;
    private const int TagFixtureSecondLeg = 5008;
    private const int TagFixtureDecisive = 5009;
    private const int TagFixtureAttendance = 5010;
    private const int TagEnd = 5011;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the fixtures module.
    /// </summary>
    public enum XmlFixturesStates
    {
        TagName = 0,
        TagId = 4,
        TagRound = 13,
        TagTeamId = 10,
        TagFixtures = 5000,
        TagFixture = 5001,
        TagFixtureReplayNumber = 5002,
        TagFixtureId = 5003,
        TagFixtureWeekNumber = 5004,
        TagFixtureWeekRoundNumber = 5005,
        TagFixtureResult = 5006,
        TagFixtureHomeAdvantage = 5007,
        TagFixtureSecondLeg = 5008,
        TagFixtureDecisive = 5009,
        TagFixtureAttendance = 5010,
        TagEnd = 5011
    }

    private static XmlFixturesStates State = XmlFixturesStates.TagFixtures;
    private static Fixture NewFixture = new Fixture();
    private static List<Fixture> Fixtures = new List<Fixture>();
    private static int Residx1 = 0;
    private static int Residx2 = 0;
    private static int Teamidx = 0;

    /// <summary>
    /// Function reading a fixtures xml file and adding the fixtures
    /// to the internal list of fixtures.
    /// @param filename The name of the fixtures xml file to be read.
    /// @param fixtures The list of fixtures to be read into.
    /// </summary>
    /// <param name="filename">The name of the fixtures xml file to read.</param>
    /// <param name="fixtures">The list of fixtures to be read into.</param>
    public static void FixturesXmlLoadSaveRead(string filename, List<Fixture> fixtures)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlFixturesStates.TagFixtures;
        NewFixture = new Fixture();
        Fixtures = fixtures;
        Residx1 = 0;
        Residx2 = 0;
        Teamidx = 0;

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
                            FixturesXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            FixturesXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            FixturesXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        FixturesXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        FixturesXmlLoadSaveReadText(reader.ReadString());
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
    public static void FixturesXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void FixturesXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the fixture XML parser, dispatching
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
            case TagFixtures:
                State = XmlFixturesStates.TagFixtures;
                validTag = true;
                break;
            case TagFixture:
                State = XmlFixturesStates.TagFixture;
                validTag = true;
                break;
            case TagFixtureReplayNumber:
                State = XmlFixturesStates.TagFixtureReplayNumber;
                validTag = true;
                break;
            case TagFixtureId:
                State = XmlFixturesStates.TagFixtureId;
                validTag = true;
                break;
            case TagFixtureWeekNumber:
                State = XmlFixturesStates.TagFixtureWeekNumber;
                validTag = true;
                break;
            case TagFixtureWeekRoundNumber:
                State = XmlFixturesStates.TagFixtureWeekRoundNumber;
                validTag = true;
                break;
            case TagFixtureResult:
                State = XmlFixturesStates.TagFixtureResult;
                validTag = true;
                break;
            case TagFixtureHomeAdvantage:
                State = XmlFixturesStates.TagFixtureHomeAdvantage;
                validTag = true;
                break;
            case TagFixtureSecondLeg:
                State = XmlFixturesStates.TagFixtureSecondLeg;
                validTag = true;
                break;
            case TagFixtureDecisive:
                State = XmlFixturesStates.TagFixtureDecisive;
                validTag = true;
                break;
            case TagFixtureAttendance:
                State = XmlFixturesStates.TagFixtureAttendance;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_fixtures_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (State == XmlFixturesStates.TagFixture)
        {
            Residx1 = 0;
            Residx2 = 0;
            Teamidx = 0;
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_fixtures_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the fixtures module
    /// are the numbers following the underscore in the tag names (e.g. '_5000').
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
    /// The state variable is changed in this function and the
    /// new fixture gets added to the fixtures list.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void FixturesXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagFixture)
        {
            State = XmlFixturesStates.TagFixtures;
            Fixtures.Add(NewFixture);
        }
        else if (tag == TagFixtureReplayNumber ||
                 tag == TagFixtureId ||
                 tag == TagFixtureWeekNumber ||
                 tag == TagFixtureWeekRoundNumber ||
                 tag == TagFixtureResult ||
                 tag == TagFixtureHomeAdvantage ||
                 tag == TagFixtureSecondLeg ||
                 tag == TagFixtureDecisive ||
                 tag == TagFixtureAttendance ||
                 tag == TagTeamId ||
                 tag == TagId ||
                 tag == TagRound)
        {
            State = XmlFixturesStates.TagFixture;

            if (tag == TagFixtureResult)
            {
                Residx2 = (Residx2 + 1) % 3;
                if (Residx2 == 0)
                    Residx1++;
            }
            else if (tag == TagTeamId)
                Teamidx++;
        }
        else if (tag != TagFixtures)
        {
            Debug.DebugPrintMessage($"xml_loadsave_fixtures_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the fixture variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void FixturesXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);

        switch (State)
        {
            case XmlFixturesStates.TagId:
                NewFixture.Clid = intValue;
                break;
            case XmlFixturesStates.TagRound:
                NewFixture.Round = intValue;
                break;
            case XmlFixturesStates.TagFixtureReplayNumber:
                NewFixture.ReplayNumber = intValue;
                break;
            case XmlFixturesStates.TagFixtureId:
                NewFixture.Id = intValue;
                break;
            case XmlFixturesStates.TagFixtureWeekNumber:
                NewFixture.WeekNumber = intValue;
                break;
            case XmlFixturesStates.TagFixtureWeekRoundNumber:
                NewFixture.WeekRoundNumber = intValue;
                break;
            case XmlFixturesStates.TagFixtureHomeAdvantage:
                NewFixture.HomeAdvantage = intValue != 0;
                break;
            case XmlFixturesStates.TagFixtureSecondLeg:
                NewFixture.SecondLeg = intValue != 0;
                break;
            case XmlFixturesStates.TagFixtureDecisive:
                NewFixture.Decisive = intValue != 0;
                break;
            case XmlFixturesStates.TagFixtureAttendance:
                NewFixture.Attendance = intValue;
                break;
            case XmlFixturesStates.TagFixtureResult:
                NewFixture.Result[Residx1, Residx2] = intValue;
                break;
            case XmlFixturesStates.TagTeamId:
                NewFixture.TeamIds[Teamidx] = intValue;
                break;
        }
    }

    /// <summary>
    /// Write the fixtures into the given file.
    /// </summary>
    /// <param name="filename">The fixtures xml file to write.</param>
    /// <param name="fixtures">The fixtures to write.</param>
    public static void FixturesXmlLoadSaveWrite(string filename, List<Fixture> fixtures)
    {
        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_5000>");
            foreach (Fixture fixture in fixtures)
            {
                fil.WriteLine("<_5001>");
                fil.WriteLine("<_4>" + fixture.Clid + "</_4>");
                fil.WriteLine("<_13>" + fixture.Round + "</_13>");
                fil.WriteLine("<_5002>" + fixture.ReplayNumber + "</_5002>");
                fil.WriteLine("<_5003>" + fixture.Id + "</_5003>");
                fil.WriteLine("<_5004>" + fixture.WeekNumber + "</_5004>");
                fil.WriteLine("<_5005>" + fixture.WeekRoundNumber + "</_5005>");
                fil.WriteLine("<_5007>" + (int)(fixture.HomeAdvantage ? 1 : 0) + "</_5007>");
                fil.WriteLine("<_5008>" + (int)(fixture.SecondLeg ? 1 : 0) + "</_5008>");
                fil.WriteLine("<_5009>" + (int)(fixture.Decisive ? 1 : 0) + "</_5009>");
                fil.WriteLine("<_5010>" + fixture.Attendance + "</_5010>");
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        fil.WriteLine("<_5006>" + fixture.Result[j, k] + "</_5006>");
                    }
                    fil.WriteLine("<_10>" + fixture.TeamIds[j] + "</_10>");
                }
                fil.WriteLine("</_5001>");
            }
            fil.WriteLine("</_5000>");
        }
    }
}

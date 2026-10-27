namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of live game XML files.
/// Corresponds to the C module src/xml_loadsave_live_game.h and src/xml_loadsave_live_game.c
/// (xml_loadsave_live_game_read, xml_loadsave_live_game_write and the
/// xml_loadsave_live_game_start_element, xml_loadsave_live_game_end_element and
/// xml_loadsave_live_game_text functions).
/// </summary>
public static class LiveGameXmlLoadSave
{
    // Tags. Corresponds to the tag enum of the live game module in src/xml_loadsave_live_game.c.
    private const int TagLiveGame = 8000;
    private const int TagLiveGameFixId = 8001;
    private const int TagLiveGameTeamName = 8002;
    private const int TagLiveGameAttendance = 8003;
    private const int TagLiveGameUnit = 8004;
    private const int TagLiveGameUnitPossession = 8005;
    private const int TagLiveGameUnitArea = 8006;
    private const int TagLiveGameUnitMinute = 8007;
    private const int TagLiveGameUnitTime = 8008;
    private const int TagLiveGameUnitResult = 8009;
    private const int TagLiveGameUnitEvent = 8010;
    private const int TagLiveGameUnitEventType = 8011;
    private const int TagLiveGameUnitEventTeam = 8012;
    private const int TagLiveGameUnitEventPlayer = 8013;
    private const int TagLiveGameUnitEventPlayer2 = 8014;
    private const int TagLiveGameUnitEventCommentary = 8015;
    private const int TagLiveGameUnitEventVerbosity = 8016;
    private const int TagLiveGameStat = 8017;
    private const int TagLiveGameStatPossession = 8018;
    private const int TagLiveGameStatValues = 8019;
    private const int TagLiveGameStatValue = 8020;
    private const int TagLiveGameStatPlayers = 8021;
    private const int TagLiveGameStatPlayer = 8022;
    private const int TagLiveGameStatPlayerElement = 8023;
    private const int TagEnd = 8024;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the live game module.
    /// </summary>
    public enum XmlLiveGameStates
    {
        TagLiveGame = 8000,
        TagLiveGameFixId = 8001,
        TagLiveGameTeamName = 8002,
        TagLiveGameAttendance = 8003,
        TagLiveGameUnit = 8004,
        TagLiveGameUnitPossession = 8005,
        TagLiveGameUnitArea = 8006,
        TagLiveGameUnitMinute = 8007,
        TagLiveGameUnitTime = 8008,
        TagLiveGameUnitResult = 8009,
        TagLiveGameUnitEvent = 8010,
        TagLiveGameUnitEventType = 8011,
        TagLiveGameUnitEventTeam = 8012,
        TagLiveGameUnitEventPlayer = 8013,
        TagLiveGameUnitEventPlayer2 = 8014,
        TagLiveGameUnitEventCommentary = 8015,
        TagLiveGameUnitEventVerbosity = 8016,
        TagLiveGameStat = 8017,
        TagLiveGameStatPossession = 8018,
        TagLiveGameStatValues = 8019,
        TagLiveGameStatValue = 8020,
        TagLiveGameStatPlayers = 8021,
        TagLiveGameStatPlayer = 8022,
        TagLiveGameStatPlayerElement = 8023,
        TagEnd = 8024
    }

    private static XmlLiveGameStates State = XmlLiveGameStates.TagLiveGame;
    private static LiveGame NewLiveGame = new LiveGame();
    private static LiveGameUnit NewUnit = new LiveGameUnit();
    private static int Unitidx = 0;
    private static int Statvalidx = 0;
    private static int Statvalidx2 = 0;
    private static int Statplidx = 0;
    private static int Statplidx2 = 0;
    private static int TeamNameIdx = 0;

    /// <summary>
    /// Function reading a live game xml file and filling in the given live game.
    /// @param filename The name of the live game xml file to be read.
    /// @param liveGame The live game to be read into.
    /// </summary>
    /// <param name="filename">The name of the live game xml file to read.</param>
    /// <param name="liveGame">The live game to be read into.</param>
    public static void LiveGameXmlLoadSaveRead(string filename, LiveGame liveGame)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlLiveGameStates.TagLiveGame;
        NewLiveGame = liveGame;
        NewUnit = new LiveGameUnit();
        Unitidx = 0;
        Statvalidx = 0;
        Statvalidx2 = 0;
        Statplidx = 0;
        Statplidx2 = 0;
        TeamNameIdx = 0;

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
                            LiveGameXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            LiveGameXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            LiveGameXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        LiveGameXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        LiveGameXmlLoadSaveReadText(reader.ReadString());
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
    public static void LiveGameXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void LiveGameXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the live game XML parser, dispatching
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
            case TagLiveGame:
                State = XmlLiveGameStates.TagLiveGame;
                validTag = true;
                break;
            case TagLiveGameFixId:
                State = XmlLiveGameStates.TagLiveGameFixId;
                validTag = true;
                break;
            case TagLiveGameTeamName:
                State = XmlLiveGameStates.TagLiveGameTeamName;
                validTag = true;
                break;
            case TagLiveGameAttendance:
                State = XmlLiveGameStates.TagLiveGameAttendance;
                validTag = true;
                break;
            case TagLiveGameUnit:
                State = XmlLiveGameStates.TagLiveGameUnit;
                validTag = true;
                break;
            case TagLiveGameUnitPossession:
                State = XmlLiveGameStates.TagLiveGameUnitPossession;
                validTag = true;
                break;
            case TagLiveGameUnitArea:
                State = XmlLiveGameStates.TagLiveGameUnitArea;
                validTag = true;
                break;
            case TagLiveGameUnitMinute:
                State = XmlLiveGameStates.TagLiveGameUnitMinute;
                validTag = true;
                break;
            case TagLiveGameUnitTime:
                State = XmlLiveGameStates.TagLiveGameUnitTime;
                validTag = true;
                break;
            case TagLiveGameUnitResult:
                State = XmlLiveGameStates.TagLiveGameUnitResult;
                validTag = true;
                break;
            case TagLiveGameUnitEvent:
                State = XmlLiveGameStates.TagLiveGameUnitEvent;
                validTag = true;
                break;
            case TagLiveGameUnitEventType:
                State = XmlLiveGameStates.TagLiveGameUnitEventType;
                validTag = true;
                break;
            case TagLiveGameUnitEventTeam:
                State = XmlLiveGameStates.TagLiveGameUnitEventTeam;
                validTag = true;
                break;
            case TagLiveGameUnitEventPlayer:
                State = XmlLiveGameStates.TagLiveGameUnitEventPlayer;
                validTag = true;
                break;
            case TagLiveGameUnitEventPlayer2:
                State = XmlLiveGameStates.TagLiveGameUnitEventPlayer2;
                validTag = true;
                break;
            case TagLiveGameUnitEventCommentary:
                State = XmlLiveGameStates.TagLiveGameUnitEventCommentary;
                validTag = true;
                break;
            case TagLiveGameUnitEventVerbosity:
                State = XmlLiveGameStates.TagLiveGameUnitEventVerbosity;
                validTag = true;
                break;
            case TagLiveGameStat:
                State = XmlLiveGameStates.TagLiveGameStat;
                validTag = true;
                break;
            case TagLiveGameStatPossession:
                State = XmlLiveGameStates.TagLiveGameStatPossession;
                validTag = true;
                break;
            case TagLiveGameStatValues:
                State = XmlLiveGameStates.TagLiveGameStatValues;
                validTag = true;
                break;
            case TagLiveGameStatValue:
                State = XmlLiveGameStates.TagLiveGameStatValue;
                validTag = true;
                break;
            case TagLiveGameStatPlayers:
                State = XmlLiveGameStates.TagLiveGameStatPlayers;
                validTag = true;
                break;
            case TagLiveGameStatPlayer:
                State = XmlLiveGameStates.TagLiveGameStatPlayer;
                validTag = true;
                break;
            case TagLiveGameStatPlayerElement:
                State = XmlLiveGameStates.TagLiveGameStatPlayerElement;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_live_game_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (tag == TagLiveGame)
        {
            Statvalidx = 0;
            Statplidx = 0;
            TeamNameIdx = 0;
        }
        else if (tag == TagLiveGameUnit || tag == TagLiveGameUnitEvent)
        {
            Unitidx = 0;
        }
        else if (tag == TagLiveGameStatValues)
        {
            Statvalidx2 = 0;
        }
        else if (tag == TagLiveGameStatPlayers)
        {
            Statplidx2 = 0;
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_live_game_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the live game module
    /// are the numbers following the underscore in the tag names (e.g. '_8000').
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
    /// new unit gets added to the live game.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void LiveGameXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagLiveGameFixId ||
            tag == TagLiveGameTeamName ||
            tag == TagLiveGameAttendance ||
            tag == TagLiveGameUnit ||
            tag == TagLiveGameStat)
        {
            State = XmlLiveGameStates.TagLiveGame;

            if (tag == TagLiveGameUnit)
                NewLiveGame.Units.Add(NewUnit);
            else if (tag == TagLiveGameTeamName)
                TeamNameIdx++;
        }
        else if (tag == TagLiveGameUnitPossession ||
                 tag == TagLiveGameUnitArea ||
                 tag == TagLiveGameUnitMinute ||
                 tag == TagLiveGameUnitTime ||
                 tag == TagLiveGameUnitResult ||
                 tag == TagLiveGameUnitEvent)
        {
            State = XmlLiveGameStates.TagLiveGameUnit;

            if (tag == TagLiveGameUnitResult)
                Unitidx++;
        }
        else if (tag == TagLiveGameUnitEventType ||
                 tag == TagLiveGameUnitEventTeam ||
                 tag == TagLiveGameUnitEventPlayer ||
                 tag == TagLiveGameUnitEventPlayer2 ||
                 tag == TagLiveGameUnitEventCommentary ||
                 tag == TagLiveGameUnitEventVerbosity)
        {
            State = XmlLiveGameStates.TagLiveGameUnitEvent;
        }
        else if (tag == TagLiveGameStatPossession ||
                 tag == TagLiveGameStatPlayers ||
                 tag == TagLiveGameStatValues)
        {
            State = XmlLiveGameStates.TagLiveGameStat;

            if (tag == TagLiveGameStatValues)
                Statvalidx++;
            else if (tag == TagLiveGameStatPlayers)
                Statplidx++;
        }
        else if (tag == TagLiveGameStatPlayer)
        {
            State = XmlLiveGameStates.TagLiveGameStatPlayers;
            Statplidx2++;
        }
        else if (tag == TagLiveGameStatValue)
        {
            State = XmlLiveGameStates.TagLiveGameStatValues;
            Statvalidx2++;
        }
        else if (tag == TagLiveGameStatPlayerElement)
        {
            State = XmlLiveGameStates.TagLiveGameStatPlayer;
        }
        else if (tag != TagLiveGame)
        {
            Debug.DebugPrintMessage($"xml_loadsave_live_game_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the live game variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void LiveGameXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);
        float floatValue = (float)(double.TryParse(text, out double tagValue2) ? tagValue2 / 10000 : 0);

        switch (State)
        {
            case XmlLiveGameStates.TagLiveGameFixId:
                NewLiveGame.FixId = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameTeamName:
                NewLiveGame.TeamNames[TeamNameIdx] = text;
                break;
            case XmlLiveGameStates.TagLiveGameAttendance:
                NewLiveGame.Attendance = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitPossession:
                NewUnit.Possession = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitArea:
                NewUnit.Area = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitMinute:
                NewUnit.Minute = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitTime:
                NewUnit.Time = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitResult:
                NewUnit.Result[Unitidx] = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitEventType:
                NewUnit.Event.Type = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitEventTeam:
                NewUnit.Event.Team = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitEventPlayer:
                NewUnit.Event.Player = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitEventPlayer2:
                NewUnit.Event.Player2 = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameUnitEventCommentary:
                NewUnit.Event.Commentary = text;
                break;
            case XmlLiveGameStates.TagLiveGameUnitEventVerbosity:
                NewUnit.Event.Verbosity = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameStatPossession:
                NewLiveGame.Stats.Possession = floatValue;
                break;
            case XmlLiveGameStates.TagLiveGameStatValue:
                NewLiveGame.Stats.Values[Statvalidx, Statvalidx2] = intValue;
                break;
            case XmlLiveGameStates.TagLiveGameStatPlayerElement:
                NewLiveGame.Stats.Players[Statplidx, Statplidx2].Add(text);
                break;
        }
    }

    /// <summary>
    /// Write the live game into the given file.
    /// </summary>
    /// <param name="filename">The live game xml file to write.</param>
    /// <param name="liveGame">The live game to write.</param>
    public static void LiveGameXmlLoadSaveWrite(string filename, LiveGame liveGame)
    {
        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_8000>");
            if (liveGame.Fix != null)
                fil.WriteLine("<_8001>" + liveGame.Fix.Id + "</_8001>");
            for (int i = 0; i < 2; i++)
            {
                if (liveGame.TeamNames[i] != null)
                    fil.WriteLine("<_8002>" + liveGame.TeamNames[i] + "</_8002>");
            }
            fil.WriteLine("<_8003>" + liveGame.Attendance + "</_8003>");
            foreach (LiveGameUnit unit in liveGame.Units)
            {
                LiveGameXmlLoadSaveWriteUnit(fil, unit);
            }
            LiveGameXmlLoadSaveWriteStats(fil, liveGame.Stats);
            fil.WriteLine("</_8000>");
        }
    }

    /// <summary>
    /// Write a single unit of the live game into the given file.
    /// </summary>
    /// <param name="fil">The file to write to.</param>
    /// <param name="unit">The unit to write.</param>
    public static void LiveGameXmlLoadSaveWriteUnit(TextWriter fil, LiveGameUnit unit)
    {
        fil.WriteLine("\t<_8004>");
        fil.WriteLine("\t<_8005>" + unit.Possession + "</_8005>");
        fil.WriteLine("\t<_8006>" + unit.Area + "</_8006>");
        fil.WriteLine("\t<_8007>" + unit.Minute + "</_8007>");
        fil.WriteLine("\t<_8008>" + unit.Time + "</_8008>");
        fil.WriteLine("\t<_8009>" + unit.Result[0] + "</_8009>");
        fil.WriteLine("\t<_8009>" + unit.Result[1] + "</_8009>");

        fil.WriteLine("\t<_8010>");
        fil.WriteLine("\t\t<_8011>" + unit.Event.Type + "</_8011>");
        fil.WriteLine("\t\t<_8016>" + unit.Event.Verbosity + "</_8016>");
        if (unit.Event.Commentary != null)
            fil.WriteLine("\t\t<_8015>" + unit.Event.Commentary + "</_8015>");
        fil.WriteLine("\t\t<_8012>" + unit.Event.Team + "</_8012>");
        fil.WriteLine("\t\t<_8013>" + unit.Event.Player + "</_8013>");
        fil.WriteLine("\t\t<_8014>" + unit.Event.Player2 + "</_8014>");
        fil.WriteLine("\t</_8010>");
        fil.WriteLine("\t</_8004>");
    }

    /// <summary>
    /// Write the stats of a live game into the given file.
    /// </summary>
    /// <param name="fil">The file to write to.</param>
    /// <param name="stats">The stats to write.</param>
    public static void LiveGameXmlLoadSaveWriteStats(TextWriter fil, LiveGameStats stats)
    {
        fil.WriteLine("<_8017>");
        fil.WriteLine("<_8018>" + (int)Math.Round(stats.Possession * 10000) + "</_8018>");
        for (int i = 0; i < 2; i++)
        {
            fil.WriteLine("\t<_8019>");
            for (int j = 0; j < 9; j++)
            {
                fil.WriteLine("\t\t<_8020>" + stats.Values[i, j] + "</_8020>");
            }
            fil.WriteLine("\t</_8019>");
        }
        for (int i = 0; i < 2; i++)
        {
            fil.WriteLine("\t<_8021>");
            for (int j = 0; j < 5; j++)
            {
                fil.WriteLine("\t<_8022>");
                foreach (object player in stats.Players[i, j])
                {
                    fil.WriteLine("\t\t<_8023>" + player + "</_8023>");
                }
                fil.WriteLine("\t</_8022>");
            }
            fil.WriteLine("\t</_8021>");
        }
        fil.WriteLine("</_8017>");
    }
}

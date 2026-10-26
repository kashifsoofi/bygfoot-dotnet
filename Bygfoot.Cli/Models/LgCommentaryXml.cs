namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of LG commentary XML files.
/// Corresponds to the C module src/xml_lg_commentary.h and src/xml_lg_commentary.c
/// (xml_lg_commentary_read, xml_lg_commentary_read_start_element, xml_lg_commentary_read_end_element
/// and xml_lg_commentary_read_text, xml_lg_commentary_event_name_to_int).
/// </summary>
public static class LgCommentaryXml
{
    private const string TagLgCommentary = "lg_commentary";
    private const string TagEvent = "event";
    private const string TagEventName = "name";
    private const string TagEventCommentary = "commentary";
    private const string AttNameCondition = "cond";
    private const string AttNamePriority = "pri";

    private const string EventNameGeneral = "general";
    private const string EventNameStartMatch = "start_match";
    private const string EventNameHalfTime = "half_time";
    private const string EventNameExtraTime = "extra_time";
    private const string EventNameEndMatch = "end_match";
    private const string EventNameLostPossession = "lost_possession";
    private const string EventNameScoringChance = "scoring_chance";
    private const string EventNameHeader = "header";
    private const string EventNamePenalty = "penalty";
    private const string EventNameFreeKick = "free_kick";
    private const string EventNameGoal = "goal";
    private const string EventNameOwnGoal = "own_goal";
    private const string EventNamePost = "post";
    private const string EventNameMiss = "miss";
    private const string EventNameCornerKick = "corner";
    private const string EventNameKeeperPushedInCorner = "keeper_pushed_in_corner";
    private const string EventNamePlayerPushedInCorner = "player_pushed_in_corner";
    private const string EventNameSave = "save";
    private const string EventNameCrossBar = "cross_bar";
    private const string EventNameFoul = "foul";
    private const string EventNameFoulYellow = "foul_yellow";
    private const string EventNameFoulRed = "foul_red";
    private const string EventNameFoulRedInjury = "foul_red_injury";
    private const string EventNameSendOff = "send_off";
    private const string EventNameInjury = "injury";
    private const string EventNameTempInjury = "temp_injury";
    private const string EventNamePenalties = "penalties";
    private const string EventNameStadiumBreakdown = "stadium_breakdown";
    private const string EventNameStadiumRiots = "stadium_riots";
    private const string EventNameStadiumFire = "stadium_fire";
    private const string EventNameSubstitution = "substitution";
    private const string EventNameStructureChange = "structure_change";
    private const string EventNameStyleChange = "style_change";
    private const string EventNameBoostChange = "boost_change";

    /// <summary>
    /// Enum with the states used in the XML parser functions.
    /// Corresponds to the C enum XmlLgCommentaryStates.
    /// </summary>
    public enum XmlLgCommentaryStates
    {
        StateLgCommentary = 0,
        StateEvent,
        StateEventName,
        StateEventCommentary,
        StateEnd
    }

    private static XmlLgCommentaryStates State = XmlLgCommentaryStates.StateLgCommentary;
    private static int CommentaryIdx = -1;
    private static string Condition = null;
    private static int Priority = 1;

    /// <summary>
    /// Function reading an XML file specifying the LG commentary.
    /// Commentaries get appended to the lg commentary array of the event.
    /// @param commentaryFile name of the xml file (e.g. 'lg_commentary_en.xml')
    /// to be read. Full path is not necessary, if the file is located in
    /// one of the support directories; neither are the prefix 'lg_commentary_'
    /// or the suffix '.xml'.
    /// @param lgCommentary The array we append the commentaries to.
    /// </summary>
    /// <param name="commentaryFile">Name of the LG commentary xml file.</param>
    /// <param name="lgCommentary">The array to append the commentaries to.</param>
    public static void XmlLgCommentaryRead(string commentaryFile, IList<LgCommentary>[] lgCommentary)
    {
        using (var reader = XmlReader.Create(commentaryFile))
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
                            XmlLgCommentaryReadStartElement(reader, reader.LocalName);
                            XmlLgCommentaryReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlLgCommentaryReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlLgCommentaryReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlLgCommentaryReadText(reader.ReadString(), lgCommentary);
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
    public static void XmlLgCommentaryReadStartElement(XmlReader reader, string elementName)
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
    public static void XmlLgCommentaryReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the LG commentary XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagEvent:
                State = XmlLgCommentaryStates.StateEvent;
                break;
            case TagEventName:
                State = XmlLgCommentaryStates.StateEventName;
                break;
            case TagEventCommentary:
                State = XmlLgCommentaryStates.StateEventCommentary;
                Condition = getAttribute(AttNameCondition);
                string priorityAttributeValue = getAttribute(AttNamePriority);
                Priority = priorityAttributeValue != null && int.TryParse(priorityAttributeValue, out int priorityValue)
                    ? priorityValue
                    : 1;
                break;
            case TagLgCommentary:
                break;
            default:
                Debug.DebugPrintMessage($"xml_lg_commentary_read_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
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
    public static void XmlLgCommentaryReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagEvent:
                State = XmlLgCommentaryStates.StateLgCommentary;
                break;
            case TagEventName:
            case TagEventCommentary:
                State = XmlLgCommentaryStates.StateEvent;
                break;
            case TagLgCommentary:
                break;
            default:
                Debug.DebugPrintMessage($"xml_lg_commentary_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the variables (e.g. team names)
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    /// <param name="lgCommentary">The array to append the commentaries to.</param>
    public static void XmlLgCommentaryReadText(string text, IList<LgCommentary>[] lgCommentary)
    {
        if (State == XmlLgCommentaryStates.StateEventName)
            CommentaryIdx = XmlLgCommentaryEventNameToInt(text);
        else if (State == XmlLgCommentaryStates.StateEventCommentary && CommentaryIdx != -1)
        {
            lgCommentary[CommentaryIdx].Add(new LgCommentary
            {
                Text = text,
                Condition = Condition,
                Priority = Math.Max(1, Priority),
                Id = Bygfoot.LgCommentaryIdNew()
            });
        }
    }

    /// <summary>
    /// Return the appropriate enum integer going with the event string.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <returns>The event type, or -1 if the event name is unknown.</returns>
    public static int XmlLgCommentaryEventNameToInt(string eventName)
    {
        return eventName switch
        {
            EventNameGeneral => (int)LiveGameEventType.General,
            EventNameStartMatch => (int)LiveGameEventType.StartMatch,
            EventNameHalfTime => (int)LiveGameEventType.HalfTime,
            EventNameExtraTime => (int)LiveGameEventType.ExtraTime,
            EventNameEndMatch => (int)LiveGameEventType.EndMatch,
            EventNameLostPossession => (int)LiveGameEventType.LostPossession,
            EventNameScoringChance => (int)LiveGameEventType.ScoringChance,
            EventNameHeader => (int)LiveGameEventType.Header,
            EventNamePenalty => (int)LiveGameEventType.Penalty,
            EventNameFreeKick => (int)LiveGameEventType.FreeKick,
            EventNameGoal => (int)LiveGameEventType.Goal,
            EventNameOwnGoal => (int)LiveGameEventType.OwnGoal,
            EventNamePost => (int)LiveGameEventType.Post,
            EventNameMiss => (int)LiveGameEventType.Miss,
            EventNameCornerKick => (int)LiveGameEventType.CornerKick,
            EventNameKeeperPushedInCorner => (int)LiveGameEventType.KeeperPushedInCorner,
            EventNamePlayerPushedInCorner => (int)LiveGameEventType.PlayerPushedInCorner,
            EventNameSave => (int)LiveGameEventType.Save,
            EventNameCrossBar => (int)LiveGameEventType.CrossBar,
            EventNameFoul => (int)LiveGameEventType.Foul,
            EventNameFoulYellow => (int)LiveGameEventType.FoulYellow,
            EventNameFoulRed => (int)LiveGameEventType.FoulRed,
            EventNameFoulRedInjury => (int)LiveGameEventType.FoulRedInjury,
            EventNameSendOff => (int)LiveGameEventType.SendOff,
            EventNameInjury => (int)LiveGameEventType.Injury,
            EventNameTempInjury => (int)LiveGameEventType.TempInjury,
            EventNamePenalties => (int)LiveGameEventType.Penalties,
            EventNameStadiumBreakdown => (int)LiveGameEventType.StadiumBreakdown,
            EventNameStadiumRiots => (int)LiveGameEventType.StadiumRiots,
            EventNameStadiumFire => (int)LiveGameEventType.StadiumFire,
            EventNameSubstitution => (int)LiveGameEventType.Substitution,
            EventNameStructureChange => (int)LiveGameEventType.StructureChange,
            EventNameStyleChange => (int)LiveGameEventType.StyleChangeAllOutDefend,
            EventNameBoostChange => (int)LiveGameEventType.BoostChangeAnti,
            _ => -1
        };
    }

    /// <summary>
    /// Process an XML element and its children, starting with the
    /// XmlLgCommentaryReadStartElement function, reading the text in between and
    /// finishing with the XmlLgCommentaryReadEndElement function.
    /// </summary>
    /// <param name="element">The XML element to process.</param>
    private static void ProcessLgCommentaryElement(XmlNode element, IList<LgCommentary>[] lgCommentary)
    {
        XmlLgCommentaryReadStartElement(element.LocalName, GetAttributesFromNode(element));

        foreach (XmlNode child in element.ChildNodes)
        {
            if (child.NodeType == XmlNodeType.Element)
                ProcessLgCommentaryElement(child, lgCommentary);
            else if (child.NodeType == XmlNodeType.Text)
                XmlLgCommentaryReadText(child.InnerText, lgCommentary);
        }

        XmlLgCommentaryReadEndElement(element.LocalName);
    }

    /// <summary>
    /// Get the attributes of an XML element as a dictionary.
    /// </summary>
    /// <param name="element">The XML element.</param>
    /// <returns>The attributes of the element.</returns>
    private static IDictionary<string, string> GetAttributesFromNode(XmlNode element)
    {
        var attributes = new Dictionary<string, string>();
        if (element.Attributes != null)
        {
            foreach (XmlAttribute attribute in element.Attributes)
                attributes[attribute.Name] = attribute.Value;
        }
        return attributes;
    }
}

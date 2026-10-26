namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the reading of strategy XML files.
/// Corresponds to the C module src/xml_strategy.h and src/xml_strategy.c
/// (xml_strategy_read, xml_strategy_read_start_element, xml_strategy_read_end_element
/// and xml_strategy_read_text, xml_strategy_load_strategies).
/// </summary>
public static class StrategyXml
{
    private const string TagStrategy = "strategy";
    private const string TagStrategySid = "sid";
    private const string TagStrategyDesc = "desc";
    private const string TagStrategyPriority = "priority";
    private const string TagStrategyPrematch = "prematch";
    private const string TagStrategyPrematchFormation = "prematch_formation";
    private const string TagStrategyPrematchLineup = "prematch_lineup";
    private const string TagStrategyPrematchBoost = "prematch_boost";
    private const string TagStrategyPrematchStyle = "prematch_style";
    private const string TagStrategyMatchAction = "match_action";
    private const string TagStrategyMatchActionBoost = "match_action_boost";
    private const string TagStrategyMatchActionStyle = "match_action_style";
    private const string TagStrategyMatchActionSub = "match_action_sub";
    private const string TagStrategyMatchActionSubInPos = "match_action_sub_in_pos";
    private const string TagStrategyMatchActionSubOutPos = "match_action_sub_out_pos";

    private const string AttNameMinFitness = "min_fitness";
    private const string AttNameCond = "cond";
    private const string AttNameSubProperty = "prop";

    private const string LineupNameBest = "best";
    private const string LineupNameWeakest = "weakest";
    private const string LineupNameFittest = "fittest";
    private const string LineupNameUnfittest = "unfittest";

    private const string BoostNameOn = "on";
    private const string BoostNameOff = "off";
    private const string BoostNameAnti = "anti";

    private const string StyleNameAllOutDefend = "all-out-defend";
    private const string StyleNameDefend = "defend";
    private const string StyleNameBalanced = "balanced";
    private const string StyleNameAttack = "attack";
    private const string StyleNameAllOutAttack = "all-out-attack";

    private const string PosNameGoalie = "goalie";
    private const string PosNameDefender = "defender";
    private const string PosNameMidfielder = "midfielder";
    private const string PosNameForward = "forward";

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the C enum of the strategy module.
    /// </summary>
    public enum XmlStrategyStates
    {
        StateStrategy = 0,
        StateStrategySid,
        StateStrategyDesc,
        StateStrategyPriority,
        StateStrategyPrematch,
        StateStrategyPrematchFormation,
        StateStrategyPrematchLineup,
        StateStrategyPrematchBoost,
        StateStrategyPrematchStyle,
        StateStrategyMatchAction,
        StateStrategyMatchActionBoost,
        StateStrategyMatchActionStyle,
        StateStrategyMatchActionSub,
        StateStrategyMatchActionSubInPos,
        StateStrategyMatchActionSubOutPos
    }

    private static XmlStrategyStates State = XmlStrategyStates.StateStrategy;
    private static int ActionId = 0;
    private static Strategy CurrentStrategy = new Strategy();
    private static StrategyPrematch CurrentPrematch = new StrategyPrematch();
    private static StrategyMatchAction CurrentMatchAction = new StrategyMatchAction();

    /// <summary>
    /// Function reading a strategy file and adding the strategy
    /// to the strategies array.
    /// @param filename Name of the xml file (e.g. 'strategy_defenders.xml') to be read.
    /// </summary>
    /// <param name="filename">The strategy xml file to read.</param>
    public static void StrategyXmlRead(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlStrategyStates.StateStrategy;

        Strategy newStrategy = new Strategy();
        newStrategy.Sid = string.Empty;
        newStrategy.Desc = string.Empty;
        newStrategy.Priority = 1;
        newStrategy.Prematch = new List<StrategyPrematch>();
        newStrategy.MatchAction = new List<StrategyMatchAction>();
        Strategy.AllStrategies.Add(newStrategy);
        CurrentStrategy = newStrategy;

        ActionId = 0;

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
                            StrategyXmlReadStartElement(reader, reader.LocalName);
                            StrategyXmlReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            StrategyXmlReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        StrategyXmlReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        StrategyXmlReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }

        // Check that all match actions have conditions.
        for (int i = 0; i < CurrentStrategy.MatchAction.Count; i++)
        {
            if (string.IsNullOrEmpty(CurrentStrategy.MatchAction[i].Condition))
            {
                // main_exit_program(EXIT_STRATEGY_ERROR,
                //   "xml_strategy_read: unconditional match action encountered in file %s. match actions MUST have conditions.", filename);
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
    public static void StrategyXmlReadStartElement(XmlReader reader, string elementName)
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
    public static void StrategyXmlReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the strategy XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagStrategy:
                State = XmlStrategyStates.StateStrategy;
                break;
            case TagStrategySid:
                State = XmlStrategyStates.StateStrategySid;
                break;
            case TagStrategyDesc:
                State = XmlStrategyStates.StateStrategyDesc;
                break;
            case TagStrategyPriority:
                State = XmlStrategyStates.StateStrategyPriority;
                break;
            case TagStrategyPrematch:
            {
                StrategyPrematch newPrematch = new StrategyPrematch();
                State = XmlStrategyStates.StateStrategyPrematch;
                newPrematch.Condition = string.Empty;
                newPrematch.Lineup = -100;
                newPrematch.Boost = -100;
                newPrematch.Style = -100;
                newPrematch.MinFitness = 0;
                newPrematch.Formations = new List<string>();
                string condition = getAttribute(AttNameCond);
                if (newPrematch.Condition == null)
                    newPrematch.Condition = condition;
                CurrentStrategy.Prematch.Add(newPrematch);
                CurrentPrematch = newPrematch;
                break;
            }
            case TagStrategyPrematchFormation:
                State = XmlStrategyStates.StateStrategyPrematchFormation;
                break;
            case TagStrategyPrematchLineup:
            {
                State = XmlStrategyStates.StateStrategyPrematchLineup;
                string minFitness = getAttribute(AttNameMinFitness);
                if (minFitness != null && float.TryParse(minFitness, out float minFitnessValue))
                    CurrentPrematch.MinFitness = minFitnessValue / 100f;
                break;
            }
            case TagStrategyPrematchBoost:
                State = XmlStrategyStates.StateStrategyPrematchBoost;
                break;
            case TagStrategyPrematchStyle:
                State = XmlStrategyStates.StateStrategyPrematchStyle;
                break;
            case TagStrategyMatchAction:
            {
                StrategyMatchAction newMatchAction = new StrategyMatchAction();
                State = XmlStrategyStates.StateStrategyMatchAction;
                newMatchAction.Condition = string.Empty;
                newMatchAction.SubCondition = string.Empty;
                newMatchAction.Boost = -100;
                newMatchAction.Style = -100;
                newMatchAction.SubInPos = -1;
                newMatchAction.SubInProp = -1;
                newMatchAction.SubOutPos = -1;
                newMatchAction.SubOutProp = -1;
                newMatchAction.Id = ActionId++;
                string condition = getAttribute(AttNameCond);
                if (newMatchAction.Condition == null)
                    newMatchAction.Condition = condition;
                CurrentStrategy.MatchAction.Add(newMatchAction);
                CurrentMatchAction = newMatchAction;
                break;
            }
            case TagStrategyMatchActionBoost:
                State = XmlStrategyStates.StateStrategyMatchActionBoost;
                break;
            case TagStrategyMatchActionStyle:
                State = XmlStrategyStates.StateStrategyMatchActionStyle;
                break;
            case TagStrategyMatchActionSub:
                State = XmlStrategyStates.StateStrategyMatchActionSub;
                string subCondition = getAttribute(AttNameCond);
                if (CurrentMatchAction.SubCondition == null)
                    CurrentMatchAction.SubCondition = subCondition;
                break;
            case TagStrategyMatchActionSubInPos:
                State = XmlStrategyStates.StateStrategyMatchActionSubInPos;
                string subInProp = getAttribute(AttNameSubProperty);
                if (subInProp == LineupNameBest)
                    CurrentMatchAction.SubInProp = (int)StratLineupType.Best;
                else if (subInProp == LineupNameWeakest)
                    CurrentMatchAction.SubInProp = (int)StratLineupType.Weakest;
                else if (subInProp == LineupNameFittest)
                    CurrentMatchAction.SubInProp = (int)StratLineupType.Fittest;
                else if (subInProp == LineupNameUnfittest)
                    CurrentMatchAction.SubInProp = (int)StratLineupType.Unfittest;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_start_element: unknown property value {subInProp}");
                break;
            case TagStrategyMatchActionSubOutPos:
                State = XmlStrategyStates.StateStrategyMatchActionSubOutPos;
                string subOutProp = getAttribute(AttNameSubProperty);
                if (subOutProp == LineupNameBest)
                    CurrentMatchAction.SubOutProp = (int)StratLineupType.Best;
                else if (subOutProp == LineupNameWeakest)
                    CurrentMatchAction.SubOutProp = (int)StratLineupType.Weakest;
                else if (subOutProp == LineupNameFittest)
                    CurrentMatchAction.SubOutProp = (int)StratLineupType.Fittest;
                else if (subOutProp == LineupNameUnfittest)
                    CurrentMatchAction.SubOutProp = (int)StratLineupType.Unfittest;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_start_element: unknown property value {subOutProp}");
                break;
            default:
                Debug.DebugPrintMessage($"xml_strategy_read_start_element: unknown tag: {elementName}; I'm in state {State}");
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
    public static void StrategyXmlReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagStrategySid:
            case TagStrategyDesc:
            case TagStrategyPriority:
            case TagStrategyMatchAction:
            case TagStrategyPrematch:
                State = XmlStrategyStates.StateStrategy;
                break;
            case TagStrategyPrematchFormation:
            case TagStrategyPrematchLineup:
            case TagStrategyPrematchBoost:
            case TagStrategyPrematchStyle:
                State = XmlStrategyStates.StateStrategyPrematch;
                break;
            case TagStrategyMatchActionSub:
            case TagStrategyMatchActionStyle:
            case TagStrategyMatchActionBoost:
                State = XmlStrategyStates.StateStrategyMatchAction;
                break;
            case TagStrategyMatchActionSubInPos:
            case TagStrategyMatchActionSubOutPos:
                State = XmlStrategyStates.StateStrategyMatchActionSub;
                break;
            case TagStrategy:
                break;
            default:
                Debug.DebugPrintMessage($"xml_strategy_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the strategy variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void StrategyXmlReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlStrategyStates.StateStrategySid:
                CurrentStrategy.Sid = text;
                break;
            case XmlStrategyStates.StateStrategyDesc:
                CurrentStrategy.Desc = text;
                break;
            case XmlStrategyStates.StateStrategyPriority:
                if (Strategy.AllStrategies.Count == 1)
                    CurrentStrategy.Priority = intValue;
                else
                    CurrentStrategy.Priority = Strategy.AllStrategies[Strategy.AllStrategies.Count - 2].Priority + intValue;
                break;
            case XmlStrategyStates.StateStrategyPrematchFormation:
                CurrentPrematch.Formations.Add(text);
                break;
            case XmlStrategyStates.StateStrategyPrematchLineup:
                if (text == LineupNameBest)
                    CurrentPrematch.Lineup = (int)StratLineupType.Best;
                else if (text == LineupNameWeakest)
                    CurrentPrematch.Lineup = (int)StratLineupType.Weakest;
                else if (text == LineupNameFittest)
                    CurrentPrematch.Lineup = (int)StratLineupType.Fittest;
                else if (text == LineupNameUnfittest)
                    CurrentPrematch.Lineup = (int)StratLineupType.Unfittest;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_text: unknown lineup type {text}");
                break;
            case XmlStrategyStates.StateStrategyPrematchBoost:
                if (text == BoostNameOn)
                    CurrentPrematch.Boost = 1;
                else if (text == BoostNameOff)
                    CurrentPrematch.Boost = 0;
                else if (text == BoostNameAnti)
                    CurrentPrematch.Boost = -1;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_text: unknown boost type {text}");
                break;
            case XmlStrategyStates.StateStrategyPrematchStyle:
                if (text == StyleNameAllOutDefend)
                    CurrentPrematch.Style = -2;
                else if (text == StyleNameDefend)
                    CurrentPrematch.Style = -1;
                else if (text == StyleNameBalanced)
                    CurrentPrematch.Style = 0;
                else if (text == StyleNameAttack)
                    CurrentPrematch.Style = 1;
                else if (text == StyleNameAllOutAttack)
                    CurrentPrematch.Style = 2;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_text: unknown style type {text}");
                break;
            case XmlStrategyStates.StateStrategyMatchActionStyle:
                if (text == StyleNameAllOutDefend)
                    CurrentMatchAction.Style = -2;
                else if (text == StyleNameDefend)
                    CurrentMatchAction.Style = -1;
                else if (text == StyleNameBalanced)
                    CurrentMatchAction.Style = 0;
                else if (text == StyleNameAttack)
                    CurrentMatchAction.Style = 1;
                else if (text == StyleNameAllOutAttack)
                    CurrentMatchAction.Style = 2;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_text: unknown style type {text}");
                break;
            case XmlStrategyStates.StateStrategyMatchActionBoost:
                if (text == BoostNameOn)
                    CurrentMatchAction.Boost = 1;
                else if (text == BoostNameOff)
                    CurrentMatchAction.Boost = 0;
                else if (text == BoostNameAnti)
                    CurrentMatchAction.Boost = -1;
                else
                    Debug.DebugPrintMessage($"xml_strategy_read_text: unknown boost type {text}");
                break;
            case XmlStrategyStates.StateStrategyMatchActionSubInPos:
            case XmlStrategyStates.StateStrategyMatchActionSubOutPos:
            {
                string[] positions = text.Split(',');
                int pos = 9;
                if (State == XmlStrategyStates.StateStrategyMatchActionSubInPos)
                    CurrentMatchAction.SubInPos = pos;
                else
                    CurrentMatchAction.SubOutPos = pos;

                foreach (string position in positions)
                {
                    if (position == PosNameGoalie)
                        pos *= 10;
                    else if (position == PosNameDefender)
                        pos = (pos * 10) + 1;
                    else if (position == PosNameMidfielder)
                        pos = (pos * 10) + 2;
                    else if (position == PosNameForward)
                        pos = (pos * 10) + 3;
                    else
                        Debug.DebugPrintMessage($"xml_strategy_read_text: unknown position {position}");
                }

                if (pos < 100)
                    pos = pos % 10;

                if (State == XmlStrategyStates.StateStrategyMatchActionSubInPos)
                    CurrentMatchAction.SubInPos = pos;
                else
                    CurrentMatchAction.SubOutPos = pos;
                break;
            }
        }
    }

    /// <summary>
    /// Load all strategy files found in the appropriate folder.
    /// </summary>
    public static void StrategyLoadStrategies()
    {
        string strategyDir = string.Empty;
        foreach (string directory in Variables.SupportDirectories)
        {
            if (directory.StartsWith("strategy", StringComparison.OrdinalIgnoreCase))
            {
                strategyDir = directory;
                break;
            }
        }

        if (string.IsNullOrEmpty(strategyDir))
        {
            Debug.DebugPrintMessage("xml_strategy_load_strategies: strategy directory not found.");
            return;
        }

        List<string> files = FileHelper.GetDirectoryContents(strategyDir, "strategy_", ".xml");

        if (files.Count == 0)
        {
            Debug.DebugPrintMessage("xml_strategy_load_strategies: no CPU strategies found.");
            return;
        }

        foreach (string file in files)
        {
            StrategyXmlRead(file);
        }

        // Fill secondary prematches with default values.
        for (int i = 0; i < Strategy.AllStrategies.Count; i++)
        {
            Strategy strategy = Strategy.AllStrategies[i];
            StrategyPrematch prem0 = strategy.Prematch.Count > 0 ? strategy.Prematch[0] : new StrategyPrematch();
            for (int j = 1; j < strategy.Prematch.Count; j++)
            {
                StrategyPrematch premcur = strategy.Prematch[j];
                if (premcur.Formations.Count == 0 && prem0.Formations.Count > 0)
                {
                    foreach (string formation in prem0.Formations)
                        premcur.Formations.Add(formation);
                }

                if (premcur.Boost == -100)
                    premcur.Boost = prem0.Boost;
                if (premcur.Style == -100)
                    premcur.Style = prem0.Style;
                if (premcur.Lineup == -100)
                    premcur.Lineup = prem0.Lineup;
                if (premcur.MinFitness == -100)
                    premcur.MinFitness = prem0.MinFitness;
            }
        }
    }
}

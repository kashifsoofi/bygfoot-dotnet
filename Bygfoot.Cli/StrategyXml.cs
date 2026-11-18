using System.Globalization;
using System.Xml;

namespace Bygfoot.Cli;

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
    private const string TagStrategyPrematchFormation = "formation";
    private const string TagStrategyPrematchLineup = "lineup";
    private const string TagStrategyPrematchBoost = "prematch_boost";
    private const string TagStrategyPrematchStyle = "prematch_style";
    private const string TagStrategyMatchAction = "match_action";
    private const string TagStrategyMatchActionBoost = "boost";
    private const string TagStrategyMatchActionStyle = "style";
    private const string TagStrategyMatchActionSub = "sub";
    private const string TagStrategyMatchActionSubInPos = "in";
    private const string TagStrategyMatchActionSubOutPos = "out";

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
    /// Enum with the states used in the strategy XML parser.
    /// Corresponds to the C enum of the strategy module.
    /// </summary>
    private enum XmlStrategyStates
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

    /// <summary>
    /// Function reading a strategy file and adding the strategy
    /// to the strategies array.
    /// </summary>
    /// <param name="filename">The strategy xml file to read.</param>
    public static void StrategyXmlRead(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        Strategy newStrategy = new()
        {
            Sid = string.Empty,
            Desc = string.Empty,
            Priority = 1,
            Prematch = new List<StrategyPrematch>(),
            MatchAction = new List<StrategyMatchAction>()
        };
        Strategy.AllStrategies.Add(newStrategy);

        var parser = new Parser(newStrategy);
        using XmlReader reader = XmlReader.Create(filename);
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    parser.ReadStartElement(reader);
                    break;
                case XmlNodeType.EndElement:
                    parser.ReadEndElement(reader.LocalName);
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.SignificantWhitespace:
                case XmlNodeType.Whitespace:
                    parser.ReadText(reader.Value);
                    break;
            }
        }

        // Check that all match actions have conditions.
        // C: main_exit_program(EXIT_STRATEGY_ERROR,
        //   "xml_strategy_read: unconditional match action encountered in file %s. match actions MUST have conditions.", filename);
        foreach (StrategyMatchAction matchAction in newStrategy.MatchAction)
        {
            if (string.IsNullOrEmpty(matchAction.Condition))
            {
                // The exit is intentionally not active yet.
            }
        }
    }

    /// <summary>
    /// Single-file strategy XML parser. One instance is created per file;
    /// the instance fields correspond to the file-scope statics of the C
    /// xml_strategy module (state, action_id, curstrat, curprematch and
    /// curmatchaction).
    /// </summary>
    private sealed class Parser
    {
        private readonly Strategy _strategy;
        private XmlStrategyStates _state = XmlStrategyStates.StateStrategy;
        private int _actionId;
        private StrategyPrematch _prematch = new();
        private StrategyMatchAction _matchAction = new();

        public Parser(Strategy strategy) => _strategy = strategy;

        /// <summary>
        /// Start element handler of the strategy XML parser, dispatching
        /// on the name of the opening tag and updating the parser state
        /// accordingly.
        /// </summary>
        public void ReadStartElement(XmlReader reader)
        {
            switch (reader.LocalName)
            {
                case TagStrategy:
                    _state = XmlStrategyStates.StateStrategy;
                    break;
                case TagStrategySid:
                    _state = XmlStrategyStates.StateStrategySid;
                    break;
                case TagStrategyDesc:
                    _state = XmlStrategyStates.StateStrategyDesc;
                    break;
                case TagStrategyPriority:
                    _state = XmlStrategyStates.StateStrategyPriority;
                    break;
                case TagStrategyPrematch:
                {
                    StrategyPrematch newPrematch = new()
                    {
                        Condition = string.Empty,
                        Lineup = -100,
                        Boost = -100,
                        Style = -100,
                        MinFitness = 0,
                        Formations = new List<int>()
                    };
                    _state = XmlStrategyStates.StateStrategyPrematch;
                    if (string.IsNullOrEmpty(newPrematch.Condition))
                        newPrematch.Condition = reader.GetAttribute(AttNameCond) ?? string.Empty;
                    _strategy.Prematch.Add(newPrematch);
                    _prematch = newPrematch;
                    break;
                }
                case TagStrategyPrematchFormation:
                    _state = XmlStrategyStates.StateStrategyPrematchFormation;
                    break;
                case TagStrategyPrematchLineup:
                {
                    _state = XmlStrategyStates.StateStrategyPrematchLineup;
                    string? minFitness = reader.GetAttribute(AttNameMinFitness);
                    if (!string.IsNullOrEmpty(minFitness) &&
                        float.TryParse(minFitness, NumberStyles.Float, CultureInfo.InvariantCulture, out float minFitnessValue))
                        _prematch.MinFitness = minFitnessValue / 100f;
                    break;
                }
                case TagStrategyPrematchBoost:
                    _state = XmlStrategyStates.StateStrategyPrematchBoost;
                    break;
                case TagStrategyPrematchStyle:
                    _state = XmlStrategyStates.StateStrategyPrematchStyle;
                    break;
                case TagStrategyMatchAction:
                {
                    StrategyMatchAction newMatchAction = new()
                    {
                        Condition = string.Empty,
                        SubCondition = string.Empty,
                        Boost = -100,
                        Style = -100,
                        SubInPos = -1,
                        SubInProp = -1,
                        SubOutPos = -1,
                        SubOutProp = -1,
                        Id = _actionId++
                    };
                    _state = XmlStrategyStates.StateStrategyMatchAction;
                    if (string.IsNullOrEmpty(newMatchAction.Condition))
                        newMatchAction.Condition = reader.GetAttribute(AttNameCond) ?? string.Empty;
                    _strategy.MatchAction.Add(newMatchAction);
                    _matchAction = newMatchAction;
                    break;
                }
                case TagStrategyMatchActionBoost:
                    _state = XmlStrategyStates.StateStrategyMatchActionBoost;
                    break;
                case TagStrategyMatchActionStyle:
                    _state = XmlStrategyStates.StateStrategyMatchActionStyle;
                    break;
                case TagStrategyMatchActionSub:
                    _state = XmlStrategyStates.StateStrategyMatchActionSub;
                    if (string.IsNullOrEmpty(_matchAction.SubCondition))
                        _matchAction.SubCondition = reader.GetAttribute(AttNameCond) ?? string.Empty;
                    break;
                case TagStrategyMatchActionSubInPos:
                    _state = XmlStrategyStates.StateStrategyMatchActionSubInPos;
                    SetSubProperty(_matchAction, reader, isSubIn: true);
                    break;
                case TagStrategyMatchActionSubOutPos:
                    _state = XmlStrategyStates.StateStrategyMatchActionSubOutPos;
                    SetSubProperty(_matchAction, reader, isSubIn: false);
                    break;
                default:
                    Debug.DebugPrintMessage($"xml_strategy_read_start_element: unknown tag: {reader.LocalName}; I'm in state {_state}");
                    break;
            }
        }

        /// <summary>
        /// End element handler of the strategy XML parser, walking the
        /// parser state back up to the enclosing element.
        /// </summary>
        public void ReadEndElement(string elementName)
        {
            switch (elementName)
            {
                case TagStrategySid:
                case TagStrategyDesc:
                case TagStrategyPriority:
                case TagStrategyMatchAction:
                case TagStrategyPrematch:
                    _state = XmlStrategyStates.StateStrategy;
                    break;
                case TagStrategyPrematchFormation:
                case TagStrategyPrematchLineup:
                case TagStrategyPrematchBoost:
                case TagStrategyPrematchStyle:
                    _state = XmlStrategyStates.StateStrategyPrematch;
                    break;
                case TagStrategyMatchActionSub:
                case TagStrategyMatchActionStyle:
                case TagStrategyMatchActionBoost:
                    _state = XmlStrategyStates.StateStrategyMatchAction;
                    break;
                case TagStrategyMatchActionSubInPos:
                case TagStrategyMatchActionSubOutPos:
                    _state = XmlStrategyStates.StateStrategyMatchActionSub;
                    break;
                case TagStrategy:
                    break;
                default:
                    Debug.DebugPrintMessage($"xml_strategy_read_end_element: unknown tag: {elementName}; I'm in state {_state}");
                    break;
            }
        }

        /// <summary>
        /// Text handler of the strategy XML parser. Fills in the strategy
        /// variables according to the current parser state.
        /// </summary>
        public void ReadText(string text)
        {
            // C: float_value = (gfloat)g_ascii_strtod(...); int_value = (int)float_value;
            int intValue = float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue)
                ? (int)floatValue
                : 0;

            switch (_state)
            {
                case XmlStrategyStates.StateStrategySid:
                    _strategy.Sid = text;
                    break;
                case XmlStrategyStates.StateStrategyDesc:
                    _strategy.Desc = text;
                    break;
                case XmlStrategyStates.StateStrategyPriority:
                    if (Strategy.AllStrategies.Count == 1)
                        _strategy.Priority = intValue;
                    else
                        _strategy.Priority = Strategy.AllStrategies[Strategy.AllStrategies.Count - 2].Priority + intValue;
                    break;
                case XmlStrategyStates.StateStrategyPrematchFormation:
                    _prematch.Formations.Add(intValue);
                    break;
                case XmlStrategyStates.StateStrategyPrematchLineup:
                    SetLineup(_prematch, text);
                    break;
                case XmlStrategyStates.StateStrategyPrematchBoost:
                    SetBoost(_prematch, text);
                    break;
                case XmlStrategyStates.StateStrategyPrematchStyle:
                    SetStyle(_prematch, text);
                    break;
                case XmlStrategyStates.StateStrategyMatchActionStyle:
                    SetStyle(_matchAction, text);
                    break;
                case XmlStrategyStates.StateStrategyMatchActionBoost:
                    SetBoost(_matchAction, text);
                    break;
                case XmlStrategyStates.StateStrategyMatchActionSubInPos:
                    _matchAction.SubInPos = ParseSubPos(text);
                    break;
                case XmlStrategyStates.StateStrategyMatchActionSubOutPos:
                    _matchAction.SubOutPos = ParseSubPos(text);
                    break;
            }
        }

        /// <summary>Read the "prop" attribute of a "in"/"out" tag.</summary>
        private static void SetSubProperty(StrategyMatchAction matchAction, XmlReader reader, bool isSubIn)
        {
            int prop = reader.GetAttribute(AttNameSubProperty) switch
            {
                LineupNameBest => (int)StratLineupType.Best,
                LineupNameWeakest => (int)StratLineupType.Weakest,
                LineupNameFittest => (int)StratLineupType.Fittest,
                LineupNameUnfittest => (int)StratLineupType.Unfittest,
                _ => -100 // unknown, keep the current value
            };

            if (prop == -100)
                Debug.DebugPrintMessage($"xml_strategy_read_start_element: unknown property value {reader.GetAttribute(AttNameSubProperty)}");
            else if (isSubIn)
                matchAction.SubInProp = prop;
            else
                matchAction.SubOutProp = prop;
        }

        private static void SetLineup(StrategyPrematch prematch, string text)
        {
            int lineup = text switch
            {
                LineupNameBest => (int)StratLineupType.Best,
                LineupNameWeakest => (int)StratLineupType.Weakest,
                LineupNameFittest => (int)StratLineupType.Fittest,
                LineupNameUnfittest => (int)StratLineupType.Unfittest,
                _ => -100 // unknown, keep the current value
            };

            if (lineup == -100)
                Debug.DebugPrintMessage($"xml_strategy_read_text: unknown lineup type {text}");
            else
                prematch.Lineup = lineup;
        }

        private static void SetBoost(StrategyPrematch prematch, string text)
        {
            int boost = text switch
            {
                BoostNameOn => 1,
                BoostNameOff => 0,
                BoostNameAnti => -1,
                _ => -100 // unknown, keep the current value
            };

            if (boost == -100)
                Debug.DebugPrintMessage($"xml_strategy_read_text: unknown boost type {text}");
            else
                prematch.Boost = boost;
        }

        private static void SetBoost(StrategyMatchAction matchAction, string text)
        {
            int boost = text switch
            {
                BoostNameOn => 1,
                BoostNameOff => 0,
                BoostNameAnti => -1,
                _ => -100 // unknown, keep the current value
            };

            if (boost == -100)
                Debug.DebugPrintMessage($"xml_strategy_read_text: unknown boost type {text}");
            else
                matchAction.Boost = boost;
        }

        private static void SetStyle(StrategyPrematch prematch, string text)
        {
            int style = text switch
            {
                StyleNameAllOutDefend => -2,
                StyleNameDefend => -1,
                StyleNameBalanced => 0,
                StyleNameAttack => 1,
                StyleNameAllOutAttack => 2,
                _ => -100 // unknown, keep the current value
            };

            if (style == -100)
                Debug.DebugPrintMessage($"xml_strategy_read_text: unknown style type {text}");
            else
                prematch.Style = style;
        }

        private static void SetStyle(StrategyMatchAction matchAction, string text)
        {
            int style = text switch
            {
                StyleNameAllOutDefend => -2,
                StyleNameDefend => -1,
                StyleNameBalanced => 0,
                StyleNameAttack => 1,
                StyleNameAllOutAttack => 2,
                _ => -100 // unknown, keep the current value
            };

            if (style == -100)
                Debug.DebugPrintMessage($"xml_strategy_read_text: unknown style type {text}");
            else
                matchAction.Style = style;
        }

        /// <summary>
        /// Parses a space-separated list of position names ("goalie defender
        /// midfielder forward") into the C position integer.
        /// </summary>
        private static int ParseSubPos(string text)
        {
            int pos = 9;
            foreach (string position in text.Split(' '))
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

            return pos < 100 ? pos % 10 : pos;
        }
    }

    /// <summary>
    /// Load all strategy files found in the appropriate folder.
    /// Corresponds to C: xml_strategy_load_strategies.
    /// </summary>
    public static void StrategyLoadStrategies()
    {
        Debug.DebugPrintMessage("xml_strategy_load_strategies\n");

        string? strategyDir = FileHelper.GetFirstSupportDirSuffix("strategy");

        if (strategyDir == null)
        {
            Main.MainExitProgram(Bygfoot.ExitCode.StrategyError,
                "xml_strategy_load_strategies: strategy directory not found.");
            return;
        }

        List<string> files = FileHelper.GetDirectoryContents(strategyDir, "strategy_", ".xml");

        if (files.Count == 0)
            Main.MainExitProgram(Bygfoot.ExitCode.StrategyError,
                "xml_strategy_load_strategies: no CPU strategies found.");

        // C: sprintf(buf, "%s%s%s", strategydir, G_DIR_SEPARATOR_S, name) —
        // GetDirectoryContents already returns the full paths.
        foreach (string file in files)
            StrategyXmlRead(file);

        /* Fill secondary prematches with default values. */
        for (int i = 0; i < Strategy.AllStrategies.Count; i++)
        {
            StrategyPrematch prem0 = Strategy.AllStrategies[i].Prematch[0];

            if (prem0.Formations.Count == 0)
                Main.MainExitProgram(Bygfoot.ExitCode.StrategyError,
                    "xml_strategy_read ({0}): The primary prematch should have at least one formation.",
                    Strategy.AllStrategies[i].Sid);

            for (int j = 1; j < Strategy.AllStrategies[i].Prematch.Count; j++)
            {
                StrategyPrematch premcur = Strategy.AllStrategies[i].Prematch[j];

                if (premcur.Formations.Count == 0)
                    premcur.Formations.AddRange(prem0.Formations);

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

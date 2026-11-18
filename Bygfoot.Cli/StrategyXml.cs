using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Bygfoot.Cli;

/// <summary>
/// Static class to handle the reading of strategy XML files.
/// Corresponds to the C module src/xml_strategy.h and src/xml_strategy.c
/// (xml_strategy_read and xml_strategy_load_strategies).
///
/// The C markup-parser callbacks (xml_strategy_read_start_element,
/// xml_strategy_read_end_element and xml_strategy_read_text, plus the
/// STATE_STRATEGY* state enum) are replaced by XmlSerializer: the file is
/// deserialized into the XmlStrategy* proxy types below, and
/// XmlStrategy.ToStrategy() maps the values onto the C struct POCOs
/// (Strategy, StrategyPrematch, StrategyMatchAction). The word values for
/// lineup/boost/style are kept as strings on the proxies so the same
/// "unknown word keeps the -100 default" leniency as the C parser.
/// </summary>
public static class StrategyXml
{
    // Element and attribute names of the strategy files. Internal (not
    // private) because the XmlStrategy* proxy types below, which are
    // top-level, reference them in their [Xml...] attributes.
    internal const string TagStrategy = "strategy";
    internal const string TagStrategySid = "sid";
    internal const string TagStrategyDesc = "desc";
    internal const string TagStrategyPriority = "priority";
    internal const string TagStrategyPrematch = "prematch";
    internal const string TagStrategyPrematchFormation = "formation";
    internal const string TagStrategyPrematchLineup = "lineup";
    internal const string TagStrategyPrematchBoost = "prematch_boost";
    internal const string TagStrategyPrematchStyle = "prematch_style";
    internal const string TagStrategyMatchAction = "match_action";
    internal const string TagStrategyMatchActionBoost = "boost";
    internal const string TagStrategyMatchActionStyle = "style";
    internal const string TagStrategyMatchActionSub = "sub";
    internal const string TagStrategyMatchActionSubInPos = "in";
    internal const string TagStrategyMatchActionSubOutPos = "out";

    internal const string AttNameMinFitness = "min_fitness";
    internal const string AttNameCond = "cond";
    internal const string AttNameSubProperty = "prop";

    // The word values of the lineup/boost/style/position elements. Internal
    // because they are referenced from XmlStrategy.ToStrategy().
    internal const string LineupNameBest = "best";
    internal const string LineupNameWeakest = "weakest";
    internal const string LineupNameFittest = "fittest";
    internal const string LineupNameUnfittest = "unfittest";

    internal const string BoostNameOn = "on";
    internal const string BoostNameOff = "off";
    internal const string BoostNameAnti = "anti";

    internal const string StyleNameAllOutDefend = "all-out-defend";
    internal const string StyleNameDefend = "defend";
    internal const string StyleNameBalanced = "balanced";
    internal const string StyleNameAttack = "attack";
    internal const string StyleNameAllOutAttack = "all-out-attack";

    internal const string PosNameGoalie = "goalie";
    internal const string PosNameDefender = "defender";
    internal const string PosNameMidfielder = "midfielder";
    internal const string PosNameForward = "forward";

    /// <summary>
    /// Function reading a strategy file.
    /// </summary>
    /// <param name="filename">The strategy xml file to read.</param>
    /// <returns>
    /// The loaded strategy (not yet appended to AllStrategies — the caller
    /// adds it), or null if the file does not exist.
    /// </returns>
    public static Strategy? StrategyXmlRead(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return null;

        XmlStrategy? xml;
        using (XmlReader reader = XmlReader.Create(filename))
        {
            try
            {
                xml = new XmlSerializer(typeof(XmlStrategy)).Deserialize(reader) as XmlStrategy;
            }
            catch (Exception ex)
            {
                // C: g_critical("xml_strategy_read: error parsing file %s\n", filename);
                //     misc_print_error(&error, TRUE);
                Debug.DebugPrintMessage($"xml_strategy_read: error parsing file {filename}");
                Misc.MiscPrintError(ex, abortProgram: true);
                return null;
            }
        }

        if (xml is not { })
        {
            // C: g_critical("xml_strategy_read: error parsing file %s\n", filename);
            //     misc_print_error(&error, TRUE);
            Debug.DebugPrintMessage($"xml_strategy_read: error parsing file {filename}");
            Misc.MiscPrintError(new InvalidOperationException("strategy root element missing"), abortProgram: true);
            return null;
        }

        Strategy strategy = xml.ToStrategy();

        // Check that all match actions have conditions.
        // C: main_exit_program(EXIT_STRATEGY_ERROR,
        //   "xml_strategy_read: unconditional match action encountered in file %s. match actions MUST have conditions.", filename);
        foreach (StrategyMatchAction matchAction in strategy.MatchAction)
        {
            if (string.IsNullOrEmpty(matchAction.Condition))
            {
                // The exit is intentionally not active yet.
            }
        }

        return strategy;
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
        {
            Strategy? loaded = StrategyXmlRead(file);
            if (loaded is { })
                Strategy.AllStrategies.Add(loaded);
        }

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

/// <summary>
/// XmlSerializer root type for strategy files (&lt;strategy&gt;).
/// </summary>
[XmlRoot(StrategyXml.TagStrategy)]
public sealed class XmlStrategy
{
    [XmlElement(StrategyXml.TagStrategySid)]
    public string? Sid { get; set; }

    [XmlElement(StrategyXml.TagStrategyDesc)]
    public string? Desc { get; set; }

    [XmlElement(StrategyXml.TagStrategyPriority)]
    public int Priority { get; set; } = 1;

    [XmlElement(StrategyXml.TagStrategyPrematch)]
    public List<XmlPrematch> Prematch { get; set; } = new();

    [XmlElement(StrategyXml.TagStrategyMatchAction)]
    public List<XmlMatchAction> MatchAction { get; set; } = new();

    /// <summary>
    /// Maps this deserialized proxy onto a new Strategy instance (the C
    /// struct POCO), replicating the value resolution of the old C parser
    /// callbacks. The caller appends the result to Strategy.AllStrategies.
    /// </summary>
    public Strategy ToStrategy()
    {
        Strategy strategy = new()
        {
            Sid = Sid ?? string.Empty,
            Desc = Desc ?? string.Empty,
            Priority = 1,
            Prematch = new List<StrategyPrematch>(),
            MatchAction = new List<StrategyMatchAction>()
        };

        // C: the priority is relative — the first strategy loaded gets the
        // file value, subsequent strategies accumulate on the previous one.
        if (Strategy.AllStrategies.Count == 0)
            strategy.Priority = Priority;
        else
            strategy.Priority = Strategy.AllStrategies[^1].Priority + Priority;

        foreach (XmlPrematch xmlPrematch in Prematch)
        {
            StrategyPrematch prematch = new()
            {
                Condition = string.Empty,
                Lineup = -100,
                Boost = -100,
                Style = -100,
                MinFitness = 0,
                Formations = new List<int>()
            };
            prematch.Condition = xmlPrematch.Condition ?? string.Empty;
            prematch.Formations.AddRange(xmlPrematch.Formations);
            SetLineup(prematch, xmlPrematch.Lineup?.LineupType);
            SetBoost(prematch, xmlPrematch.Boost);
            SetStyle(prematch, xmlPrematch.Style);

            // C: min_fitness attribute on the lineup element, given in percent.
            if (!string.IsNullOrEmpty(xmlPrematch.Lineup?.MinFitness) &&
                float.TryParse(xmlPrematch.Lineup.MinFitness, NumberStyles.Float, CultureInfo.InvariantCulture, out float minFitnessValue))
                prematch.MinFitness = minFitnessValue / 100f;

            strategy.Prematch.Add(prematch);
        }

        // C: action_id is a per-file running counter.
        int actionId = 0;
        foreach (XmlMatchAction xmlMatchAction in MatchAction)
        {
            StrategyMatchAction matchAction = new()
            {
                Condition = string.Empty,
                SubCondition = string.Empty,
                Boost = -100,
                Style = -100,
                SubInPos = -1,
                SubInProp = -1,
                SubOutPos = -1,
                SubOutProp = -1,
                Id = actionId++
            };
            matchAction.Condition = xmlMatchAction.Condition ?? string.Empty;
            SetStyle(matchAction, xmlMatchAction.Style);
            SetBoost(matchAction, xmlMatchAction.Boost);

            if (xmlMatchAction.Sub is { } sub)
            {
                matchAction.SubCondition = sub.Condition ?? string.Empty;
                if (sub.In is { } subIn)
                {
                    matchAction.SubInPos = ParseSubPos(subIn.Position);
                    SetSubProperty(matchAction, subIn.Prop, isSubIn: true);
                }
                if (sub.Out is { } subOut)
                {
                    matchAction.SubOutPos = ParseSubPos(subOut.Position);
                    SetSubProperty(matchAction, subOut.Prop, isSubIn: false);
                }
            }

            strategy.MatchAction.Add(matchAction);
        }

        return strategy;
    }

    /// <summary>
    /// Reads the "prop" attribute of a "in"/"out" tag.
    /// </summary>
    private static void SetSubProperty(StrategyMatchAction matchAction, string? prop, bool isSubIn)
    {
        int value = prop switch
        {
            StrategyXml.LineupNameBest => (int)StratLineupType.Best,
            StrategyXml.LineupNameWeakest => (int)StratLineupType.Weakest,
            StrategyXml.LineupNameFittest => (int)StratLineupType.Fittest,
            StrategyXml.LineupNameUnfittest => (int)StratLineupType.Unfittest,
            _ => -100 // unknown, keep the current value
        };

        if (value == -100)
            Debug.DebugPrintMessage($"xml_strategy_read: unknown property value {prop}");
        else if (isSubIn)
            matchAction.SubInProp = value;
        else
            matchAction.SubOutProp = value;
    }

    /// <summary>
    /// Parses the lineup element text; unknown (or missing) values keep the
    /// current (default -100) value, like the C parser.
    /// </summary>
    private static void SetLineup(StrategyPrematch prematch, string? text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        int lineup = text switch
        {
            StrategyXml.LineupNameBest => (int)StratLineupType.Best,
            StrategyXml.LineupNameWeakest => (int)StratLineupType.Weakest,
            StrategyXml.LineupNameFittest => (int)StratLineupType.Fittest,
            StrategyXml.LineupNameUnfittest => (int)StratLineupType.Unfittest,
            _ => -100 // unknown, keep the current value
        };

        if (lineup == -100)
            Debug.DebugPrintMessage($"xml_strategy_read: unknown lineup type {text}");
        else
            prematch.Lineup = lineup;
    }

    /// <summary>
    /// Parses a boost element text; unknown (or missing) values keep the
    /// current (default -100) value, like the C parser.
    /// </summary>
    private static void SetBoost(StrategyPrematch prematch, string? text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        int boost = text switch
        {
            StrategyXml.BoostNameOn => 1,
            StrategyXml.BoostNameOff => 0,
            StrategyXml.BoostNameAnti => -1,
            _ => -100 // unknown, keep the current value
        };

        if (boost == -100)
            Debug.DebugPrintMessage($"xml_strategy_read: unknown boost type {text}");
        else
            prematch.Boost = boost;
    }

    /// <summary>
    /// Parses a boost element text; unknown (or missing) values keep the
    /// current (default -100) value, like the C parser.
    /// </summary>
    private static void SetBoost(StrategyMatchAction matchAction, string? text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        int boost = text switch
        {
            StrategyXml.BoostNameOn => 1,
            StrategyXml.BoostNameOff => 0,
            StrategyXml.BoostNameAnti => -1,
            _ => -100 // unknown, keep the current value
        };

        if (boost == -100)
            Debug.DebugPrintMessage($"xml_strategy_read: unknown boost type {text}");
        else
            matchAction.Boost = boost;
    }

    /// <summary>
    /// Parses a style element text; unknown (or missing) values keep the
    /// current (default -100) value, like the C parser.
    /// </summary>
    private static void SetStyle(StrategyPrematch prematch, string? text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        int style = text switch
        {
            StrategyXml.StyleNameAllOutDefend => -2,
            StrategyXml.StyleNameDefend => -1,
            StrategyXml.StyleNameBalanced => 0,
            StrategyXml.StyleNameAttack => 1,
            StrategyXml.StyleNameAllOutAttack => 2,
            _ => -100 // unknown, keep the current value
        };

        if (style == -100)
            Debug.DebugPrintMessage($"xml_strategy_read: unknown style type {text}");
        else
            prematch.Style = style;
    }

    /// <summary>
    /// Parses a style element text; unknown (or missing) values keep the
    /// current (default -100) value, like the C parser.
    /// </summary>
    private static void SetStyle(StrategyMatchAction matchAction, string? text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        int style = text switch
        {
            StrategyXml.StyleNameAllOutDefend => -2,
            StrategyXml.StyleNameDefend => -1,
            StrategyXml.StyleNameBalanced => 0,
            StrategyXml.StyleNameAttack => 1,
            StrategyXml.StyleNameAllOutAttack => 2,
            _ => -100 // unknown, keep the current value
        };

        if (style == -100)
            Debug.DebugPrintMessage($"xml_strategy_read: unknown style type {text}");
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
            if (position == StrategyXml.PosNameGoalie)
                pos *= 10;
            else if (position == StrategyXml.PosNameDefender)
                pos = (pos * 10) + 1;
            else if (position == StrategyXml.PosNameMidfielder)
                pos = (pos * 10) + 2;
            else if (position == StrategyXml.PosNameForward)
                pos = (pos * 10) + 3;
            else
                Debug.DebugPrintMessage($"xml_strategy_read: unknown position {position}");
        }

        return pos < 100 ? pos % 10 : pos;
    }
}

/// <summary>XmlSerializer type for a &lt;prematch&gt; element.</summary>
public sealed class XmlPrematch
{
    [XmlAttribute(StrategyXml.AttNameCond)]
    public string? Condition { get; set; }

    [XmlElement(StrategyXml.TagStrategyPrematchFormation)]
    public List<int> Formations { get; set; } = new();

    /// <summary>
    /// The lineup element: its text is the lineup word ("best", "weakest",
    /// "fittest", "unfittest"), its min_fitness attribute the minimum fitness
    /// (percent). Modelled as a complex type because the .NET
    /// XmlAttributeAttribute has no ElementName to bind the attribute to the
    /// lineup element.
    /// </summary>
    [XmlElement(StrategyXml.TagStrategyPrematchLineup)]
    public XmlLineup? Lineup { get; set; }

    /// <summary>prematch_boost element text ("on", "off", "anti").</summary>
    [XmlElement(StrategyXml.TagStrategyPrematchBoost)]
    public string? Boost { get; set; }

    /// <summary>prematch_style element text.</summary>
    [XmlElement(StrategyXml.TagStrategyPrematchStyle)]
    public string? Style { get; set; }
}

/// <summary>XmlSerializer type for the &lt;lineup&gt; element (text + min_fitness attribute).</summary>
public sealed class XmlLineup
{
    [XmlText]
    public string LineupType { get; set; } = string.Empty;

    [XmlAttribute(StrategyXml.AttNameMinFitness)]
    public string? MinFitness { get; set; }
}

/// <summary>XmlSerializer type for a &lt;match_action&gt; element.</summary>
public sealed class XmlMatchAction
{
    [XmlAttribute(StrategyXml.AttNameCond)]
    public string? Condition { get; set; }

    /// <summary>boost element text ("on", "off", "anti").</summary>
    [XmlElement(StrategyXml.TagStrategyMatchActionBoost)]
    public string? Boost { get; set; }

    /// <summary>style element text.</summary>
    [XmlElement(StrategyXml.TagStrategyMatchActionStyle)]
    public string? Style { get; set; }

    [XmlElement(StrategyXml.TagStrategyMatchActionSub)]
    public XmlSub? Sub { get; set; }
}

/// <summary>XmlSerializer type for a &lt;sub&gt; element of a match action.</summary>
public sealed class XmlSub
{
    [XmlAttribute(StrategyXml.AttNameCond)]
    public string? Condition { get; set; }

    /// <summary>The &lt;in&gt; element (position list text + prop attribute).</summary>
    [XmlElement(StrategyXml.TagStrategyMatchActionSubInPos)]
    public XmlSubPosition? In { get; set; }

    /// <summary>The &lt;out&gt; element (position list text + prop attribute).</summary>
    [XmlElement(StrategyXml.TagStrategyMatchActionSubOutPos)]
    public XmlSubPosition? Out { get; set; }
}

/// <summary>
/// XmlSerializer type for an &lt;in&gt;/&lt;out&gt; element: the element text
/// is the space-separated position list, the "prop" attribute the lineup type.
/// </summary>
public sealed class XmlSubPosition
{
    [XmlText]
    public string Position { get; set; } = string.Empty;

    [XmlAttribute(StrategyXml.AttNameSubProperty)]
    public string? Prop { get; set; }
}

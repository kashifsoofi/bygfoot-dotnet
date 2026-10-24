namespace Bygfoot.Cli.Models;

using System;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Lineup types for a CPU team (ie. which players are preferred when putting together the first 11). 
/// Corresponds to C enum StratLineupType.
/// </summary>
public enum StratLineupType
{
    Best = 1,        // STRAT_LINEUP_BEST
    Weakest,         // STRAT_LINEUP_WEAKEST
    Fittest,         // STRAT_LINEUP_FITTEST,
    Unfittest,       // STRAT_LINEUP_UNFITTEST
    End              // STRent_lineup_end (sentinel)
}

/// <summary>
/// A struct describing the pre-match strategy settings of a CPU team. 
/// Corresponds to C struct StrategyPrematch.
/// </summary>
public class StrategyPrematch
{
    /// <summary>A condition describing when the strategy should be applied. Corresponds to C: gchar *condition.</summary>
    public string Condition { get; set; } = string.Empty;

    /// <summary>Array of possible formations, sorted by preference. Corresponds to C: GArray *formations.</summary>
    public List<string> Formations { get; set; } = new List<string>();

    /// <summary>Boost, style values and lineup type. Corresponds to C: gint boost, style, lineup.</summary>
    public int Boost { get; set; }
    public int Style { get; set; }
    public int Lineup { get; set; }

    /// <summary>The fitness value below which a player gets substituted if there's a sub with better fitness. Corresponds to C: gfloat min_fitness.</summary>
    public float MinFitness { get; set; }
}

/// <summary>
/// A structure describing a own-team action during a match. 
/// Corresponds to C struct StrategyMatchAction.
/// </summary>
public class StrategyMatchAction
{
    /// <summary>A condition describing when the action should be taken. Corresponds to C: gchar *condition, *sub_condition.</summary>
    public string Condition { get; set; } = string.Empty;
    public string SubCondition { get; set; } = string.Empty;

    /// <summary>New boost and style values. Corresponds to C: gint boost, style.</summary>
    public int Boost { get; set; }
    public int Style { get; set; }

    /// <summary>Substitution specifiers (position and property). Corresponds to C: gint sub_in_pos, sub_in_prop, sub_out_pos, sub_out_prop.</summary>
    public int SubInPos { get; set; }
    public int SubInProp { get; set; }
    public int SubOutPos { get; set; }
    public int SubOutProp { get; set; }

    /// <summary>An id to prevent actions from being applied again and again during a match. Corresponds to C: gint id.</summary>
    public int Id { get; set; }
}

/// <summary>
/// A CPU strategy. 
/// Corresponds to C struct Strategy.
/// </summary>
public class Strategy
{
    /// <summary>String id and description of the strategy. Corresponds to C: gchar *sid, *desc.</summary>
    public string Sid { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;

    /// <summary>How often this strategy gets picked, relative to the other strategies. Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>Array with prematch settings. Corresponds to C: GArray *prematch.</summary>
    public List<StrategyPrematch> Prematch { get; set; } = new List<StrategyPrematch>();

    /// <summary>Array with match action settings. Corresponds to C: GArray *match_action.</summary>
    public List<StrategyMatchAction> MatchAction { get; set; } = new List<StrategyMatchAction>();

    public static List<Strategy> AllStrategies { get; set; } = new List<Strategy>();

    /// <summary>
    /// Loads all strategy XML files from the specified directory into AllStrategies.
    /// </summary>
    /// <param name="directoryPath">The directory containing XML files.</param>
    public static void LoadAll(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return;

        var files = Directory.GetFiles(directoryPath, "*.xml");
        foreach (var file in files)
        {
            try
            {
                var strategy = LoadFromXml(file);
                AllStrategies.Add(strategy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading strategy file {file}: {ex.Message}");
            }
        }
    }

    private static readonly Dictionary<string, StratLineupType> LineupTypes = new Dictionary<string, StratLineupType>
    {
        { "best", StratLineupType.Best },
        { "weakest", StratLineupType.Weakest },
        { "fittest", StratLineupType.Fittest },
        { "unfittest", StratLineupType.Unfittest }
    };

    private static readonly Dictionary<string, int> BoostTypes = new Dictionary<string, int>
    {
        { "on", 1 },
        { "off", 0 },
        { "anti", -1 }
    };

    private static readonly Dictionary<string, int> StyleTypes = new Dictionary<string, int>
    {
        { "all-out-defend", -2 },
        { "defend", -1 },
        { "balanced", 0 },
        { "attack", 1 },
        { "all-out-attack", 2 }
    };

    private static readonly Dictionary<string, int> PosTypes = new Dictionary<string, int>
    {
        { "goalie", 1 },
        { "defender", 2 },
        { "midfielder", 3 },
        { "forward", 4 }
    };

    /// <summary>
    /// Loads a strategy from an XML file.
    /// </summary>
    /// <param name="filePath">The path to the XML file.</param>
    /// <returns>A Strategy object.</returns>
    public static Strategy LoadFromXml(string filePath)
    {
        var strategy = new Strategy();
        var doc = new XmlDocument();
        doc.Load(filePath);

        var root = doc.DocumentElement;
        if (root == null) throw new Exception("Invalid XML file");

        // Load Root Strategy info
        foreach (XmlNode node in root.ChildNodes)
        {
            switch (node.Name.ToLower())
            {
                case "sid":
                    strategy.Sid = node.InnerText;
                    break;
                case "desc":
                    strategy.Desc = node.InnerText;
                    break;
                case "priority":
                    strategy.Priority = int.Parse(node.InnerText);
                    break;
                case "prematch":
                    LoadPrematch(node, strategy);
                    break;
                case "match_action":
                    LoadMatchAction(node, strategy);
                    break;
            }
        }

        return strategy;
    }

    private static void LoadPrematch(XmlNode node, Strategy strategy)
    {
        var prematch = new StrategyPrematch();

        foreach (XmlNode child in node.ChildNodes)
        {
            switch (child.Name.ToLower())
            {
                case "condition":
                    prematch.Condition = child.Attributes?["cond"]?.Value ?? child.InnerText;
                    break;
                case "formation":
                    prematch.Formations.Add(child.InnerText);
                    break;
                case "lineup":
                    var lineupStr = child.InnerText;
                    if (LineupTypes.TryGetValue(lineupStr, out var lineup))
                        prematch.Lineup = (int)lineup;
                    break;
                case "prematch_boost":
                    var boostStr = child.InnerText;
                    if (BoostTypes.TryGetValue(boostStr, out var boost))
                        prematch.Boost = boost;
                    break;
                case "prematch_style":
                    var styleStr = child.InnerText;
                    if (StyleTypes.TryGetValue(styleStr, out var style))
                        prematch.Style = style;
                    break;
                case "min_fitness":
                    if (float.TryParse(child.InnerText, out float minFitness))
                        prematch.MinFitness = minFitness / 100.0f;
                    break;
            }
        }
        strategy.Prematch.Add(prematch);
    }

    private static void LoadMatchAction(XmlNode node, Strategy strategy)
    {
        var action = new StrategyMatchAction();

        // Check attributes
        foreach (XmlAttribute attr in node.Attributes)
        {
            switch (attr.Name.ToLower())
            {
                case "condition":
                    action.Condition = attr.Value;
                    break;
                case "sub_condition":
                    action.SubCondition = attr.Value;
                    break;
                case "boost":
                    if (BoostTypes.TryGetValue(attr.Value, out var boost))
                        action.Boost = boost;
                    break;
                case "style":
                    if (StyleTypes.TryGetValue(attr.Value, out var style))
                        action.Style = style;
                    break;
            }
        }

        // Check children
        foreach (XmlNode child in node.ChildNodes)
        {
            switch (child.Name.ToLower())
            {
                case "id":
                    action.Id = int.Parse(child.InnerText);
                    break;
                case "sub":
                    if (child.Attributes != null)
                    {
                        foreach (XmlAttribute attr in child.Attributes)
                        {
                            switch (attr.Name.ToLower())
                            {
                                case "cond":
                                    action.SubCondition = attr.Value;
                                    break;
                                case "in":
                                    if (PosTypes.TryGetValue(attr.Value, out var inPos))
                                        action.SubInPos = inPos;
                                    break;
                                case "prop":
                                    if (LineupTypes.TryGetValue(attr.Value, out var inProp))
                                        action.SubInProp = (int)inProp;
                                    break;
                                case "out":
                                    if (PosTypes.TryGetValue(attr.Value, out var outPos))
                                        action.SubOutPos = outPos;
                                    break;
                            }
                        }
                    }
                    break;
            }
        }
        strategy.MatchAction.Add(action);
    }
}

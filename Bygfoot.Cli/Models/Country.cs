using System;
using System.Collections.Generic;
using System.Xml;

namespace Bygfoot.Cli.Models;

/// <summary>
/// A struct representing a country.
/// Corresponds to C struct Country in bygfoot.h.
/// </summary>
public class Country
{
    /// <summary>Name of the country. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>Symbol of the country, eg a flag pixmap. Corresponds to C: gchar *symbol.</summary>
    public string Symbol { get; set; }

    /// <summary>Id of the country, eg 'england'. Corresponds to C: gchar *sid.</summary>
    public string SId { get; set; }

    /// <summary>
    /// A rating point from 0-10 telling us how good the first league of the country is.
    /// Spain, for instance, has rating 10, whereas Ireland has only 5.
    /// Corresponds to C: gint rating.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>Leagues and cups arrays. Corresponds to C: GArray *leagues.</summary>
    public List<League> Leagues { get; set; } = new List<League>();

    /// <summary>Corresponds to C: GArray *cups.</summary>
    public List<Cup> Cups { get; set; } = new List<Cup>();

    /// <summary>Pointer array holding all cups. Corresponds to C: GPtrArray *allcups.</summary>
    public List<Cup> AllCups { get; set; } = new List<Cup>();

    /// <summary>
    /// Loads a country from an XML file.
    /// </summary>
    /// <param name="filePath">Path to the XML file.</param>
    public static Country LoadFromXml(string filePath)
    {
        var country = new Country();
        var doc = new XmlDocument();
        doc.Load(filePath);
        var root = doc.DocumentElement;
        if (root == null) return country;
        foreach (XmlNode node in root.ChildNodes)
        {
            switch (node.Name.ToLower())
            {
                case "name":
                    country.Name = node.InnerText;
                    break;
                case "symbol":
                    country.Symbol = node.InnerText;
                    break;
                case "sid":
                    country.SId = node.InnerText;
                    break;
                case "rating":
                    country.Rating = int.Parse(node.InnerText);
                    break;
                case "leagues":
                    LoadLeagues(node, country);
                    break;
                case "cups":
                    LoadCups(node, country);
                    break;
            }
        }
        return country;
    }

    private static void LoadLeagues(XmlNode node, Country country)
    {
        foreach (XmlNode leagueNode in node.ChildNodes)
        {
            if (leagueNode.Name.ToLower() == "league")
            {
                country.Leagues.Add(ParseLeague(leagueNode));
            }
        }
    }

    private static void LoadCups(XmlNode node, Country country)
    {
        foreach (XmlNode cupNode in node.ChildNodes)
        {
            if (cupNode.Name.ToLower() == "cup")
            {
                country.Cups.Add(ParseCup(cupNode));
            }
        }
    }

    private static League ParseLeague(XmlNode node)
    {
        return League.ParseLeague(node);
    }

    private static Cup ParseCup(XmlNode node)
    {
        return Cup.ParseCup(node);
    }
}

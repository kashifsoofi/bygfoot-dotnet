using System;
using System.Collections.Generic;

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
}

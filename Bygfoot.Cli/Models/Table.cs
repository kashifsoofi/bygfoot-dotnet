namespace Bygfoot.Cli.Models;

/// <summary>
/// Table element values.
/// </summary>
public enum TableElementValues
{
    TablePlayed = 0,
    TableWon,
    TableDraw,
    TableLost,
    TableGf,
    TableGa,
    TableGd,
    TablePts,
    TableEnd
}

/// <summary>
/// An element representing a team in the tables.
/// </summary>
public class TableElement
{
    /// <summary>
    /// The team object.
    /// </summary>
    public Team Team { get; set; }

    /// <summary>
    /// Unique identifier of the team.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// The rank of the element before the last update. Used to display an arrow if the rank changed.
    /// </summary>
    public int OldRank { get; set; }

    /// <summary>
    /// Values related to the table standings.
    /// </summary>
    public int[] Values { get; set; }
}

/// <summary>
/// A table belonging to a league or a cup with round robin.
/// </summary>
public class Table
{
    /// <summary>
    /// The name of the league or cup.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The league/cup ID. </summary>
    public int Clid { get; set; }

    /// <summary>
    /// The cup round (or -1 if it's a league). </summary>
    public int Round { get; set; }

    /// <summary>
    /// The elements participating in the table. (Assuming GArray maps to a collection type)
    /// </summary>
    public object Elements { get; set; } 
}
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
    /// The elements participating in the table.
    /// </summary>
    public List<TableElement> Elements { get; set; } = new List<TableElement>();

    /// <summary>
    /// Return a newly allocated empty table.
    /// Corresponds to C: table_new.
    /// </summary>
    /// <returns>A new table.</returns>
    public static Table New()
    {
        Table newTable = new Table();
        newTable.Name = null;
        newTable.Clid = -1;
        newTable.Round = -1;
        newTable.Elements = new List<TableElement>();

        return newTable;
    }

    /// <summary>
    /// Return a nullified table element.
    /// Corresponds to C: table_element_new.
    /// </summary>
    /// <param name="team">The team.</param>
    /// <param name="oldRank">The old rank.</param>
    /// <returns>A new table element.</returns>
    public static TableElement TableElementNew(Team team, int oldRank)
    {
        TableElement newElement = new TableElement();
        newElement.Team = team;
        newElement.TeamId = team.Id;
        newElement.OldRank = oldRank;
        newElement.Values = new int[(int)TableElementValues.TableEnd + 1];

        return newElement;
    }

    /// <summary>
    /// Update the appropriate table entries after a fixture has been calculated.
    /// Corresponds to C: table_update.
    /// </summary>
    /// <param name="fix">The fixture that's just been calculated.</param>
    public static void Update(Fixture fix)
    {
        int idx = fix.Result[0, 0] < fix.Result[1, 0] ? 1 : 0;
        int end = fix.Clid < Bygfoot.IdCupStart ? 2 : 1;

        for (int j = 0; j < end; j++)
        {
            TableElement element = null;
            TableUpdateGetElements(ref element, fix, j == 1);

            if (element == null)
                continue;

            element.Values[(int)TableElementValues.TablePlayed]++;
            element.Values[(int)TableElementValues.TableGf] += fix.Result[j, 0];
            element.Values[(int)TableElementValues.TableGa] += fix.Result[1 - j, 0];
            element.Values[(int)TableElementValues.TableGd] = element.Values[(int)TableElementValues.TableGf] - element.Values[(int)TableElementValues.TableGa];

            if (fix.Result[0, 0] == fix.Result[1, 0])
            {
                element.Values[(int)TableElementValues.TableDraw]++;
                element.Values[(int)TableElementValues.TablePts] += 1;
            }
            else
            {
                if (j == idx)
                {
                    element.Values[(int)TableElementValues.TableWon]++;
                    element.Values[(int)TableElementValues.TablePts] += 3;
                }
                else
                {
                    element.Values[(int)TableElementValues.TableLost]++;
                }
            }
        }
    }

    /// <summary>
    /// Get the pointers to the table entries representing the two teams from the fixture.
    /// Corresponds to C: table_update_get_elements.
    /// </summary>
    /// <param name="elements">The table elements.</param>
    /// <param name="fix">The fixture.</param>
    /// <param name="nonCumulative">Whether to return the last non-cumulative table.</param>
    public static void TableUpdateGetElements(ref TableElement elements, Fixture fix, bool nonCumulative)
    {
        // Implementation depends on the not yet converted league and cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compare function used to sort the tables.
    /// Corresponds to C: table_element_compare_func.
    /// </summary>
    /// <param name="element1">The first table element.</param>
    /// <param name="element2">The second table element.</param>
    /// <param name="clidPointer">The clid and the cup round of the table encoded as a pointer.</param>
    /// <returns>The comparison result.</returns>
    public static int TableElementCompareFunc(TableElement element1, TableElement element2, object clidPointer)
    {
        // Implementation depends on the not yet converted league, cup and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out if there are active leagues (which automatically have tables then) or cups with tables in the country.
    /// Corresponds to C: query_tables_in_country.
    /// </summary>
    /// <returns>Whether there are tables.</returns>
    public static bool QueryTablesInCountry()
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (League.QueryLeagueActive(league))
                return true;
        }

        foreach (Cup cup in Variables.Country.Cups)
        {
            if (CupHasTables(cup))
                return true;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (CupHasTables(cup))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Copy a table.
    /// Corresponds to C: table_copy.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The new table.</returns>
    public static Table Copy(Table table)
    {
        Table newTable = new Table();
        newTable.Name = table.Name;
        newTable.Clid = table.Clid;
        newTable.Round = table.Round;
        newTable.Elements = new List<TableElement>();

        foreach (TableElement element in table.Elements)
        {
            TableElement newElement = new TableElement();
            newElement.Team = element.Team;
            newElement.TeamId = element.TeamId;
            newElement.OldRank = element.OldRank;
            newElement.Values = new int[element.Values.Length];

            newTable.Elements.Add(newElement);
        }

        return newTable;
    }

    /// <summary>
    /// Refresh the team pointers in the table from the team ids.
    /// Corresponds to C: table_refresh_team_pointers.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The refreshed table.</returns>
    public static void RefreshTeamPointers(Table table)
    {
        foreach (TableElement element in table.Elements)
        {
            element.Team = Team.OfId(element.TeamId);
        }
    }

    /// <summary>
    /// Whether a cup has tables.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>Whether the cup has tables.</returns>
    private static bool CupHasTables(Cup cup)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }
}
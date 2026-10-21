namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Structure representing a fixture, or, in other words, a match. 
/// Corresponds to C struct Fixture.
/// </summary>
public class Fixture
{
    /// <summary>The cup or league the fixture belongs to. Corresponds to C: gint clid.</summary>
    public int Clid { get; set; }

    /// <summary>The unique id of the fixture. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>The round (in a cup) the fixture belongs to. Corresponds to C: gint round.</summary>
    public int Round { get; set; }

    /// <summary>The replay number (ie. how often the match was repeated because of a draw). Corresponds to C: gint replay_number.</summary>
    public int ReplayNumber { get; set; }

    /// <summary>When it takes place. Corresponds to C: gint week_number, week_round_number.</summary>
    public int WeekNumber { get; set; }
    public int WeekRoundNumber { get; set; }

    /// <summary>The teams involved. Corresponds to C: Team *teams[2].</summary>
    public List<Team> Teams { get; set; } = new List<Team>();

    /// <summary>Ids of the teams. Needed when the team pointers get invalid (e.g. after promotion/relegation). Corresponds to C: gint team_ids[2].</summary>
    public int[] TeamIds { get; set; } = new int[2];

    /// <summary>
    /// The number of goals for each team in regulation, extra time and penalty shoot-out. 
    /// Corresponds to C: gint result[2][3].
    /// </summary>
    public int[,] Result { get; set; } = new int[2, 3];

    /// <summary>Whether there's home advantage. Corresponds to C: gboolean home_advantage.</summary>
    public bool HomeAdvantage { get; set; }

    /// <summary>Whether this is the second leg. Corresponds to C: gboolean second_leg.</summary>
    public bool SecondLeg { get; set; }

    /// <summary>Whether the game has to be decided. Corresponds to C: gboolean decisive.</summary>
    public bool Decisive { get; set; }

    /// <summary>How many people attended. Corresponds to C: gint attendance.</summary>
    public int Attendance { get; set; }

    /// <summary>The live game used for the fixture calculation. Corresponds to C: gpointer live_game.</summary>
    public LiveGame LiveGame { get; set; }
}

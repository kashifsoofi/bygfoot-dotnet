namespace Bygfoot.Cli.Models;

/// <summary>
/// A struct representing a betting possibility: a fixture plus odds.
/// Corresponds to C struct BetMatch.
/// </summary>
public class BetMatch
{
    /// <summary>The match the bet is about. Corresponds to C: gint fix_id.</summary>
    public int FixId { get; set; }

    /// <summary>The odds for a win, draw or loss of the first team. Corresponds to C: gfloat odds[3].</summary>
    public float[] Odds { get; set; } = new float[3];
}

/// <summary>
/// A struct representing a bet by a user.
/// Corresponds to C struct BetUser.
/// </summary>
public class BetUser
{
    /// <summary>Match the user betted on. Corresponds to C: gint fix_id.</summary>
    public int FixId { get; set; }

    /// <summary>Which outcome he picked. Corresponds to C: gint outcome.</summary>
    public int Outcome { get; set; }

    /// <summary>How much he wagered. Corresponds to C: gint wager.</summary>
    public int Wager { get; set; }
}

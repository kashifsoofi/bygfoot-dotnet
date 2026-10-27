namespace Bygfoot.Cli.Models;

/// <summary>
/// The data about a user's youth academy is bundled here. 
/// Corresponds to C struct YouthAcademy.
/// </summary>
public class YouthAcademy
{
    /// <summary>Pointer to the team the academy belongs to. Corresponds to C: Team *tm.</summary>
    public Team Team { get; set; }

    /// <summary>Position preference of the user. Corresponds to C: gint pos_pref.</summary>
    public int PosPref { get; set; }

    /// <summary>Coach quality, like scout or physio. Corresponds to C: gint coach.</summary>
    public int Coach { get; set; }

    /// <summary>Percentage of income the user devotes to the youth academy. Corresponds to C: gint percentage.</summary>
    public int Percentage { get; set; }

    /// <summary>Average coach and percentage values; these are used when a new youth comes into the academy to determine the quality of the youth. Corresponds to C: gfloat av_coach, av_percentage.</summary>
    public float AvCoach { get; set; }

    /// <summary>
    /// Average percentage values; these are used when a new youth comes into the academy to determine the quality of the youth.
    /// Corresponds to C: gfloat av_percentage.
    /// </summary>
    public float AvPercentage { get; set; }

    /// <summary>
    /// When this counter is <= 0, a new youth appears. Corresponds to C: gfloat counter_youth.
    /// </summary>
    public float CounterYouth { get; set; }

    /// <summary>The youths are simply young players. Corresponds to C: GArray *players.</summary>
    public List<Player> Players { get; set; } = new List<Player>();
}

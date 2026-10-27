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

    /// <summary>
    /// Create a new youth academy and add the initial youths to it.
    /// Corresponds to C: youth_academy_new.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void New(User user)
    {
        user.YouthAcademy.Team = user.Team;
        user.YouthAcademy.Coach = (int)Quality.QualityAverage;
        user.YouthAcademy.PosPref = (int)PlayerPos.Any;
        user.YouthAcademy.Percentage = Variables.Constants.GetInt("int_youth_academy_default_percentage");
        user.YouthAcademy.AvCoach = (float)Quality.QualityAverage;
        user.YouthAcademy.AvPercentage = (float)Variables.Constants.GetInt("int_youth_academy_default_percentage");
        user.YouthAcademy.CounterYouth = Misc.MathRndi((int)Variables.Constants.GetFloat("float_youth_academy_youth_counter_lower"), (int)Variables.Constants.GetFloat("float_youth_academy_youth_counter_upper"));
        user.YouthAcademy.Players = new List<Player>();

        AddNewPlayer(user.YouthAcademy);
    }

    /// <summary>
    /// Add a new player to the academy based on the average skill value of the team,
    /// the average percentage the user paid for the academy for a period of time and
    /// the average youth coach quality.
    /// Corresponds to C: youth_academy_add_new_player.
    /// </summary>
    /// <param name="youthAcademy">The youth academy.</param>
    public static void AddNewPlayer(YouthAcademy youthAcademy)
    {
        float[] posProbs = new float[4]
        {
            Variables.Constants.GetFloat("float_youth_academy_pos_goalie"),
            Variables.Constants.GetFloat("float_youth_academy_pos_defender"),
            Variables.Constants.GetFloat("float_youth_academy_pos_midfielder"),
            Variables.Constants.GetFloat("float_youth_academy_pos_forward")
        };
        float rndom = Misc.MathRndi(0, (int)posProbs[3]);
        int newPos = 0;

        for (int i = 0; i < 4; i++)
        {
            if (i == youthAcademy.PosPref)
                posProbs[i] += Variables.Constants.GetFloat("float_youth_academy_coach_search_addition") * (float)(Quality.QualityEnd - youthAcademy.Coach);
            if (i > 0)
                posProbs[i] += posProbs[i - 1];
        }

        rndom = Misc.MathRndi(0, (int)posProbs[3]);
        for (int i = 0; i < 4; i++)
        {
            if (rndom <= (int)posProbs[i])
            {
                newPos = i;
                break;
            }
        }

        Player newPlayer = new Player();
        newPlayer.Name = youthAcademy.Team.NamesFile;
        newPlayer.Id = Bygfoot.PlayerIdNew();
        newPlayer.Pos = (PlayerPos)newPos;
        newPlayer.Cpos = (PlayerPos)newPos;
        newPlayer.Age = Misc.MathRndi((int)Variables.Constants.GetFloat("float_youth_academy_age_lower"), (int)Variables.Constants.GetFloat("float_youth_academy_age_upper"));
        newPlayer.PeakAge = Misc.MathRndi((int)(Variables.Constants.GetFloat("float_player_peak_age_lower") + (newPos == (int)PlayerPos.Goalie ? Variables.Constants.GetFloat("float_player_peak_age_goalie_addition") : 0)), (int)(Variables.Constants.GetFloat("float_player_peak_age_upper") + (newPos == (int)PlayerPos.Goalie ? Variables.Constants.GetFloat("float_player_peak_age_goalie_addition") : 0)));
        newPlayer.Streak = PlayerStreak.None;
        newPlayer.StreakCount = 0;
        newPlayer.StreakProb = 0;
        newPlayer.Team = youthAcademy.Team;

        youthAcademy.Players.Add(newPlayer);
    }

    /// <summary>
    /// Update the users' youth academies.
    /// Corresponds to C: youth_academy_update_weekly.
    /// </summary>
    public static void UpdateWeekly()
    {
        for (int i = 0; i < Variables.Users.Count; i++)
        {
            User user = Variables.Users[i];
            YouthAcademy ya = user.YouthAcademy;

            if (Variables.Options.GetInt("int_opt_disable_ya") != 0)
                continue;

            for (int j = ya.Players.Count - 1; j >= 0; j--)
            {
                Player player = ya.Players[j];
                Player.UpdateWeekly(player);

                if (player.Fitness < 0.9f)
                    Player.UpdateFitness(player);
                else
                {
                    player.Fitness += Misc.MathRndi(-50, 50) / 1000f;
                    player.Fitness = Math.Min(player.Fitness, 1);
                }

                if (player.Age + 0.1 > Variables.Constants.GetFloat("float_player_age_lower") &&
                    player.Age + 0.08 <= Variables.Constants.GetFloat("float_player_age_lower") &&
                    Variables.Options.GetInt("int_opt_goto_mode") == 0)
                {
                    User.EventAdd(user, (int)EventType.Warning, -1, -1, null, "Youth " + player.Name + " will be too old for the youth academy soon. Move him to your team or kick him out of the academy. Otherwise he'll probably look for another team to play in.", null);
                }
                else if (player.Age > Variables.Constants.GetFloat("float_player_age_lower"))
                {
                    if (Variables.Options.GetInt("int_opt_goto_mode") == 0)
                    {
                        User.EventAdd(user, (int)EventType.Warning, -1, -1, null, "Youth " + player.Name + " thought he's old enough for a real contract and left your youth academy.", null);
                    }

                    ya.Players.Remove(player);
                }
            }
        }
    }
}

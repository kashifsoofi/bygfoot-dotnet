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

/// <summary>
/// Static class to handle the betting functions. Corresponds to the C module src/bet.h and src/bet.c
/// (the bet_from_fixture, bet_update_user_bets, bet_get_odds, bet_update, bet_is_user, bet_place and
/// bet_remove functions).
/// </summary>
public static class Bet
{
    /// <summary>
    /// Round a betting odd to a multiple of 0.05. Corresponds to C: bet_round_odd.
    /// </summary>
    /// <param name="odd">The odd to round.</param>
    /// <returns>The rounded odd.</returns>
    public static float RoundOdd(float odd)
    {
        int localOdd = (int)Math.Round(odd * 100);

        if (localOdd % 5 == 0)
            return odd;

        if (localOdd % 5 >= 3)
            return (float)((localOdd + (5 - localOdd % 5)) / 100);

        return (float)((localOdd - localOdd % 5) / 100);
    }

    /// <summary>
    /// Find the bet containing the fixture. Corresponds to C: bet_from_fixture.
    /// </summary>
    /// <param name="fixId">The fixture id.</param>
    /// <returns>The bet or null.</returns>
    public static BetMatch FromFixture(int fixId)
    {
        // Implementation depends on the not yet converted fixtures and bets functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Manage the bets made by the users. Corresponds to C: bet_update_user_bets.
    /// </summary>
    public static void UpdateUserBets()
    {
        foreach (User user in Variables.Users)
        {
            user.Bets[0].Clear();
            user.Bets[0] = new List<BetUser>();

            foreach (BetUser bet in user.Bets[1])
            {
                BetMatch betMatch = FromFixture(bet.FixId);
                Fixture fix = null;

                if (fix != null)
                {
                    int outcome = fix.Result[0, 0] < fix.Result[1, 0] ? 2 : (fix.Result[0, 0] == fix.Result[1, 0] ? 1 : 0);

                    if (outcome == bet.Outcome)
                    {
                        user.Money += (int)Math.Round((float)bet.Wager * betMatch.Odds[outcome]);
                        user.MoneyIn[1, 4] += (int)Math.Round((float)bet.Wager * betMatch.Odds[outcome]);
                        bet.Wager = (int)Math.Round((float)bet.Wager * betMatch.Odds[outcome]);
                    }
                    else
                    {
                        user.Money -= bet.Wager;
                        user.MoneyOut[1, 9] -= bet.Wager;
                        bet.Wager *= -1;
                    }

                    user.Bets[0].Add(bet);
                }
            }

            user.Bets[1].Clear();
            user.Bets[1] = new List<BetUser>();
        }
    }

    /// <summary>
    /// Calculate the odds for the bet. Corresponds to C: bet_get_odds.
    /// </summary>
    /// <param name="bet">The bet.</param>
    public static void GetOdds(BetMatch bet)
    {
        // Implementation depends on the not yet converted fixture, player and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Write the bets for the current week round. Corresponds to C: bet_update.
    /// </summary>
    public static void Update()
    {
        // Implementation depends on the not yet converted fixture functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the user bet if the user betted on the bet or null. Corresponds to C: bet_is_user.
    /// </summary>
    /// <param name="bet">The bet.</param>
    /// <returns>The user bet or null.</returns>
    public static BetUser IsUser(BetMatch bet)
    {
        User user = Variables.Users[Variables.CurUser];

        for (int i = 1; i >= 0; i--)
        {
            foreach (BetUser userBet in user.Bets[i])
            {
                if (userBet.FixId == bet.FixId)
                    return userBet;
            }
        }

        return null;
    }

    /// <summary>
    /// Place a new bet. Corresponds to C: bet_place.
    /// </summary>
    /// <param name="fixId">The fixture id.</param>
    /// <param name="outcome">The outcome.</param>
    /// <param name="wager">The wager.</param>
    /// <returns>TRUE on success, FALSE otherwise.</returns>
    public static bool Place(int fixId, int outcome, int wager)
    {
        // Implementation depends on the not yet converted finance, game gui and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove the bet on the given fixture. Corresponds to C: bet_remove.
    /// </summary>
    /// <param name="fixId">The fixture id.</param>
    public static void Remove(int fixId)
    {
        User user = Variables.Users[Variables.CurUser];

        for (int i = 0; i < user.Bets[1].Count; i++)
        {
            if (user.Bets[1][i].FixId == fixId)
            {
                user.Bets[1].RemoveAt(i);
                return;
            }
        }
    }
}

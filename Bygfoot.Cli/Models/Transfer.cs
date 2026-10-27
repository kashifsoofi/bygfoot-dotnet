namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Structure representing a player on the transfer list. Corresponds to C Transfer struct.
/// </summary>
public class Transfer
{
    /// <summary>Team of the player. Corresponds to C: Team *tm.</summary>
    public Team Tm { get; set; }

    /// <summary>Id of player in the team. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>Time until player gets removed from the list. Corresponds to C: gint time.</summary>
    public int Time { get; set; }

    /// <summary>Estimated fees and wages for different scout qualities. Corresponds to C: gint fee[QUALITY_END], wage[QUALITY_END].</summary>
    public int[] Fee { get; set; }

    /// <summary>Estimated fees and wages for different scout qualities. Corresponds to C: gint fee[QUALITY_END], wage[quality_end].</summary>
    public int[] Wage { get; set; }

    /// <summary>Offers for the player. Corresponds to C: GArray *offers.</summary>
    public List<TransferOffer> Offers { get; set; } = new List<TransferOffer>();

    /// <summary>
    /// Weekly update of the transfer list.
    /// Corresponds to C: transfer_update.
    /// </summary>
    public static void TransferUpdate()
    {
        if ((Variables.Week > TransferGetDeadline() || Variables.Options.GetInt("int_opt_disable_transfers") != 0) && Variables.TransferList.Count == 0)
            return;

        if (Variables.Week >= TransferGetDeadline())
        {
            for (int i = Variables.TransferList.Count - 1; i >= 0; i--)
            {
                if (Variables.TransferList[i].Offers.Count == 0)
                    TransferRemovePlayer(i);
            }

            if (Variables.TransferList.Count == 0)
                return;
        }

        for (int i = Variables.TransferList.Count - 1; i >= 0; i--)
        {
            Transfer trans = Variables.TransferList[i];
            for (int j = trans.Offers.Count - 1; j >= 0; j--)
            {
                TransferOffer offer = trans.Offers[j];
                if (offer.Status == TransferOfferStatus.Rejected2 || Team.IsUser(offer.Tm) != -1)
                    trans.Offers.RemoveAt(j);
                else if (offer.Status == TransferOfferStatus.Rejected)
                    offer.Status = TransferOfferStatus.Rejected2;
            }
        }

        if (Variables.Week < TransferGetDeadline())
        {
            TransferAddNewPlayers();
            TransferAddCpuOffers();
        }

        TransferEvaluateOffers();

        for (int i = Variables.TransferList.Count - 1; i >= 0; i--)
        {
            Transfer trans = Variables.TransferList[i];
            if (Team.IsUser(trans.Tm) == -1 && trans.Offers.Count == 0)
            {
                trans.Time--;
                if (trans.Time == 0)
                    TransferRemovePlayer(i);
            }
        }
    }

    /// <summary>
    /// Add offers for the users' players on the list.
    /// We also sort the offers from other users.
    /// Corresponds to C: transfer_add_cpu_offers.
    /// </summary>
    public static void TransferAddCpuOffers()
    {
        // Implementation depends on the not yet converted player and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return a random team as a potential buyer for a user player.
    /// Corresponds to C: transfer_team_get_new.
    /// </summary>
    /// <returns>The new team.</returns>
    public static Team TransferTeamGetNew()
    {
        List<Team> leagueTeams = Team.GetSorted((t1, t2) => Team.CompareFunc(t1, t2, 2), 2, false);
        List<Team> cupTeams = Team.GetSorted((t1, t2) => Team.CompareFunc(t1, t2, 2), 2, true);
        Team returnValue = null;

        while (returnValue == null || Team.IsUser(returnValue) != -1)
        {
            int rndom = Misc.MathRndi(0, leagueTeams.Count + cupTeams.Count - 1);
            if (rndom < leagueTeams.Count)
                returnValue = leagueTeams[rndom];
            else
                returnValue = cupTeams[rndom - leagueTeams.Count];
        }

        return returnValue;
    }

    /// <summary>
    /// Compare function for transfer offers.
    /// Corresponds to C: transfer_offer_compare_func.
    /// </summary>
    /// <param name="tr1">The first transfer offer.</param>
    /// <param name="tr2">The second transfer offer.</param>
    /// <returns>The comparison result.</returns>
    public static int TransferOfferCompareFunc(TransferOffer tr1, TransferOffer tr2)
    {
        int returnValue;

        if ((int)tr1.Status >= (int)TransferOfferStatus.Rejected &&
            (int)tr2.Status >= (int)TransferOfferStatus.Rejected)
        {
            returnValue = 0;
        }
        else if ((int)tr1.Status >= (int)TransferOfferStatus.Rejected)
        {
            returnValue = 1;
        }
        else if ((int)tr2.Status >= (int)TransferOfferStatus.Rejected)
        {
            returnValue = -1;
        }
        else
        {
            returnValue = tr1.Fee != tr2.Fee
                ? Misc.MiscIntCompare(tr1.Fee, tr2.Fee)
                : Misc.MiscIntCompare(tr1.Wage, tr2.Wage);
        }

        return returnValue;
    }

    /// <summary>
    /// Send notification events about accepted transfer offers for user players.
    /// Corresponds to C: transfer_offers_notify.
    /// </summary>
    /// <param name="trans">The transfer.</param>
    /// <param name="sort">Whether to sort the offers.</param>
    public static void TransferOffersNotify(Transfer trans, bool sort)
    {
        if (sort)
            trans.Offers.Sort((o1, o2) => TransferOfferCompareFunc(o1, o2));

        if (trans.Offers.Count > 0)
            trans.Offers[0].Status = TransferOfferStatus.Accepted;

        if (trans.Tm != null)
        {
            int userIndex = Team.IsUser(trans.Tm);
            User user = userIndex != -1 ? Variables.Users[userIndex] : null;
            if (user != null)
            {
                User.EventAdd(user, (int)EventType.TransferOfferUser, -1, -1, null, trans.Offers[0].Tm?.Name ?? string.Empty, null);
            }
        }

        User.EventShowNext();
    }

    /// <summary>
    /// Move players to the users' teams if their offer was accepted or remove the players from the list if not.
    /// Corresponds to C: transfer_evaluate_offers.
    /// </summary>
    public static void TransferEvaluateOffers()
    {
        // Implementation depends on the not yet converted player and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add new players to the transfer list.
    /// Corresponds to C: transfer_add_new_players.
    /// </summary>
    public static void TransferAddNewPlayers()
    {
        // Implementation depends on the not yet converted team, player and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Select a random player for the transfer list.
    /// Corresponds to C: transfer_player_get_new.
    /// </summary>
    /// <param name="cup">Whether the player should be from an international cup.</param>
    /// <returns>The new player.</returns>
    public static Player TransferPlayerGetNew(bool cup)
    {
        // Implementation depends on the not yet converted team, player and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add the player to the transfer list.
    /// @param time The number of weeks he'll be on the list.
    /// Corresponds to C: transfer_add_player.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="time">The time.</param>
    public static void TransferAddPlayer(Player pl, int time)
    {
        // Implementation depends on the not yet converted player and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the transfer deadline.
    /// Corresponds to C: transfer_get_deadline.
    /// </summary>
    /// <returns>The deadline.</returns>
    public static int TransferGetDeadline()
    {
        // Implementation depends on the not yet converted league, cup and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove a player from the transfer list.
    /// Corresponds to C: transfer_remove_player.
    /// </summary>
    /// <param name="idx">The index.</param>
    public static void TransferRemovePlayer(int idx)
    {
        Variables.TransferList.RemoveAt(idx);
    }

    /// <summary>
    /// Remove a player given by a pointer from the list.
    /// Corresponds to C: transfer_remove_player_ptr.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void TransferRemovePlayerPtr(Player pl)
    {
        for (int i = 0; i < Variables.TransferList.Count; i++)
        {
            if (Variables.TransferList[i].Tm == pl.Team && Variables.TransferList[i].Id == pl.Id)
            {
                TransferRemovePlayer(i);
                break;
            }
        }
    }

    /// <summary>
    /// Find out whether a player is on the transfer list.
    /// Corresponds to C: query_transfer_player_is_on_list.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <returns>Whether the player is on the list.</returns>
    public static bool QueryTransferPlayerIsOnList(Player pl)
    {
        for (int i = 0; i < Variables.TransferList.Count; i++)
        {
            if (Variables.TransferList[i].Tm == pl.Team && Variables.TransferList[i].Id == pl.Id)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Check whether there is an offer by the current user for the player.
    /// Corresponds to C: query_transfer_current_team_offer.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <returns>Whether there's an offer.</returns>
    public static bool QueryTransferCurrentTeamOffer(Player pl)
    {
        // Implementation depends on the not yet converted user and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether a player is on the list and there are offers to buy him.
    /// Corresponds to C: transfer_player_has_offer.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <returns>The offer or null.</returns>
    public static TransferOffer TransferPlayerHasOffer(Player pl)
    {
        for (int i = 0; i < Variables.TransferList.Count; i++)
        {
            Transfer trans = Variables.TransferList[i];
            if (trans.Tm == pl.Team && trans.Id == pl.Id && trans.Offers.Count > 0 &&
                trans.Offers[0].Status == TransferOfferStatus.Accepted)
            {
                return trans.Offers[0];
            }
        }

        return null;
    }

    /// <summary>
    /// Add an offer to the player with specified index on the transfer list.
    /// @param return TRUE if the offer updated an earlier offer, FALSE, otherwise.
    /// Corresponds to C: transfer_add_offer.
    /// </summary>
    /// <param name="idx">The index.</param>
    /// <param name="tm">The team.</param>
    /// <param name="fee">The fee.</param>
    /// <param name="wage">The wage.</param>
    /// <returns>Whether the offer updated an earlier offer.</returns>
    public static bool TransferAddOffer(int idx, Team tm, int fee, int wage)
    {
        Transfer trans = Variables.TransferList[idx];

        for (int i = 0; i < trans.Offers.Count; i++)
        {
            if (trans.Offers[i].Tm == tm)
            {
                trans.Offers[i].Fee = fee;
                trans.Offers[i].Wage = wage;
                trans.Offers[i].Status = TransferOfferStatus.NotConsidered;
                return true;
            }
        }

        TransferOffer newOffer = new TransferOffer();
        newOffer.Tm = tm;
        newOffer.Fee = fee;
        newOffer.Wage = wage;
        newOffer.Status = TransferOfferStatus.NotConsidered;
        trans.Offers.Add(newOffer);

        return false;
    }

    /// <summary>
    /// Remove any offers from the team for the given transfer player.
    /// Corresponds to C: transfer_remove_offer.
    /// </summary>
    /// <param name="idx">The index.</param>
    /// <param name="tm">The team.</param>
    /// <returns>Whether an offer was removed.</returns>
    public static bool TransferRemoveOffer(int idx, Team tm)
    {
        Transfer trans = Variables.TransferList[idx];

        for (int i = 0; i < trans.Offers.Count; i++)
        {
            if (trans.Offers[i].Tm == tm)
            {
                trans.Offers.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Add or remove a user player from the transfer list.
    /// Corresponds to C: transfer_add_remove_user_player.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void TransferAddRemoveUserPlayer(Player pl)
    {
        // Implementation depends on the not yet converted player, team and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the current user has to do some transfer business before a user change or a new week.
    /// Corresponds to C: transfer_offers_pending.
    /// </summary>
    /// <returns>Whether there are pending offers.</returns>
    public static bool TransferOffersPending()
    {
        for (int i = 0; i < Variables.TransferList.Count; i++)
        {
            Transfer trans = Variables.TransferList[i];
            if ((trans.Tm == Variables.Users[Variables.CurUser].Team && trans.Offers.Count > 0 &&
                 trans.Offers[0].Status == TransferOfferStatus.Accepted) ||
                (Team.IsUser(trans.Tm) == -1 && trans.Offers.Count > 0 &&
                 trans.Offers[0].Tm == Variables.Users[Variables.CurUser].Team &&
                 trans.Offers[0].Status == TransferOfferStatus.Accepted))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Find out whether the team has already made an offer for the player
    /// and set the fee and wage accordingly.
    /// Corresponds to C: transfer_get_previous_offer.
    /// </summary>
    /// <param name="tr">The transfer.</param>
    /// <param name="tm">The team.</param>
    /// <param name="fee">The fee.</param>
    /// <param name="wage">The wage.</param>
    public static void TransferGetPreviousOffer(Transfer tr, Team tm, ref int fee, ref int wage)
    {
        for (int i = 0; i < tr.Offers.Count; i++)
        {
            if (tr.Offers[i].Tm == tm)
            {
                fee = tr.Offers[i].Fee;
                wage = tr.Offers[i].Wage;
                return;
            }
        }
    }

    /// <summary>
    /// Find out if a potential new star balks because there are already enough stars on the user team.
    /// Corresponds to C: transfer_new_star_balks.
    /// </summary>
    /// <param name="tr">The transfer.</param>
    /// <param name="troff">The offer.</param>
    /// <returns>Whether the star bails.</returns>
    public static bool TransferNewStarBalks(Transfer tr, TransferOffer troff)
    {
        // Implementation depends on the not yet converted player and maths functions.
        throw new NotImplementedException();
    }
}

/// <summary>
/// A structure representing a transfer offer. Corresponds to C TransferOffer struct.
/// </summary>
public class TransferOffer
{
    /// <summary>The team that makes the offer. Corresponds to C: Team *tm.</summary>
    public Team Tm { get; set; }

    /// <summary>Transfer fee and wage offer. Corresponds to C: gint fee, wage.</summary>
public int Fee { get; set; }
    public int Wage { get; set; }

    /// <summary>Whether the offer got accepted or rejected etc. Corresponds to C: gint status.</summary>
    public TransferOfferStatus Status { get; set; }
}

/// <summary>
/// Enumeration for the status of a transfer offer. Corresponds to C enum TransferOfferStatus.
/// </summary>
public enum TransferOfferStatus
{
    NotConsidered = 0, // TRANSFER_OFFER_NOT_CONSIDERED
    Accepted,          // TRANSFER_OFFER_ACCEPTED
    Rejected,          // TRANSFER_OFFER_REJECTED
    Rejected2,         // TRANSFER_OFFER_REJECTED2
    End                // TRANSFER_OFFER_END
}

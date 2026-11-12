namespace Bygfoot.Cli;

/// <summary>
/// Static class to handle the finance functions. Corresponds to the C module src/finance.h and src/finance.c
/// (the finance_update_user_weekly, finance_wage_unit, finance_team_drawing_credit_loan, finance_pay_loan,
/// finance_get_loan, finance_get_stadium_improvement_cost, finance_get_stadium_improvement_duration,
/// finance_assign_game_money, finance_update_current_interest, finance_calculate_alr_weekly_installment and
/// finance_calculate_alr_start_week functions).
/// </summary>
public static class Finance
{
    /// <summary>
    /// Weekly update of finances. Corresponds to C: finance_update_user_weekly.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void UpdateUserWeekly(User user)
    {
        int i;
        Team tm = user.Team;
        Team newTeam = null;
        float[] physioFactor =
        {
            Variables.Constants.GetFloat("float_finance_physio_factor1"),
            Variables.Constants.GetFloat("float_finance_physio_factor2"),
            Variables.Constants.GetFloat("float_finance_physio_factor3"),
            Variables.Constants.GetFloat("float_finance_physio_factor4")
        };
        float[] scoutFactor =
        {
            Variables.Constants.GetFloat("float_finance_scout_factor1"),
            Variables.Constants.GetFloat("float_finance_scout_factor2"),
            Variables.Constants.GetFloat("float_finance_scout_factor3"),
            Variables.Constants.GetFloat("float_finance_scout_factor4")
        };
        float[] ycFactor =
        {
            Variables.Constants.GetFloat("float_finance_yc_factor1"),
            Variables.Constants.GetFloat("float_finance_yc_factor2"),
            Variables.Constants.GetFloat("float_finance_yc_factor3"),
            Variables.Constants.GetFloat("float_finance_yc_factor4")
        };

        if (user.Money > int.MaxValue - 50000000)
        {
            User.EventAdd(user, (int)EventType.Charity, -1, -1, null, null);
            user.Money = (user.Money - user.Money % 2) / 2;
        }

        if (Variables.Week % 2 == 1)
        {
            for (i = 0; i < (int)MonOut.MonOutEnd; i++)
            {
                user.MoneyOut[0, i] = user.MoneyOut[1, i];
                user.MoneyOut[1, i] = 0;
            }
            for (i = 0; i < (int)MonIn.MonInEnd; i++)
            {
                user.MoneyIn[0, i] = user.MoneyIn[1, i];
                user.MoneyIn[1, i] = 0;
            }
        }

        user.Money += user.Sponsor.Benefit;
        user.MoneyIn[1, (int)MonIn.MonInSponsor] += user.Sponsor.Benefit;
        user.Sponsor.Contract = Math.Max(user.Sponsor.Contract - 1, 0);

        user.Money -= (int)Math.Round((double)user.Sponsor.Benefit *
            user.YouthAcademy.Percentage / 100);
        user.MoneyOut[1, (int)MonOut.MonOutYa] -= (int)Math.Round((double)user.Sponsor.Benefit *
            user.YouthAcademy.Percentage / 100);

        if (user.Counters[(int)CounterType.NewSponsor] > 1)
            user.Counters[(int)CounterType.NewSponsor]--;
        else if (user.Sponsor.Contract == 4 &&
            MathHelper.Rnd(0, 1) < ((1 - Variables.Constants.GetFloat("float_sponsor_continue_prob")) /
            (2 * Variables.Constants.GetInt("int_user_success_offer_limit")) *
            user.Counters[(int)CounterType.Success]) +
            (1 - (1 - Variables.Constants.GetFloat("float_sponsor_continue_prob")) / 2))
            user.Counters[(int)CounterType.NewSponsor] = -1;
        else if (user.Sponsor.Contract == 0)
            user.Counters[(int)CounterType.NewSponsor] = 1;

        if (Team.QueryTeamPlays(tm, Variables.Week - 1, 1))
            for (i = 0; i < tm.Players.Count; i++)
            {
                user.MoneyOut[1, (int)MonOut.MonOutWage] -= tm.Players[i].Wage;
                user.Money -= tm.Players[i].Wage;

                if (tm.Players[i].Health != PlayerInjury.None)
                {
                    int physioCost = (int)Math.Round(WageUnit(tm) * physioFactor[user.Physio % 10]);
                    user.Money -= physioCost;
                    user.MoneyOut[1, (int)MonOut.MonOutPhysio] -= physioCost;
                }
            }

        int scoutCost = (int)Math.Round(WageUnit(tm) * scoutFactor[user.Scout % 10]);
        user.MoneyOut[1, (int)MonOut.MonOutScout] -= scoutCost;
        user.Money -= scoutCost;

        if (user.YouthAcademy.Players.Count > 0)
        {
            int ycCost = (int)Math.Round(WageUnit(tm) * ycFactor[user.YouthAcademy.Coach % 10]);
            user.MoneyOut[1, (int)MonOut.MonOutYc] -= ycCost;
            user.Money -= ycCost;
        }

        user.Debt = (int)Math.Round((double)user.Debt * (1 + user.DebtInterest));

        if (user.AlrStartWeek != 0 && Variables.Week >= user.AlrStartWeek)
        {
            PayLoan(user, user.AlrWeeklyInstallment);
            if (user.Debt == 0)
            {
                user.AlrStartWeek =
                    user.AlrWeeklyInstallment = 0;
            }
        }

        if (user.Money < -TeamDrawingCreditLoan(user.Team, false) &&
            user.Counters[(int)CounterType.Positive] == -1 && Variables.DebugLevel < 50)
        {
            user.Counters[(int)CounterType.Overdrawn]++;
            if (user.Counters[(int)CounterType.Overdrawn] <=
                Variables.Constants.GetInt("int_finance_overdraw_limit"))
            {
                User.EventAdd(user, (int)EventType.Overdraw, user.Counters[(int)CounterType.Overdrawn], -1, null, null);
                user.Counters[(int)CounterType.Positive] =
                    Variables.Constants.GetInt("int_finance_overdraw_positive");
            }
        }
        else if (user.Counters[(int)CounterType.Positive] > -1)
            user.Counters[(int)CounterType.Positive]--;

        if (user.Counters[(int)CounterType.Loan] > -1)
            user.Counters[(int)CounterType.Loan]--;

        if (user.Counters[(int)CounterType.Loan] == 0 && Variables.DebugLevel < 50)
            User.EventAdd(user, (int)EventType.Warning, -1, -1, null, "You have to pay back your loan this week.");

        if (user.Counters[(int)CounterType.Positive] == 0 && Variables.DebugLevel < 50)
            User.EventAdd(user, (int)EventType.Warning, -1, -1, null, "Your bank account has to be above your drawing credit limit next week.");

        if (((user.Counters[(int)CounterType.Loan] == -1 && user.Debt != 0) ||
            (user.Counters[(int)CounterType.Positive] == -1 &&
            user.Money < -TeamDrawingCreditLoan(user.Team, false))) && Variables.DebugLevel < 50)
        {
            newTeam = Team.GetNew(tm, true);
            User.EventAdd(user, (int)EventType.FireFinance, -1, -1, newTeam, null);
        }
    }

    /// <summary>
    /// Return a base value for team finances. Scout wages etc. depend on this value. Corresponds to C: finance_wage_unit.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>The wage unit.</returns>
    public static float WageUnit(Team tm)
    {
        float sum = 0;

        for (int i = 0; i < tm.Players.Count; i++)
            sum += tm.Players[i].Wage;

        return sum / tm.Players.Count;
    }

    /// <summary>
    /// Calculate the drawing credit or maximum loan of a team based on player values and stadium data. Corresponds to C: finance_team_drawing_credit_loan.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="loan">Whether a loan is drawn.</param>
    /// <returns>The drawing credit or maximum loan.</returns>
    public static int TeamDrawingCreditLoan(Team tm, bool loan)
    {
        double sum = 0;

        for (int i = 0; i < tm.Players.Count; i++)
            sum += tm.Players[i].Value;

        sum = (sum * Variables.Constants.GetFloat("float_finance_credit_player_value_weight")) +
            ((float)tm.Stadium.Capacity * Math.Pow(tm.Stadium.Safety, Variables.Constants.GetFloat("float_finance_credit_stadium_safety_exponent")) * Variables.Constants.GetFloat("float_finance_credit_stadium_weight"));

        sum *= loan ? Variables.Constants.GetFloat("float_finance_credit_factor_loan") : Variables.Constants.GetFloat("float_finance_credit_factor_drawing");

        return (int)Math.Round((double)sum);
    }

    /// <summary>
    /// Contract a loan for the current user. Corresponds to C: finance_get_loan.
    /// </summary>
    /// <param name="value">The amount of money.</param>
    public static void GetLoan(int value)
    {
        User user = Variables.Users[Variables.CurUser];

        float debtOld = user.Debt;
        float debtNew = -value;

        user.Money += value;
        user.Debt = (int)(user.Debt - value);

        if (user.Counters[(int)CounterType.Loan] == -1)
        {
            user.Counters[(int)CounterType.Loan] = Variables.Constants.GetInt("int_finance_payback_weeks");
            user.DebtInterest = Variables.CurrentInterest;
        }
        else
        {
            /** Calculate new interest in a way that the user can't take unfair advantage of new market interest. */
            user.DebtInterest = (float)Math.Pow(
                (debtOld * Math.Pow(1 + user.DebtInterest, user.Counters[(int)CounterType.Loan]) + debtNew * Math.Pow(1 + Variables.CurrentInterest, user.Counters[(int)CounterType.Loan])) / user.Debt,
                (float)1 / user.Counters[(int)CounterType.Loan]) - 1;
        }

        Debug.DebugPrintMessage($"You have {user.Counters[(int)CounterType.Loan]} weeks to pay back your loan.");
    }

    /// <summary>
    /// Pay back some loan for the specified user. Corresponds to C: finance_pay_loan.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="value">The amount of money paid back.</param>
    public static void PayLoan(User user, int value)
    {
        int add = (int)Math.Round((float)value / (float)(-user.Debt) * Variables.Constants.GetInt("int_finance_payback_weeks"));

        if (value > -user.Debt)
            value = -user.Debt;

        user.Money -= value;
        user.Debt += value;

        if (user.Debt == 0)
        {
            user.Counters[(int)CounterType.Loan] = -1;
            user.AlrStartWeek = 0;
            user.AlrWeeklyInstallment = 0;
            Debug.DebugPrintMessage("You are free from debt.");
        }
        else
        {
            user.Counters[(int)CounterType.Loan] = Math.Min(user.Counters[(int)CounterType.Loan] + add, Variables.Constants.GetInt("int_finance_payback_weeks"));
            Debug.DebugPrintMessage($"You have {user.Counters[(int)CounterType.Loan]} weeks to pay back the rest of your loan.");
        }
    }

    /// <summary>
    /// Return the cost of a stadium improvement. Corresponds to C: finance_get_stadium_improvement_cost.
    /// </summary>
    /// <param name="value">The improvement value, either number of new seats or safety increase.</param>
    /// <param name="capacity">Whether seats are built or capacity increased.</param>
    /// <returns>The cost.</returns>
    public static int GetStadiumImprovementCost(float value, bool capacity)
    {
        float returnValue;

        if (capacity)
        {
            returnValue = WageUnit(Variables.Users[Variables.CurUser].Team) *
                (value / Variables.Constants.GetInt("int_stadium_improvement_base_seats")) *
                Variables.Constants.GetFloat("float_stadium_improvement_wage_unit_factor_seats");

            if (value - Variables.Constants.GetInt("int_stadium_improvement_base_seats") >= Variables.Constants.GetInt("int_stadium_improvement_max_discount_seats"))
                returnValue *= 1 - Variables.Constants.GetFloat("float_stadium_improvement_max_discount");
            else
                returnValue *= 1 - Variables.Constants.GetFloat("float_stadium_improvement_max_discount") * ((value - Variables.Constants.GetInt("int_stadium_improvement_base_seats")) / Variables.Constants.GetInt("int_stadium_improvement_max_discount_seats"));
        }
        else
        {
            returnValue = WageUnit(Variables.Users[Variables.CurUser].Team) *
                (value / Variables.Constants.GetFloat("float_stadium_improvement_base_safety")) *
                Variables.Constants.GetFloat("float_stadium_improvement_wage_unit_factor_safety");

            if (value - Variables.Constants.GetFloat("float_stadium_improvement_base_safety") >= Variables.Constants.GetFloat("float_stadium_improvement_max_discount_safety"))
                returnValue *= 1 - Variables.Constants.GetFloat("float_stadium_improvement_max_discount");
            else
                returnValue *= 1 - Variables.Constants.GetFloat("float_stadium_improvement_max_discount") * ((value - Variables.Constants.GetFloat("float_stadium_improvement_base_safety")) / Variables.Constants.GetFloat("float_stadium_improvement_max_discount_safety"));
        }

        return (int)Math.Round((double)returnValue);
    }

    /// <summary>
    /// Return the expected duration of a stadium improvement. Corresponds to C: finance_get_stadium_improvement_duration.
    /// </summary>
    /// <param name="value">The improvement value, either number of new seats or safety increase.</param>
    /// <param name="capacity">Whether seats are built or capacity increased.</param>
    /// <returns>The duration.</returns>
    public static int GetStadiumImprovementDuration(float value, bool capacity)
    {
        int returnValue;

        if (capacity)
            returnValue = (int)((value - 1) / Variables.Constants.GetInt("int_stadium_improvement_base_seats")) + 1;
        else
            returnValue = (int)(((int)Math.Round((double)value * 100) - 1) / (int)Math.Round((double)Variables.Constants.GetFloat("float_stadium_improvement_base_safety") * 100)) + 1;

        return returnValue;
    }

    /// <summary>
    /// Update the user's accounts depending on match type and attendance. Corresponds to C: finance_assign_game_money.
    /// </summary>
    /// <param name="fix">The fixture being examined.</param>
    public static void AssignGameMoney(Fixture fix)
    {
        int[] userIdx = { Team.IsUser(fix.Teams[0]), Team.IsUser(fix.Teams[1]) };
        float journeyFactor =
            (fix.Clid < Bygfoot.IdCupStart ||
             (fix.Clid >= Bygfoot.IdCupStart &&
              League.QueryLeagueCupHasProperty(fix.Clid, "national")))
            ? Variables.Constants.GetFloat("float_game_finance_journey_factor_national")
            : Variables.Constants.GetFloat("float_game_finance_journey_factor_international");
        int[] ticketIncome = { 0, 0 };

        if (fix.Clid >= Bygfoot.IdCupStart &&
            !Cup.CupFromClid(fix.Clid).Rounds[fix.Round].HomeAway)
        {
            ticketIncome[0] =
                ticketIncome[1] = (int)Math.Round((double)(fix.Attendance * fix.Teams[0].Stadium.TicketPrice / 2));
        }
        else
            ticketIncome[0] = (int)Math.Round((double)(fix.Attendance * fix.Teams[0].Stadium.TicketPrice));

        for (int i = 0; i < 2; i++)
        {
            if (userIdx[i] != -1)
            {
                User user = Variables.Users[userIdx[i]];
                user.Money += ticketIncome[i];
                user.MoneyIn[1, (int)MonIn.MonInTicket] += ticketIncome[i];

                user.Money -=
                    (int)Math.Round((double)ticketIncome[i] * user.YouthAcademy.Percentage / 100);
                user.MoneyOut[1, (int)MonOut.MonOutYa] -=
                    (int)Math.Round((double)ticketIncome[i] * user.YouthAcademy.Percentage / 100);

                if (i == 0 && Variables.DebugLevel < 50)
                {
                    fix.Teams[0].Stadium.Safety -=
                        (float)MathHelper.Rnd(Variables.Constants.GetFloat("float_game_stadium_safety_deterioration_lower"),
                                              Variables.Constants.GetFloat("float_game_stadium_safety_deterioration_upper"));
                    fix.Teams[0].Stadium.Safety = Math.Clamp(fix.Teams[0].Stadium.Safety, 0, 1);
                }

                if (i == 1 || !fix.HomeAdvantage)
                {
                    int journeyCost = (int)Math.Round(WageUnit(fix.Teams[i]) * journeyFactor);
                    user.MoneyOut[1, (int)MonOut.MonOutJourney] -= journeyCost;
                    user.Money -= journeyCost;
                }
            }
        }
    }

    /// <summary>
    /// Change the current interest on the market (random walk with three possibilities). Corresponds to C: finance_update_current_interest.
    /// </summary>
    public static void UpdateCurrentInterest()
    {
        Variables.CurrentInterest += Misc.MathRndi(-1, 1) * Variables.Constants.GetFloat("float_finance_interest_step");

        if (Variables.CurrentInterest < Variables.Constants.GetFloat("float_finance_interest_lower"))
            Variables.CurrentInterest = Variables.Constants.GetFloat("float_finance_interest_lower");
        else if (Variables.CurrentInterest > Variables.Constants.GetFloat("float_finance_interest_upper"))
            Variables.CurrentInterest = Variables.Constants.GetFloat("float_finance_interest_upper");
    }

    /// <summary>
    /// Calculate the weekly installment for an automatic loan repayment depending on the start week. Corresponds to C: finance_calculate_alr_weekly_installment.
    /// </summary>
    /// <param name="startWeek">The start week.</param>
    /// <returns>The weekly installment.</returns>
    public static int CalculateAlrWeeklyInstallment(int startWeek)
    {
        User user = Variables.Users[Variables.CurUser];
        float debtEnd;
        float interestFactor;
        float maxStartWeek;
        float installment;
        int weeklyInstallment;

        maxStartWeek = Math.Min(Variables.Week + user.Counters[(int)CounterType.Loan], Fixture.GetLastScheduledWeek());
        debtEnd = user.Debt * (float)Math.Pow(1 + user.DebtInterest, maxStartWeek - 1);
        interestFactor =
            ((float)Math.Pow(1 + user.DebtInterest, maxStartWeek - startWeek + 1) - 1) / user.DebtInterest;

        installment = -debtEnd / interestFactor;
        weeklyInstallment = (int)Math.Round((double)installment);

        return (weeklyInstallment > installment) ? weeklyInstallment : weeklyInstallment + 1;
    }

    /// <summary>
    /// Calculate the start week for an automatic loan repayment depending on the weekly installment. Corresponds to C: finance_calculate_alr_start_week.
    /// </summary>
    /// <param name="weeklyInstallment">The weekly installment.</param>
    /// <returns>The start week.</returns>
    public static int CalculateAlrStartWeek(int weeklyInstallment)
    {
        User user = Variables.Users[Variables.CurUser];
        int upper;
        int startWeek;
        int installment;

        upper = Math.Min(Variables.Week + user.Counters[(int)CounterType.Loan], Fixture.GetLastScheduledWeek());

        for (startWeek = Variables.Week + 1; startWeek <= upper; startWeek++)
        {
            installment = CalculateAlrWeeklyInstallment(startWeek);
            if (installment > weeklyInstallment)
                return startWeek - 1;
            else if (installment == weeklyInstallment)
                return startWeek;
        }

        return startWeek;
    }
}

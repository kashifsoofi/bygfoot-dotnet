namespace Bygfoot.Cli.Models;

using System;
using System.Collections.Generic;

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
        // Implementation depends on the not yet converted user, team, finance and user_event functions.
        throw new NotImplementedException();
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
        // Implementation depends on the not yet converted user, team, cup and finance functions.
        throw new NotImplementedException();
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
        // Implementation depends on the not yet converted fixture and finance functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculate the start week for an automatic loan repayment depending on the weekly installment. Corresponds to C: finance_calculate_alr_start_week.
    /// </summary>
    /// <param name="weeklyInstallment">The weekly installment.</param>
    /// <returns>The start week.</returns>
    public static int CalculateAlrStartWeek(int weeklyInstallment)
    {
        // Implementation depends on the not yet converted fixture and finance functions.
        throw new NotImplementedException();
    }
}

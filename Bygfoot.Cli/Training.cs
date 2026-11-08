namespace Bygfoot.Cli;

/// <summary>
/// Hotel quality of a training camp. Corresponds to C enum TrainingCampHotel in training_struct.h.
/// </summary>
public enum TrainingCampHotel
{
    TrainingCampHotelGood = 1,   // TRAINING_CAMP_HOTEL_GOOD
    TrainingCampHotelFirst,      // TRAINING_CAMP_HOTEL_FIRST
    TrainingCampHotelPremium,    // TRAINING_CAMP_HOTEL_PREMIUM
    TrainingCampHotelEnd         // TRAINING_CAMP_HOTEL_END
}

/// <summary>
/// Training camp calculations.
/// Corresponds to the C functions in src/training.h and src/training.c and the definitions in src/training_struct.h.
/// </summary>
public static class Training
{
    /// <summary>Corresponds to C: CAMP_SCALE_MIN.</summary>
    public const int CampScaleMin = 0;
    /// <summary>Corresponds to C: CAMP_SCALE_MAX.</summary>
    public const int CampScaleMax = 10;
    /// <summary>Corresponds to C: CAMP_SCALE_STEP.</summary>
    public const int CampScaleStep = 1;

    /// <summary>
    /// Calculate the effects of a training camp on all players of a team.
    /// Corresponds to C: calculateTrainingCamp(Team *current_team, gdouble value_training, gdouble value_recreation, gint number_camp).
    /// </summary>
    /// <param name="currentTeam">The team the players belong to.</param>
    /// <param name="valueTraining">The value of the training (0-10).</param>
    /// <param name="valueRecreation">The value of the recreation (0-10).</param>
    /// <param name="numberCamp">The number of training camps.</param>
    public static void CalculateTrainingCamp(Team currentTeam, double valueTraining, double valueRecreation, int numberCamp)
    {
        for (int count = 0; count < currentTeam.Players.Count; count++)
        {
            Player player = currentTeam.Players[count];

            float skillPoints = 0;
            float fitnessPoints = 0;
            int recoveryPoints = 0;

            if (((valueTraining > 0) || (valueRecreation > 0)) && (player.Health == PlayerInjury.None))
            {
                skillPoints = CalculateSkillPoints((int)valueTraining,
                                                   numberCamp,
                                                   player.Age,
                                                   player.Fitness,
                                                   player.Lsu);

                fitnessPoints = CalculateFitnessPoints((int)valueTraining,
                                                       (int)valueRecreation,
                                                       numberCamp,
                                                       player.Age);
                recoveryPoints = 0;
            }
            else
            {
                recoveryPoints = CalculateRecoveryPoints((int)valueRecreation,
                                                         numberCamp,
                                                         player.Age);
                skillPoints = 0;
                fitnessPoints = 0;
            }

            // Set new values
            if ((player.Health > 0) && (player.Recovery > 0))
            {
                // Health & recovery
                player.Recovery -= recoveryPoints;
            }
            else
            {
                // Skill
                if ((player.Skill + skillPoints) <= (player.Talent - 2))
                {
                    player.Skill += skillPoints;
                    player.Lsu = 0;

                    // Functions which are called during a skill-update by player_update_skill() in player.c
                    player.Cskill = Player.GetCskill(player, (int)player.Cpos, true);
                    player.Value = Player.AssignValue(player);

                    for (int i = 0; i < (int)Quality.QualityEnd; i++)
                        if (player.Skill > player.Etal[i])
                        {
                            Player.EstimateTalent(player);
                            break;
                        }
                }

                // Fitness
                player.Fitness -= fitnessPoints;
                if (player.Fitness > 1)
                    player.Fitness = 1;
                else if (player.Fitness < 0)
                    player.Fitness = 0;
            }
        }
    }

    /// <summary>
    /// Calculate the skill points gained from a training camp.
    /// Corresponds to C: calculate_skill_points(gint value_training, gint number_camp, gfloat age, gfloat fitness, gfloat lsu).
    /// </summary>
    /// <param name="valueTraining">The value of the training (0-10).</param>
    /// <param name="numberCamp">The number of training camps.</param>
    /// <param name="age">The age of the player.</param>
    /// <param name="fitness">The fitness of the player.</param>
    /// <param name="lsu">The load of the player.</param>
    /// <returns>The gained skill points.</returns>
    public static float CalculateSkillPoints(int valueTraining, int numberCamp, float age, float fitness, float lsu)
    {
        int trainingPoints = 0;
        float skillPoints = 0;
        int random = 0;

        // Calculat training points -> age
        if (age < 20)
            trainingPoints += 4;
        else if (age < 25)
            trainingPoints += 3;
        else if (age < 30)
            trainingPoints += 2;
        else
            trainingPoints += 1;

        // Calculat training points -> fittness
        if (fitness > 0.75)
            trainingPoints += 4;
        else if (fitness > 0.50)
            trainingPoints += 3;
        else if (fitness > 0.25)
            trainingPoints += 2;
        else
            trainingPoints += 1;

        // Calculat training points -> lsu
        if (lsu > 15)
            trainingPoints += 4;
        else if (lsu > 10)
            trainingPoints += 3;
        else if (lsu > 5)
            trainingPoints += 2;
        else
            trainingPoints += 1;

        // Calculate skill points
        random = RandomInt(1, 3);
        skillPoints = trainingPoints - random;

        if (skillPoints == 0)
            return skillPoints;
        else
        {
            skillPoints = (float)(skillPoints * valueTraining * numberCamp / 100.0);
            return skillPoints;
        }
    }

    /// <summary>
    /// Calculate the fitness points gained (or lost) from a training camp.
    /// Corresponds to C: calculate_fitness_points(gint value_training, gint value_recreation, gint number_camp, gfloat age).
    /// </summary>
    /// <param name="valueTraining">The value of the training (0-10).</param>
    /// <param name="valueRecreation">The value of the recreation (0-10).</param>
    /// <param name="numberCamp">The number of training camps.</param>
    /// <param name="age">The age of the player.</param>
    /// <returns>The gained (or lost) fitness points.</returns>
    public static float CalculateFitnessPoints(int valueTraining, int valueRecreation, int numberCamp, float age)
    {
        int trainingPoints = 0;
        float fitnessPoints = 0;
        int random = 0;

        if (valueRecreation > 8)
        {
            if (valueRecreation == 9)
            {
                // Calculate fitness points
                random = RandomInt(1, 2);
                fitnessPoints = (float)((0.02 * numberCamp) + (random / 100.0)) * -1;
            }
            else
            {
                // Calculate fitness points
                random = RandomInt(1, 2);
                fitnessPoints = (float)((0.04 * numberCamp) + (random / 100.0)) * -1;
            }
        }
        else
        {
            // Calculat fitness points -> age
            if (age < 20)
                trainingPoints += 1;
            else if (age < 25)
                trainingPoints += 2;
            else if (age < 30)
                trainingPoints += 3;
            else
                trainingPoints += 4;

            // Calculat fitness points -> value training
            switch (valueTraining)
            {
                case 2: trainingPoints += 1; break;
                case 3: trainingPoints += 1; break;
                case 4: trainingPoints += 2; break;
                case 5: trainingPoints += 2; break;
                case 6: trainingPoints += 3; break;
                case 7: trainingPoints += 3; break;
                case 8: trainingPoints += 4; break;
                case 9: trainingPoints += 4; break;
                case 10: trainingPoints += 5; break;
            }

            // Calculate fitness points
            random = RandomInt(1, 2);
            fitnessPoints = (((float)trainingPoints + random) / numberCamp) / 100;
        }

        return fitnessPoints;
    }

    /// <summary>
    /// Calculate the recovery points gained from a training camp.
    /// Corresponds to C: calculate_recovery_points(gint value_recreation, gint number_camp, gfloat age).
    /// </summary>
    /// <param name="valueRecreation">The value of the recreation (0-10).</param>
    /// <param name="numberCamp">The number of training camps.</param>
    /// <param name="age">The age of the player.</param>
    /// <returns>The gained recovery points (1 or 0).</returns>
    public static int CalculateRecoveryPoints(int valueRecreation, int numberCamp, float age)
    {
        int trainingPoints = 0;
        int recoveryPoints = 0;
        int random = 0;

        // Calculat recovery points -> age
        if (age < 20)
            trainingPoints += 4;
        else if (age < 25)
            trainingPoints += 3;
        else if (age < 30)
            trainingPoints += 2;
        else
            trainingPoints += 1;

        // Calculat recovery points -> recreation
        switch (valueRecreation)
        {
            case 10: trainingPoints += 4; break;
            case 9: trainingPoints += 3; break;
            case 8: trainingPoints += 2; break;
            case 7: trainingPoints += 1; break;
        }

        // Calculate recreation points
        random = RandomInt(1, 2);
        recoveryPoints = (trainingPoints + random) * numberCamp;

        if (recoveryPoints > 17)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// Get a random integer in [min, max].
    /// Corresponds to C: random_int(gint min, gint max).
    /// </summary>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns>A random integer in [min, max].</returns>
    public static int RandomInt(int min, int max)
    {
        return Variables.RandGenerator.Next(min, max + 1); // random() works until max -1
    }

    /// <summary>
    /// Subtract the costs of a training camp from the current user's finances.
    /// Corresponds to C: calculateCostsTrainingCamp(gint number_camp).
    /// </summary>
    /// <param name="numberCamp">The number of training camps (1-3).</param>
    public static void CalculateCostsTrainingCamp(int numberCamp)
    {
        User user = Variables.Users[Variables.CurUser];
        float factor = Finance.WageUnit(user.Team);

        int[] costs = new int[3]
        {
            (int)(factor * OptionHelper.ConstFloat("float_training_camp_factor1")),
            (int)(factor * OptionHelper.ConstFloat("float_training_camp_factor2")),
            (int)(factor * OptionHelper.ConstFloat("float_training_camp_factor2"))
        };

        user.MoneyOut[0, (int)MonOut.MonOutTrainingCamp] -= costs[numberCamp - 1];
        user.Money -= costs[numberCamp - 1];
    }
}

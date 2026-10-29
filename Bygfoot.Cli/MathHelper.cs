namespace Bygfoot.Cli;

/// <summary>
/// Mathematical helper functions.
/// Corresponds to the C functions in src/maths.c and the macros in src/maths.h.
/// </summary>
public static class MathHelper
{
    /// <summary>Internal variable for the Gauss random generator.</summary>
    private static double gaussV1 = 0;

    /// <summary>Internal variable for the Gauss random generator.</summary>
    private static double gaussV2 = 0;

    /// <summary>Internal variable for the Gauss random generator.</summary>
    private static double gaussS = 0;

    /// <summary>Internal phase variable for the Gauss random generator.</summary>
    private static int gaussPhase = 0;

    /// <summary>
    /// Generate a Gauss-distributed (pseudo)random number.
    /// "By Box and Muller, and recommended by Knuth".
    /// @return A Gauss-distributed random number.
    /// </summary>
    public static double GaussRand()
    {
        double x = 0;

        if (gaussPhase == 0)
        {
            double u1 = Variables.RandGenerator.NextDouble();
            double u2 = Variables.RandGenerator.NextDouble();

            gaussV1 = 2 * u1 - 1;
            gaussV2 = 2 * u2 - 1;
            gaussS = gaussV1 * gaussV1 + gaussV2 * gaussV2;

            while (gaussS >= 1 || gaussS == 0)
            {
                u1 = Variables.RandGenerator.NextDouble();
                u2 = Variables.RandGenerator.NextDouble();
                gaussV1 = 2 * u1 - 1;
                gaussV2 = 2 * u2 - 1;
                gaussS = gaussV1 * gaussV1 + gaussV2 * gaussV2;
            }

            x = gaussV1 * Math.Sqrt(-2 * Math.Log(gaussS) / gaussS);
        }
        else
        {
            x = gaussV2 * Math.Sqrt(-2 * Math.Log(gaussS) / gaussS);
        }

        gaussPhase = 1 - gaussPhase;

        return x;
    }

    /// <summary>
    /// Generate a Gauss-distributed random number within given boundaries
    /// using GaussRand().
    /// Expectation value of the distribution is (upper + lower) / 2,
    /// the variance is so that the number is between the boundaries with probability
    /// 99,7 %. If the number isn't between the boundaries, we cut off.
    /// @param lower Lower cutoff boundary.
    /// @param upper Upper cutoff boundary.
    /// @return A Gauss-distributed number
    /// </summary>
    public static double GaussDist(double lower, double upper)
    {
        double result = (upper - lower) / 6 * GaussRand() + (upper + lower) / 2;

        if (result < lower)
            result = lower;

        if (result > upper)
            result = upper;

        return result;
    }

    /// <summary>
    /// Get a certain part of an integer number.
    /// If 'place' is between 1 and 9, the 'place'th digit beginning
    /// from the right is returned, e.g. if the number = 1234 and
    /// place = 2, the function returns 3.
    /// If 'place' is between 10 and 19, say 10 + x, the first
    /// 'x' digits are returned, e.g. number = 8765 and place = 12 leads to
    /// return value 87.
    /// If 'place' is between 20 and 29, say 20 + x, the last
    /// 'x' digits are returned, e.g. number = 4869 and place = 22
    /// leads to return value 69.
    /// @param value The number which gets scrutinized.
    /// @param place The number telling the function which part of 'value' to return.
    /// @return A part of the integer 'value'.
    /// </summary>
    public static int GetPlace(int value, int place)
    {
        if (place < 10)
        {
            double pow = Math.Pow(10, place);
            return (int)((value % pow - value % (pow / 10)) / (pow / 10));
        }

        if (place < 20)
        {
            while (value >= Math.Pow(10, place % 10))
                value = (value - value % 10) / 10;

            return value;
        }

        return value % (int)Math.Pow(10, place % 10);
    }

    /// <summary>
    /// Round an integer with given precision.
    /// If places > 0, round with precision 'places', e.g.
    /// number = 124566 and places = 2 leads to return value
    /// 124600.
    /// If places < 0, precision is length of 'number' minus
    /// 'places', e.g. number = 654987 and places = -2 leads to return
    /// value 65000.
    /// @param number The number to be rounded.
    /// @param places The precision.
    /// @return The rounded integer.
    /// </summary>
    public static int RoundInteger(int number, int places)
    {
        int length = 0;
        double copy = (double)number;

        if (places > 0)
        {
            double pow = Math.Pow(10, places);
            return (int)(Math.Round((double)number / pow, MidpointRounding.AwayFromZero) * pow);
        }

        while (copy >= 1)
        {
            copy /= 10;
            length++;
        }

        double pow2 = Math.Pow(10, length + places);
        return (int)(Math.Round((double)number / pow2, MidpointRounding.AwayFromZero) * pow2);
    }

    /// <summary>
    /// Generate a permutation of integers and write it to 'array'.
    /// @param array The integer array we store the permutation in.
    /// It must have size at least end - start - 1.
    /// @param start The integer to start with.
    /// @param end The integer to end with.
    /// </summary>
    public static void GeneratePermutation(int[] array, int start, int end)
    {
        for (int i = start; i < end + 1; i++)
            array[i - start] = i;

        for (int i = 0; i < end - start; i++)
        {
            int j = Misc.MathRndi(i, end - start);
            int tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }

    /// <summary>
    /// This function tells us how many teams from 'number' teams
    /// have to be left away to obtain a power of 2.
    /// </summary>
    /// <param name="number">The number of teams.</param>
    /// <returns>The number of byes.</returns>
    public static int GetByeLen(int number)
    {
        int i = 0;
        for (i = 0; i < 20; i++)
        {
            if (Math.Pow(2, i) >= number)
                break;
        }

        return (int)Math.Pow(2, i) - number;
    }

    /// <summary>
    /// Return the sum of the integers in the array.
    /// @param array The integer array.
    /// @param max The size of the array.
    /// @return The sum of all the integers in the array.
    /// </summary>
    public static int SumIntArray(int[] array, int max)
    {
        int sum = 0;

        for (int i = 0; i < max; i++)
            sum += array[i];

        return sum;
    }

    /// <summary>
    /// Generate a random number in the range [lower, upper] (math_rnd).
    /// </summary>
    /// <param name="lower">The lower cutoff boundary.</param>
    /// <param name="upper">The upper cutoff boundary.</param>
    /// <returns>A random number in the given range.</returns>
    public static double Rnd(double lower, double upper)
    {
        return lower + (upper - lower) * Variables.RandGenerator.NextDouble();
    }

    /// <summary>
    /// Get a random integer in the range [lower, upper] (math_rndi).
    /// </summary>
    /// <param name="lower">The lower cutoff boundary.</param>
    /// <param name="upper">The upper cutoff boundary.</param>
    /// <returns>A random integer in the given range.</returns>
    public static int Rndi(int lower, int upper)
    {
        return Misc.MathRndi(lower, upper);
    }

    /// <summary>
    /// Generate a Gauss-distributed random number within given boundaries
    /// (math_gauss_disti).
    /// </summary>
    /// <param name="lower">The lower cutoff boundary.</param>
    /// <param name="upper">The upper cutoff boundary.</param>
    /// <returns>The rounded Gauss-distributed number.</returns>
    public static int GaussDisti(int lower, int upper)
    {
        return (int)Math.Round(GaussDist((double)lower - 0.499, (double)upper + 0.499), MidpointRounding.ToEven);
    }
}

namespace Bygfoot.Cli;

using System.Text;
using Models;

/// <summary>
/// Miscellaneous helper functions.
/// Corresponds to the C functions in src/misc.c and the macros in src/misc.h.
/// </summary>
public static class Misc
{
    /// <summary>Exit when the print_error function is called on a set error.
    /// Corresponds to EXIT_PRINT_ERROR in the ExitCodes enum in bygfoot.h.</summary>
    private const int ExitPrintError = 4;

    /// <summary>Convert an integer to a string.
    /// Corresponds to the C macro misc_int_to_char in misc.h.</summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The number as a string.</returns>
    public static string MiscIntToChar(int number)
    {
        return number.ToString();
    }

    /// <summary>Check whether the haystack string contains the needle string.
    /// Corresponds to the C macro query_misc_string_contains in misc.h.</summary>
    /// <param name="haystack">The string to look in.</param>
    /// <param name="needle">The substring to look for.</param>
    /// <returns>Whether haystack contains needle.</returns>
    public static bool QueryMiscStringContains(string haystack, string needle)
    {
        return haystack.Contains(needle);
    }

    /// <summary>Print the message of an error. If abortProgram is true the program exits.
    /// Corresponds to C: misc_print_error(GError **error, gboolean abort_program).</summary>
    /// <param name="error">The error to print (its message is shown).</param>
    /// <param name="abortProgram">Whether to exit the program after printing.</param>
    public static void MiscPrintError(Exception error, bool abortProgram)
    {
        if (error == null)
            return;

        DebugPrintMessage($"error message: {error.Message}");

        if (abortProgram)
            Environment.Exit(ExitPrintError);
    }

    /// <summary>Swap two integers.
    /// Corresponds to C: misc_swap_int(gint *first, gint *second).</summary>
    /// <param name="first">The first integer.</param>
    /// <param name="second">The second integer.</param>
    public static void MiscSwapInt(ref int first, ref int second)
    {
        int tmp = first;
        first = second;
        second = tmp;
    }

    /// <summary>Swap two object references.
    /// Corresponds to C: misc_swap_gpointer(gpointer *first, gpointer *second).</summary>
    /// <param name="first">The first reference.</param>
    /// <param name="second">The second reference.</param>
    public static void MiscSwapGpointer<T>(ref T first, ref T second)
    {
        T tmp = first;
        first = second;
        second = tmp;
    }

    /// <summary>Separate a string into a list of its whitespace-delimited words.
    /// Corresponds to C: misc_separate_strings(gchar *string).</summary>
    /// <param name="input">The input string (possibly containing white space).</param>
    /// <returns>A list containing the separate words.</returns>
    public static List<string> MiscSeparateStrings(string input)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(input))
            return result;

        int start = 0;
        while (start < input.Length && IsAsciiSpace(input[start]))
            start++;
        if (start >= input.Length)
        {
            DebugPrintMessage("misc_separate_strings: input string contains only white spaces");
            return result;
        }

        var word = new StringBuilder();
        for (int i = start; i <= input.Length; i++)
        {
            if (i < input.Length && !IsAsciiSpace(input[i]))
                word.Append(input[i]);
            else if (word.Length > 0)
            {
                result.Add(word.ToString());
                word.Clear();
            }
        }

        return result;
    }

    /// <summary>Randomise the elements of an object array.
    /// The original array is cleared (corresponds to g_ptr_array_free(array, TRUE)).
    /// Corresponds to C: misc_randomise_g_pointer_array(GPtrArray *array).</summary>
    /// <param name="array">The object array to randomise.</param>
    /// <returns>A new array with the same elements in random order.</returns>
    public static List<T> MiscRandomiseGPointerArray<T>(List<T> array)
    {
        var newArray = new List<T>(array.Count);

        var order = new int[array.Count];
        MathGeneratePermutation(order, 0, array.Count - 1);
        for (int i = 0; i < array.Count; i++)
            newArray.Add(array[order[i]]);

        array.Clear();
        return newArray;
    }

    /// <summary>Copy a list of objects into a new list.
    /// Corresponds to C: misc_copy_ptr_array(const GPtrArray *array).</summary>
    /// <param name="array">The list to copy (may be null).</param>
    /// <returns>A new list with the same elements, or null if array is null.</returns>
    public static List<T> MiscCopyPtrArray<T>(List<T> array)
    {
        if (array == null)
            return null;
        return new List<T>(array);
    }

    /// <summary>Print a thousands-grouped output of a number, like 2 234 345 instead of 2234345.
    /// Corresponds to C: misc_print_grouped_int(gint number, gchar *buf).</summary>
    /// <param name="number">The number to print.</param>
    /// <returns>The thousands-grouped number as a string.</returns>
    public static string MiscPrintGroupedInt(int number)
    {
        int absNumber = Math.Abs(number);
        int length = 0;
        double copy = absNumber;
        while (copy >= 1)
        {
            copy /= 10;
            length++;
        }

        if (length > 9)
            return number.ToString();

        var buf = new StringBuilder();
        for (int i = length; i > 0; i--)
        {
            buf.Append(MathGetPlace(absNumber, i));
            if (i > 1 && i % 3 == 1)
                buf.Append(' ');
        }

        if (number < 0)
            return "- " + buf.ToString();

        if (number == 0)
            return "0";

        return buf.ToString();
    }

    /// <summary>Check whether an item is in an array of integers.
    /// Corresponds to C: query_integer_is_in_array(gint item, gint *array, gint max).</summary>
    /// <param name="item">The item to look for.</param>
    /// <param name="array">The array of integers to look in.</param>
    /// <param name="max">The maximum index in the array to check.</param>
    /// <returns>Whether the item is in the array.</returns>
    public static bool QueryIntegerIsInArray(int item, int[] array, int max)
    {
        for (int i = 0; i < max; i++)
        {
            if (array[i] == item)
                return true;
        }
        return false;
    }

    /// <summary>Check whether an item is in an array of integers.
    /// Corresponds to C: query_misc_integer_is_in_g_array(gint item, GArray *array).</summary>
    /// <param name="item">The item to look for.</param>
    /// <param name="array">The array of integers to look in.</param>
    /// <returns>Whether the item is in the array.</returns>
    public static bool QueryMiscIntegerIsInGArray(int item, int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == item)
                return true;
        }
        return false;
    }

    /// <summary>Compare two integers.
    /// Corresponds to C: misc_int_compare(gint first, gint second).</summary>
    /// <param name="first">The first integer.</param>
    /// <param name="second">The second integer.</param>
    /// <returns>-1 if first &gt; second, 1 if first &lt; second, 0 otherwise.</returns>
    public static int MiscIntCompare(int first, int second)
    {
        if (first > second)
            return -1;
        if (first < second)
            return 1;
        return 0;
    }

    /// <summary>Compare two floating point numbers.
    /// Corresponds to C: misc_float_compare(gfloat first, gfloat second).</summary>
    /// <param name="first">The first number.</param>
    /// <param name="second">The second number.</param>
    /// <returns>-1 if first &gt; second, 1 if first &lt; second, 0 otherwise.</returns>
    public static int MiscFloatCompare(float first, float second)
    {
        if (first > second)
            return -1;
        if (first < second)
            return 1;
        return 0;
    }

    /// <summary>Strip the root definitions directory from a definitions directory path.
    /// Corresponds to C: misc_strip_definitions_root(gchar *directory).</summary>
    /// <param name="directory">The directory path to strip.</param>
    /// <returns>The path without the root definitions directory prefix, or "" if no root prefix matched.</returns>
    public static string MiscStripDefinitionsRoot(string directory)
    {
        foreach (string prefix in Variables.RootDefinitionsDirectories)
        {
            if (directory.StartsWith(prefix))
                return directory.Substring(prefix.Length);
        }
        return "";
    }

    /// <summary>Check whether the string starts with a string in the string array.
    /// Corresponds to C: query_misc_string_starts_with(const gchar *string, GList *list).</summary>
    /// <param name="str">The string to check.</param>
    /// <param name="list">The list of strings to look for.</param>
    /// <returns>Whether the string starts with any string in the list.</returns>
    public static bool QueryMiscStringStartsWith(string str, List<string> list)
    {
        foreach (string prefix in list)
        {
            if (str.StartsWith(prefix))
                return true;
        }
        return false;
    }

    /// <summary>Check whether a string is in an array of strings.
    /// Corresponds to C: query_misc_string_in_array(const gchar *string, const GPtrArray *array).</summary>
    /// <param name="str">The string to look for.</param>
    /// <param name="array">The array of strings to look in.</param>
    /// <returns>Whether the string is in the array.</returns>
    public static bool QueryMiscStringInArray(string str, List<string> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            if (array[i] == str)
                return true;
        }
        return false;
    }

    /// <summary>Get a player's age given year and month of birth.
    /// Corresponds to C: misc_get_age_from_birth(gint birth_year, gint birth_month).</summary>
    /// <param name="birthYear">The year of birth.</param>
    /// <param name="birthMonth">The month of birth.</param>
    /// <returns>The age (in years, as a float).</returns>
    public static double MiscGetAgeFromBirth(int birthYear, int birthMonth)
    {
        var current = DateTime.Now;
        var birth = new DateTime(birthYear, birthMonth, 15);
        return (current - birth).TotalDays / 365.25f;
    }

    /// <summary>Skip past leading whitespace.
    /// Corresponds to C: misc_skip_spaces(const gchar *string).</summary>
    /// <param name="s">The input string.</param>
    /// <param name="pos">The current position in the string.</param>
    /// <returns>The new position (past the leading whitespace).</returns>
    public static int MiscSkipSpaces(string s, int pos)
    {
        while (pos < s.Length && IsAsciiSpace(s[pos]))
            pos++;
        return pos;
    }

    /// <summary>Parse an integer from a string, e.g. "10".
    /// Corresponds to C: misc_parse_value(const gchar *string, gint *value).</summary>
    /// <param name="s">The input string.</param>
    /// <param name="pos">The current position in the string.</param>
    /// <param name="value">The parsed value.</param>
    /// <returns>The new position (past the parsed value).</returns>
    public static int MiscParseValue(string s, int pos, out int value)
    {
        pos = MiscSkipSpaces(s, pos);
        value = 0;
        while (pos < s.Length && char.IsDigit(s[pos]))
        {
            value = value * 10 + (s[pos] - '0');
            pos++;
        }
        return pos;
    }

    /// <summary>Parse a simple arithmetic expression, e.g. "1+2" or "10-2".
    /// Corresponds to C: misc_parse_expression(const gchar *string, gint *result).</summary>
    /// <param name="s">The input string.</param>
    /// <param name="pos">The current position in the string.</param>
    /// <param name="result">The evaluation result.</param>
    /// <returns>The new position (past the parsed expression).</returns>
    public static int MiscParseExpression(string s, int pos, out int result)
    {
        int value;
        bool loop = true;

        pos = MiscParseValue(s, pos, out value);
        result = value;

        while (loop)
        {
            pos = MiscSkipSpaces(s, pos);
            if (pos >= s.Length)
            {
                loop = false;
                continue;
            }
            switch (s[pos])
            {
                case '+':
                    pos = MiscParseValue(s, pos + 1, out value);
                    result += value;
                    break;
                case '-':
                    pos = MiscParseValue(s, pos + 1, out value);
                    result -= value;
                    break;
                default:
                    loop = false;
                    break;
            }
        }

        return pos;
    }

    /// <summary>Parse a comparison expression, e.g. "10&lt;2", "10&lt;=2", "10GE2", "10LE2", "10=2", "10!=2".
    /// Corresponds to C: misc_parse_comparison(const gchar *string, gint *result).</summary>
    /// <param name="s">The input string.</param>
    /// <param name="pos">The current position in the string.</param>
    /// <param name="result">The evaluation result (1 if true, 0 if false).</param>
    /// <returns>The new position (past the parsed comparison).</returns>
    public static int MiscParseComparison(string s, int pos, out int result)
    {
        int value;

        pos = MiscParseExpression(s, pos, out result);
        pos = MiscSkipSpaces(s, pos);
        if (pos >= s.Length)
            return pos;

        bool hasNext = pos + 1 < s.Length;
        switch (s[pos])
        {
            case '<':
                if (hasNext && s[pos + 1] == '=')
                {
                    pos = MiscParseExpression(s, pos + 2, out value);
                    result = result <= value ? 1 : 0;
                }
                else
                {
                    pos = MiscParseExpression(s, pos + 1, out value);
                    result = result < value ? 1 : 0;
                }
                break;
            case 'L':
                if (hasNext && s[pos + 1] == 'E')
                {
                    pos = MiscParseExpression(s, pos + 2, out value);
                    result = result <= value ? 1 : 0;
                }
                else
                {
                    pos = MiscParseExpression(s, pos + 1, out value);
                    result = result < value ? 1 : 0;
                }
                break;
            case '=':
                pos = MiscParseExpression(s, pos + 1, out value);
                result = result == value ? 1 : 0;
                break;
            case '>':
                if (hasNext && s[pos + 1] == '=')
                {
                    pos = MiscParseExpression(s, pos + 2, out value);
                    result = result >= value ? 1 : 0;
                }
                else
                {
                    pos = MiscParseExpression(s, pos + 1, out value);
                    result = result > value ? 1 : 0;
                }
                break;
            case 'G':
                if (hasNext && s[pos + 1] == 'E')
                {
                    pos = MiscParseExpression(s, pos + 2, out value);
                    result = result >= value ? 1 : 0;
                }
                else
                {
                    pos = MiscParseExpression(s, pos + 1, out value);
                    result = result > value ? 1 : 0;
                }
                break;
            case '!':
                if (hasNext && s[pos + 1] == '=')
                {
                    pos = MiscParseExpression(s, pos + 2, out value);
                    result = result != value ? 1 : 0;
                }
                break;
        }

        return pos;
    }

    /// <summary>Parse a condition joined with "and", e.g. "1 = 1 and 2 = 2".
    /// Corresponds to C: misc_parse_and(const gchar *string, gint *result).</summary>
    /// <param name="s">The input string.</param>
    /// <param name="pos">The current position in the string.</param>
    /// <param name="result">The evaluation result (1 if true, 0 if false).</param>
    /// <returns>The new position (past the parsed condition).</returns>
    public static int MiscParseAnd(string s, int pos, out int result)
    {
        int value;

        pos = MiscParseComparison(s, pos, out result);
        pos = MiscSkipSpaces(s, pos);
        while (pos + 2 < s.Length && s[pos] == 'a' && s[pos + 1] == 'n' && s[pos + 2] == 'd')
        {
            pos = MiscParseComparison(s, pos + 3, out value);
            result = result != 0 && value != 0 ? 1 : 0;
            pos = MiscSkipSpaces(s, pos);
        }

        return pos;
    }

    /// <summary>Parse a full condition, e.g. "(1=1 and 2=2) or 3=3".
    /// Corresponds to C: misc_parse(const gchar *string, gint *result).</summary>
    /// <param name="s">The input string.</param>
    /// <param name="pos">The current position in the string.</param>
    /// <param name="result">The evaluation result (1 if true, 0 if false).</param>
    /// <returns>The new position (past the parsed condition).</returns>
    public static int MiscParse(string s, int pos, out int result)
    {
        int value;

        pos = MiscParseAnd(s, pos, out result);
        pos = MiscSkipSpaces(s, pos);
        while (pos + 1 < s.Length && s[pos] == 'o' && s[pos + 1] == 'r')
        {
            pos = MiscParseAnd(s, pos + 2, out value);
            result = result != 0 || value != 0 ? 1 : 0;
            pos = MiscSkipSpaces(s, pos);
        }

        return pos;
    }

    /// <summary>Assign a new value to a string.
    /// Corresponds to C: misc_string_assign(gchar **string, const gchar *contents).</summary>
    /// <param name="current">The string to assign (replaced by contents).</param>
    /// <param name="contents">The new contents (if null the string is left unchanged).</param>
    public static void MiscStringAssign(ref string current, string contents)
    {
        if (contents == null)
            return;
        current = contents;
    }

    /// <summary>Choose a random string from a set of strings separated by the pipe symbol.
    /// Corresponds to C: misc_string_choose_random(gchar *string).</summary>
    /// <param name="input">The pipe-separated set of strings.</param>
    /// <returns>A randomly chosen string from the set.</returns>
    public static string MiscStringChooseRandom(string input)
    {
        string[] array = input.Split('|');
        return array[MathRndi(0, array.Length - 1)];
    }

    /// <summary>Replace a token in a string.
    /// Corresponds to C: misc_string_replace_token(gchar *string, const gchar *token, const gchar *replacement).</summary>
    /// <param name="input">The input string.</param>
    /// <param name="token">The token to look for.</param>
    /// <param name="replacement">The replacement (should not contain the token).</param>
    /// <returns>The string with the token replaced.</returns>
    public static string MiscStringReplaceToken(string input, string token, string replacement)
    {
        if (string.IsNullOrEmpty(token) || !input.Contains(token))
            return input;
        return input.Replace(token, replacement);
    }

    /// <summary>Replace expressions of the form "[2+2]" with their evaluated result, e.g. "[2+2]" becomes "4".
    /// Expressions of the form "[2<3]" (or with G/L/GE/LE) are evaluated to 1 or 0.
    /// Corresponds to C: misc_string_replace_expressions(gchar *string).</summary>
    /// <param name="input">The input string.</param>
    /// <returns>The string with all bracketed expressions replaced.</returns>
    public static string MiscStringReplaceExpressions(string input)
    {
        string s = input;
        while (s.LastIndexOf('[') >= 0)
        {
            int i = s.LastIndexOf('[');
            int j = s.IndexOf(']', i);
            if (j < 0)
            {
                if (Globals.CurrentDebugLevel > 200)
                    DebugPrintMessage($"misc_string_replace_expressions: found [ but no ]: {s}");
                return s;
            }

            string inner = s.Substring(i + 1, j - i - 1);
            string result;
            if (inner.Contains('|'))
            {
                result = MiscStringChooseRandom(inner);
            }
            else if (inner.Contains('<') || inner.Contains('>') || inner.Contains('=')
                || inner.Contains(" G ") || inner.Contains(" L ") || inner.Contains(" GE ") || inner.Contains(" LE "))
            {
                MiscParse(inner, 0, out int value);
                result = value.ToString();
            }
            else
            {
                MiscParseExpression(inner, 0, out int value);
                result = value.ToString();
            }

            s = s.Substring(0, i) + result + s.Substring(j + 1);
        }

        return s;
    }

    /// <summary>Replace all tokens in a string.
    /// Corresponds to C: misc_string_replace_tokens(gchar *string, GPtrArray **token_rep).</summary>
    /// <param name="input">The input string.</param>
    /// <param name="tokenRep">tokenRep[0] contains the tokens, tokenRep[1] the replacements.</param>
    /// <returns>The string with the tokens replaced.</returns>
    public static string MiscStringReplaceTokens(string input, List<List<string>> tokenRep)
    {
        for (int i = 0; i < tokenRep[0].Count; i++)
        {
            if (input.Contains(tokenRep[0][i]))
                input = MiscStringReplaceToken(input, tokenRep[0][i], tokenRep[1][i]);
        }
        return input;
    }

    /// <summary>Replace the string "(paren)" with a replacement string.
    /// Corresponds to C: misc_string_replace_parenthesised(gchar *string, gchar *paren, const gchar *replacement).</summary>
    /// <param name="input">The input string.</param>
    /// <param name="paren">The content of the parentheses to look for.</param>
    /// <param name="replacement">The replacement.</param>
    /// <returns>The string with the parenthesised part replaced.</returns>
    public static string MiscStringReplaceParenthesised(string input, string paren, string replacement)
    {
        return MiscStringReplaceToken(input, $"({paren})", replacement);
    }

    /// <summary>Get the innermost parenthesised part of a string, e.g. "1 = 1" from "(1 = 1)".
    /// Corresponds to C: misc_string_get_parenthesised(const gchar *string).</summary>
    /// <param name="input">The input string.</param>
    /// <returns>The content between the last opening and first closing parenthesis, or "" if there is none.</returns>
    public static string MiscStringGetParenthesised(string input)
    {
        int open = input.LastIndexOf('(');
        int close = input.IndexOf(')');
        if (open < 0 || close < 0 || close <= open + 1)
            return "";
        return input.Substring(open + 1, close - open - 1);
    }

    /// <summary>Parse a condition like "(1 = 1 and _player1_ &gt; 1) or 3 = 3" and evaluate it.
    /// Corresponds to C: misc_parse_condition(const gchar *condition, GPtrArray **token_rep).</summary>
    /// <param name="condition">The condition to parse.</param>
    /// <param name="tokenRep">tokenRep[0] contains the tokens, tokenRep[1] the replacements.</param>
    /// <returns>Whether the condition evaluates to true.</returns>
    public static bool MiscParseCondition(string condition, List<List<string>> tokenRep)
    {
        string buf = condition;

        while (buf.Contains('('))
        {
            string inner = MiscStringGetParenthesised(buf);
            if (inner.Length == 0)
                break;
            buf = MiscParseCondition(inner, tokenRep)
                ? MiscStringReplaceParenthesised(buf, inner, "1 = 1")
                : MiscStringReplaceParenthesised(buf, inner, "1 = 2");
        }

        string buf2;
        do
        {
            buf2 = buf;
            buf = MiscStringReplaceTokens(buf, tokenRep);
            buf = MiscStringReplaceExpressions(buf);
        } while (buf2 != buf);

        if (buf.Contains('_'))
            return false;

        MiscParse(buf, 0, out int result);
        return result != 0;
    }

    /// <summary>Add a token and its replacement to a token array.
    /// Corresponds to C: misc_token_add(GPtrArray **token_rep, gint token_idx, gchar *replacement).</summary>
    /// <param name="tokenRep">tokenRep[0] gets the token, tokenRep[1] the replacement.</param>
    /// <param name="tokenIdx">The index of the token in the global Tokens list.</param>
    /// <param name="replacement">The string to replace the token with.</param>
    public static void MiscTokenAdd(List<List<string>> tokenRep, int tokenIdx, string replacement)
    {
        tokenRep[0].Add(Variables.Tokens.List[tokenIdx].StringValue);
        tokenRep[1].Add(replacement);
    }

    /// <summary>Add a boolean token and its replacement (1 or 0) to a token array.
    /// Corresponds to C: misc_token_add_bool(GPtrArray **token_rep, gint token_idx, gboolean value).</summary>
    /// <param name="tokenRep">tokenRep[0] gets the token, tokenRep[1] the replacement.</param>
    /// <param name="tokenIdx">The index of the token in the global Tokens list.</param>
    /// <param name="value">The boolean value (replaced by 1 or 0).</param>
    public static void MiscTokenAddBool(List<List<string>> tokenRep, int tokenIdx, bool value)
    {
        tokenRep[0].Add(Variables.Tokens.List[tokenIdx].StringValue);
        tokenRep[1].Add(MiscIntToChar(value ? 1 : 0));
    }

    /// <summary>Remove all occurrences of a token from a token array.
    /// Corresponds to C: misc_token_remove(GPtrArray **token_rep, gint idx).</summary>
    /// <param name="tokenRep">tokenRep[0] contains the tokens, tokenRep[1] the replacements.</param>
    /// <param name="idx">The index of the token in the global Tokens list.</param>
    public static void MiscTokenRemove(List<List<string>> tokenRep, int idx)
    {
        for (int i = tokenRep[0].Count - 1; i >= 0; i--)
        {
            if (tokenRep[0][i] == Variables.Tokens.List[idx].StringValue)
            {
                tokenRep[0].RemoveAt(i);
                tokenRep[1].RemoveAt(i);
            }
        }
    }

    /// <summary>Replace all tokens (and bracketed expressions) in a string.
    /// Corresponds to C: misc_string_replace_all_tokens(GPtrArray **token_rep, const gchar *text_tokens, gchar *dest).</summary>
    /// <param name="tokenRep">tokenRep[0] contains the tokens, tokenRep[1] the replacements.</param>
    /// <param name="textTokens">The input string containing tokens.</param>
    /// <param name="dest">The string with all tokens replaced.</param>
    /// <returns>Whether no unreplaced tokens ("_..._") remain in the result.</returns>
    public static bool MiscStringReplaceAllTokens(List<List<string>> tokenRep, string textTokens, out string dest)
    {
        dest = textTokens;

        string buf;
        do
        {
            buf = dest;
            dest = MiscStringReplaceTokens(dest, tokenRep);
            dest = MiscStringReplaceExpressions(dest);
        } while (buf != dest);

        return !dest.Contains('_');
    }

    /// <summary>Compare two strings alphabetically.
    /// Corresponds to C: misc_alphabetic_compare(gconstpointer a, gconstpointer b).</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>-1 if a comes before b, 1 if a comes after b, 0 if they are equal.</returns>
    public static int MiscAlphabeticCompare(string a, string b)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        int len0 = a.Length;
        int len1 = b.Length;
        int maxlen = Math.Min(len0, len1);

        for (int i = 0; i < maxlen; i++)
        {
            int letter0 = AlphabetIndex(alphabet, a[i]);
            int letter1 = AlphabetIndex(alphabet, b[i]);
            if (letter0 < letter1)
                return -1;
            if (letter0 > letter1)
                return 1;
        }

        if (len0 != len1)
            return len0 < len1 ? -1 : 1;

        return 0;
    }

    // --- Private helpers ---

    /// <summary>Whether a character is an ASCII whitespace character (g_ascii_isspace).</summary>
    private static bool IsAsciiSpace(char c)
    {
        return c is ' ' or '\t' or '\n' or '\v' or '\f' or '\r';
    }

    /// <summary>Get a random integer in [lower, upper] (math_rndi).</summary>
    private static int MathRndi(int lower, int upper)
    {
        return Variables.RandGenerator.Next(lower, upper + 1);
    }

    /// <summary>Get the digit at the given place (1-based from the right) of an integer (math_get_place).</summary>
    private static int MathGetPlace(int value, int place)
    {
        if (place < 10)
        {
            int pow = (int)Math.Pow(10, place);
            return (value % pow - value % (pow / 10)) / (pow / 10);
        }
        if (place < 20)
        {
            while (value >= (int)Math.Pow(10, place % 10))
                value = (value - value % 10) / 10;
            return value;
        }
        return value % (int)Math.Pow(10, place % 10);
    }

    /// <summary>Generate a random permutation of [start, end] in the array (math_generate_permutation).</summary>
    private static void MathGeneratePermutation(int[] array, int start, int end)
    {
        for (int i = start; i < end + 1; i++)
            array[i - start] = i;

        for (int i = 0; i < end - start; i++)
            MiscSwapInt(ref array[i], ref array[MathRndi(i, end - start)]);
    }

    /// <summary>Get the index of a letter in the alphabet (non-lowercase letters map to 0, like in the C code).</summary>
    private static int AlphabetIndex(string alphabet, char c)
    {
        for (int i = 0; i < alphabet.Length; i++)
        {
            if (c == alphabet[i])
                return i;
        }
        return 0;
    }

    /// <summary>Print a debug message (debug_print_message).</summary>
    private static void DebugPrintMessage(string message)
    {
        Console.WriteLine(message);
    }
}

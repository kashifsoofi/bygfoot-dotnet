namespace Bygfoot.Cli.Tests.Unit;

using AwesomeAssertions;
using TUnit.Core;

/// <summary>
/// Unit tests for the pure math helpers.
/// </summary>
public class MathHelperTests
{
    /// <summary>
    /// math_round_integer treats "places" as relative to the digit count:
    /// a 4-digit number with places -2 rounds to the hundreds.
    /// </summary>
    [Test]
    public void RoundInteger_NegativePlaces_RoundsToDecade()
    {
        MathHelper.RoundInteger(1234, -2).Should().Be(1200);
        MathHelper.RoundInteger(1234, -3).Should().Be(1230);
        MathHelper.RoundInteger(1500, -3).Should().Be(1500);
        MathHelper.RoundInteger(999, -3).Should().Be(999);
    }

    /// <summary>
    /// Places 0 rounds to a power of ten above the magnitude (a 4-digit
    /// number rounds to the ten-thousands, so smaller values collapse to 0).
    /// </summary>
    [Test]
    public void RoundInteger_ZeroPlaces_RoundsAboveMagnitude()
    {
        MathHelper.RoundInteger(1234, 0).Should().Be(0);
        MathHelper.RoundInteger(123, 0).Should().Be(0);
        MathHelper.RoundInteger(987, -1).Should().Be(1000);
        MathHelper.RoundInteger(1249, -1).Should().Be(1000);
        MathHelper.RoundInteger(1249, -2).Should().Be(1200);
    }

    /// <summary>GetByeLen returns the byes needed to reach the next power of two.</summary>
    [Test]
    public void GetByeLen_PadsToNextPowerOfTwo()
    {
        MathHelper.GetByeLen(8).Should().Be(0);
        MathHelper.GetByeLen(6).Should().Be(2);
        MathHelper.GetByeLen(5).Should().Be(3);
        MathHelper.GetByeLen(1).Should().Be(0);
    }
}

/// <summary>
/// Unit tests for the misc token helpers.
/// </summary>
public class MiscTests
{
    /// <summary>MiscIntToChar is a plain decimal conversion.</summary>
    [Test]
    [Arguments(0, "0")]
    [Arguments(42, "42")]
    public void MiscIntToChar_ConvertsToDecimalString(int number, string expected)
    {
        // Arrange
        
        // Act
        var result = Misc.MiscIntToChar(number); 
        
        // Assert
        result.Should().Be(expected);
    }

    /// <summary>QueryMiscStringContains mirrors the C contains macro.</summary>
    [Test]
    [Arguments("hello world", "world", true)]
    [Arguments("hello world", "x", false)]
    public void QueryMiscStringContains_FindsSubstring(string haystack, string needle, bool expected)
    {
        // Arrange
        
        // Act
        var result = Misc.QueryMiscStringContains(haystack, needle);
        
        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// MiscParseCondition resolves tokens and evaluates the resulting
    /// numeric expression.
    /// </summary>
    [Test]
    [Arguments("{0}", 0, "{0}", "5", " > 4", true)]
    [Arguments("{0}", 0, "{0}", "5", " < 4", false)]
    public void MiscParseCondition_EvaluatesTokenisedExpression(string name, int intValue, string strValue, string replacement, string condition, bool expected)
    {
        // Arrange
        // C: the tokens option list is loaded from the support files at
        // startup; provide the single token this test needs directly.
        Variables.Tokens = new OptionList();
        Variables.Tokens.Add(name, intValue, strValue);

        List<List<string>> tokenRep = [[], []];
        Misc.MiscTokenAdd(tokenRep, 0, replacement);
        
        // Act
        var result = Misc.MiscParseCondition($"{name} {condition}", tokenRep);

        // Assert
        result.Should().Be(expected);
    }
}

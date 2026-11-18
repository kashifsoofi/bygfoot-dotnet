namespace Bygfoot.Cli.Tests.Integration;

using AwesomeAssertions;
using TUnit.Core;

/// <summary>
/// Golden field test: loads the shipped strategy_attack2.xml through the
/// same entry point the game uses (StrategyXml.StrategyXmlRead) and
/// asserts every field of the resulting Strategy, mirroring the C parser
/// semantics (xml_strategy_read). Pins the parser behavior, e.g. for the
/// eventual switch to XmlSerializer.
/// </summary>
[NotInParallel("strategy")]
public class StrategyFileFieldTests
{
    private static string Attack2Path =>
        Path.Combine(AppContext.BaseDirectory, "support_files", "strategy", "strategy_attack2.xml");

    /// <summary>Loads strategy_attack2.xml fresh and returns the strategy it appended.</summary>
    private static Strategy LoadAttack2(out int countBefore)
    {
        countBefore = Strategy.AllStrategies.Count;
        Strategy? loaded = StrategyXml.StrategyXmlRead(Attack2Path);
        loaded.Should().NotBeNull();
        Strategy.AllStrategies.Add(loaded!);
        Strategy.AllStrategies.Count.Should().Be(countBefore + 1);
        Strategy.AllStrategies[^1].Should().Be(loaded);
        return loaded!;
    }

    [Test]
    public void Attack2_StrategyXmlRead_LoadsAllFields()
    {
        // Arrange
        int countBefore = 0;

        // Act
        Strategy strategy = LoadAttack2(out countBefore);

        // Assert: top level
        strategy.Sid.Should().Be("attack2");
        strategy.Desc.Should().Be("343, attack, best players play");

        // C: priority is relative — the first strategy loaded gets the
        // file value, subsequent strategies accumulate on the previous one.
        int expectedPriority = countBefore == 0 ? 1 : Strategy.AllStrategies[^2].Priority + 1;
        strategy.Priority.Should().Be(expectedPriority);

        // Assert: prematch (lineup: best=1, weakest=2, fittest=3, unfittest=4; unset=-100)
        strategy.Prematch.Should().HaveCount(9);
        AssertPrematch(strategy.Prematch[0], "", [343, 433, 352, 442, 532],
            lineup: 1, minFitness: 0.75f, boost: 0, style: 1);
        AssertPrematch(strategy.Prematch[1], "_LAYERDIFF_ = 1 or _AVSKILLDIFF_ < -8", [532, 442, 352],
            lineup: -100, minFitness: 0f, boost: -100, style: -1);
        AssertPrematch(strategy.Prematch[2], "_LAYERDIFF_ > 1 or _AVSKILLDIFF_ < -14", [532, 442, 352],
            lineup: -100, minFitness: 0f, boost: -100, style: -2);
        AssertPrematch(strategy.Prematch[3], "_LAYERDIFF_ = -1 or _AVSKILLDIFF_ < -8", [],
            lineup: 3, minFitness: 0.75f, boost: -100, style: -100);
        AssertPrematch(strategy.Prematch[4], "_LAYERDIFF_ < -1 or _AVSKILLDIFF_ < -14", [],
            lineup: 3, minFitness: 0.75f, boost: -1, style: -100);
        AssertPrematch(strategy.Prematch[5], "_GOALSTOWIN_ = 2", [433, 343, 352],
            lineup: -100, minFitness: 0f, boost: -100, style: 1);
        AssertPrematch(strategy.Prematch[6], "_GOALSTOWIN_ > 2", [343, 433, 352],
            lineup: -100, minFitness: 0f, boost: 1, style: 2);
        AssertPrematch(strategy.Prematch[7], "_GOALSTOWIN_ = -2", [532, 442, 352],
            lineup: -100, minFitness: 0f, boost: -100, style: -1);
        AssertPrematch(strategy.Prematch[8], "_GOALSTOWIN_ < -2", [532, 442, 352],
            lineup: -100, minFitness: 0f, boost: -100, style: -2);

        // Assert: match actions (all must have a condition; unset boost/style=-100, unset sub=-1)
        strategy.MatchAction.Should().HaveCount(15);
        strategy.MatchAction.Select(a => a.Condition).Should().OnlyContain(c => !string.IsNullOrEmpty(c));
        AssertMatchAction(0, strategy.MatchAction[0],
            "_MI_ >= 40 and _MI_ < 65 and _GOALSTOWIN_ < -3", style: -1, boost: -100);
        AssertMatchAction(1, strategy.MatchAction[1],
            "_MI_ >= 50 and _MI_ < 75 and _GOALSTOWIN_ = -2", style: -1, boost: -100,
            subCond: "_SUBSLEFT_ > 1", inPos: 1, inProp: 3, outPos: 921, outProp: 2);
        AssertMatchAction(2, strategy.MatchAction[2],
            "_MI_ >= 75 and _GOALSTOWIN_ <= -3", style: -2, boost: -1,
            inPos: 3, inProp: 4, outPos: 921, outProp: 3);
        AssertMatchAction(3, strategy.MatchAction[3],
            "_MI_ >= 65 and _MI_ < 80 and _GOALSTOWIN_ <= -3", style: -2, boost: -100,
            inPos: 2, inProp: 4, outPos: 1, outProp: 3);
        AssertMatchAction(4, strategy.MatchAction[4],
            "_MI_ >= 40 and _MI_ < 65 and _GOALSTOWIN_ <= -3", style: -1, boost: -100);
        AssertMatchAction(5, strategy.MatchAction[5],
            "_MI_ >= 80 and _GOALSTOWIN_ = 3", style: 0, boost: -1,
            inPos: 1, inProp: 3, outPos: 921, outProp: 4);
        AssertMatchAction(6, strategy.MatchAction[6],
            "_MI_ >= 40 and _MI_ < 65 and _GOALSTOWIN_ > 3", style: 0, boost: -1,
            subCond: "_SUBSLEFT_ > 1", inPos: 2, inProp: 3, outPos: 921, outProp: 4);
        AssertMatchAction(7, strategy.MatchAction[7],
            "_MI_ >= 75 and _GOALSTOWIN_ = 2", style: 2, boost: 1,
            inPos: 3, inProp: 1, outPos: 921, outProp: 2);
        AssertMatchAction(8, strategy.MatchAction[8],
            "_MI_ >= 50 and _MI_ < 75 and _GOALSTOWIN_ = 2", style: 1, boost: -100,
            subCond: "_SUBSLEFT_ > 1", inPos: 3, inProp: 1, outPos: 921, outProp: 2);
        AssertMatchAction(9, strategy.MatchAction[9],
            "_MI_ >= 65 and _MI_ < 80 and _GOALSTOWIN_ = 3", style: 2, boost: 1,
            inPos: 3, inProp: 1, outPos: 921, outProp: 2);
        AssertMatchAction(10, strategy.MatchAction[10],
            "_MI_ >= 40 and _MI_ < 65 and _GOALSTOWIN_ = 3", style: 2, boost: -100,
            subCond: "_SUBSLEFT_ > 1", inPos: 3, inProp: 1, outPos: 921, outProp: 2);
        AssertMatchAction(11, strategy.MatchAction[11],
            "_MI_ < 40 and _GOALSTOWIN_ < -2", style: -1, boost: -100);
        AssertMatchAction(12, strategy.MatchAction[12],
            "_MI_ < 40 and _GOALSTOWIN_ < -3", style: -2, boost: -100);
        AssertMatchAction(13, strategy.MatchAction[13],
            "_MI_ < 40 and _GOALSTOWIN_ > 2", style: 1, boost: -100);
        AssertMatchAction(14, strategy.MatchAction[14],
            "_MI_ < 40 and _GOALSTOWIN_ > 3", style: 2, boost: -100);
    }

    private static void AssertPrematch(StrategyPrematch p, string condition, int[] formations,
        int lineup, float minFitness, int boost, int style)
    {
        p.Condition.Should().Be(condition);
        p.Formations.Should().Equal(formations);
        p.Lineup.Should().Be(lineup);
        p.MinFitness.Should().Be(minFitness);
        p.Boost.Should().Be(boost);
        p.Style.Should().Be(style);
    }

    /// <summary>Position codes: single position = digit (G=0,D=1,M=2,F=3); combined, e.g. "midfielder defender" = 921.</summary>
    private static void AssertMatchAction(int id, StrategyMatchAction a, string condition,
        int style, int boost, string subCond = "",
        int inPos = -1, int inProp = -1, int outPos = -1, int outProp = -1)
    {
        a.Id.Should().Be(id);
        a.Condition.Should().Be(condition);
        a.SubCondition.Should().Be(subCond);
        a.Style.Should().Be(style);
        a.Boost.Should().Be(boost);
        a.SubInPos.Should().Be(inPos);
        a.SubInProp.Should().Be(inProp);
        a.SubOutPos.Should().Be(outPos);
        a.SubOutProp.Should().Be(outProp);
    }
}

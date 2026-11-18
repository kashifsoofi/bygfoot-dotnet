namespace Bygfoot.Cli.Tests.Integration;

using AwesomeAssertions;
using TUnit.Core;

/// <summary>
/// Integration tests which exercise the shipped support files
/// (copied to the test output directory by the main project build).
/// </summary>
[NotInParallel("strategy")]
public class StrategyLoadTests
{
    /// <summary>
    /// Register the output directory's support_files tree in
    /// Variables.SupportDirectories, mirroring C main_init.
    /// </summary>
    private static void RegisterSupportFiles()
    {
        Variables.SupportDirectories.Clear();
        FileHelper.FileAddSupportDirectoryRecursive(
            Path.Combine(AppContext.BaseDirectory, "support_files") + Path.DirectorySeparatorChar);
    }

    /// <summary>The strategy support directory must be registered for lookup by suffix.</summary>
    [Test]
    public void SupportFiles_ContainStrategyDirectory()
    {
        // Arrange
        RegisterSupportFiles();
        
        // Act
        string? strategyDir = FileHelper.GetFirstSupportDirSuffix("strategy");
        
        // Assert
        strategyDir.Should().NotBeNull();
        Directory.Exists(strategyDir!).Should().BeTrue();
    }

    /// <summary>
    /// All shipped strategy_*.xml files parse and the primary prematch
    /// defaults (formations, boost, style, lineup, min fitness) get filled.
    /// </summary>
    [Test]
    public void StrategyLoadStrategies_LoadsShippedStrategies()
    {
        // Arrange
        RegisterSupportFiles();

        int before = Strategy.AllStrategies.Count;
        
        // Act
        StrategyXml.StrategyLoadStrategies();

        // Assert
        Strategy.AllStrategies.Count.Should().BeGreaterThan(before);
        foreach (Strategy strategy in Strategy.AllStrategies.Skip(before))
        {
            strategy.Sid.Should().NotBeNullOrEmpty();
            strategy.Prematch.Count.Should().BeGreaterThan(0);
            strategy.Prematch[0].Formations.Count.Should().BeGreaterThan(0);
        }
    }
}

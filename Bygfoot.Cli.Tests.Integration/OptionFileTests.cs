namespace Bygfoot.Cli.Tests.Integration;

using AwesomeAssertions;
using TUnit.Core;

/// <summary>
/// Integration tests for the option file loader (C file_load_opt_file).
/// </summary>
public class OptionFileTests
{
    /// <summary>
    /// The shipped bygfoot_help file contains 34 option rows, of which the
    /// names repeat (9 string_contrib_title, 25 string_contrib_entry).
    /// C appends every row unconditionally (g_array_append_val), so all 34
    /// must survive the load; the lookup is last-wins (C datalist).
    /// </summary>
    [Test]
    public void LoadOptFile_KeepsEveryRowInclDuplicateNames()
    {
        // Arrange
        string file = Path.Combine(AppContext.BaseDirectory, "support_files", "bygfoot_help");

        // Act
        OptionList list = new();
        FileHelper.LoadOptFile(file, list, false);

        // Assert
        list.List.Count.Should().Be(34);
        list.List.Count(o => o.Name == "string_contrib_title").Should().Be(9);
        list.List.Count(o => o.Name == "string_contrib_entry").Should().Be(25);

        // Last occurrence wins in the lookup, mirroring the C datalist build.
        list.GetString("string_contrib_entry").Should().Be("Michael Dücker and public domain");
    }
}

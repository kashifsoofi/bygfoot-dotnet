namespace Bygfoot.Cli.Models;

/// <summary>
/// Structure describing a commentary for the live game. 
/// Corresponds to C struct LGCommentary.
/// </summary>
public class LgCommentary
{
    /// <summary>The commentary text (possibly containing tokens). Corresponds to C: gchar *text.</summary>
    public string Text { get; set; }

    /// <summary>A condition (if not fulfilled, the commentary doesn't get shown). Corresponds to C: gchar *condition.</summary>
    public string Condition { get; set; }

    /// <summary>Priority of the commentary (compared to the other ones for the same event). Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>An id to keep track of already used commentaries in the live game. Corresponds to C: gint id.</summary>
    public int Id { get; set; }
}

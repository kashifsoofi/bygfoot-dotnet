namespace Bygfoot.Cli.Models;

/// <summary>
/// Struct used for having all the windows in one place.
/// Corresponds to C struct Windows in bygfoot.h.
/// </summary>
public class Windows
{
    /// <summary>Corresponds to C: GtkWidget *main.</summary>
    public object Main { get; set; }
    /// <summary>Corresponds to C: GtkWidget *startup.</summary>
    public object Startup { get; set; }
    /// <summary>Corresponds to C: GtkWidget *live.</summary>
    public object Live { get; set; }
    /// <summary>Corresponds to C: GtkWidget *warning.</summary>
    public object Warning { get; set; }
    /// <summary>Corresponds to C: GtkWidget *progress.</summary>
    public object Progress { get; set; }
    /// <summary>Corresponds to C: GtkWidget *digits.</summary>
    public object Digits { get; set; }
    /// <summary>Corresponds to C: GtkWidget *stadium.</summary>
    public object Stadium { get; set; }
    /// <summary>Corresponds to C: GtkWidget *job_offer.</summary>
    public object JobOffer { get; set; }
    /// <summary>Corresponds to C: GtkWidget *yesno.</summary>
    public object Yesno { get; set; }
    /// <summary>Corresponds to C: GtkWidget *options.</summary>
    public object Options { get; set; }
    /// <summary>Corresponds to C: GtkWidget *constants.</summary>
    public object Constants { get; set; }
    /// <summary>Corresponds to C: GtkWidget *font_sel.</summary>
    public object FontSel { get; set; }
    /// <summary>Corresponds to C: GtkWidget *file_chooser.</summary>
    public object FileChooser { get; set; }
    /// <summary>Corresponds to C: GtkWidget *contract.</summary>
    public object Contract { get; set; }
    /// <summary>Corresponds to C: GtkWidget *menu_player.</summary>
    public object MenuPlayer { get; set; }
    /// <summary>Corresponds to C: GtkWidget *menu_youth.</summary>
    public object MenuYouth { get; set; }
    /// <summary>Corresponds to C: GtkWidget *user_management.</summary>
    public object UserManagement { get; set; }
    /// <summary>Corresponds to C: GtkWidget *wdebug.</summary>
    public object WDebug { get; set; }
    /// <summary>Corresponds to C: GtkWidget *help.</summary>
    public object Help { get; set; }
    /// <summary>Corresponds to C: GtkWidget *transfer_dialog.</summary>
    public object TransferDialog { get; set; }
    /// <summary>Corresponds to C: GtkWidget *sponsors.</summary>
    public object Sponsors { get; set; }
    /// <summary>Corresponds to C: GtkWidget *mmatches.</summary>
    public object MMatches { get; set; }
    /// <summary>Corresponds to C: GtkWidget *bets.</summary>
    public object Bets { get; set; }
    /// <summary>Corresponds to C: GtkWidget *splash.</summary>
    public object Splash { get; set; }
    /// <summary>Corresponds to C: GtkWidget *training_camp.</summary>
    public object TrainingCamp { get; set; }
    /// <summary>Corresponds to C: GtkWidget *news.</summary>
    public object News { get; set; }
    /// <summary>Corresponds to C: GtkWidget *alr.</summary>
    public object Alr { get; set; }

    /// <summary>The position of the horizontal divider is stored here. Corresponds to C: gint paned_pos.</summary>
    public int PanedPos { get; set; }
}

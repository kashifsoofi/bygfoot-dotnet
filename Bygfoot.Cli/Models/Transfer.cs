namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Structure representing a player on the transfer list. Corresponds to C Transfer struct.
/// </summary>
public class Transfer
{
    /// <summary>Team of the player. Corresponds to C: Team *tm.</summary>
    public Team Tm { get; set; }

    /// <summary>Id of player in the team. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>Time until player gets removed from the list. Corresponds to C: gint time.</summary>
    public int Time { get; set; }

    /// <summary>Estimated fees and wages for different scout qualities. Corresponds to C: gint fee[QUALITY_END], wage[QUALITY_END].</summary>
    public int[] Fee { get; set; }

    /// <summary>Estimated fees and wages for different scout qualities. Corresponds to C: gint fee[QUALITY_END], wage[quality_end].</summary>
    public int[] Wage { get; set; }

    /// <summary>Offers for the player. Corresponds to C: GArray *offers.</summary>
    public List<TransferOffer> Offers { get; set; } = new List<TransferOffer>();
}

/// <summary>
/// A structure representing a transfer offer. Corresponds to C TransferOffer struct.
/// </summary>
public class TransferOffer
{
    /// <summary>The team that makes the offer. Corresponds to C: Team *tm.</summary>
    public Team Tm { get; set; }

    /// <summary>Transfer fee and wage offer. Corresponds to C: gint fee, wage.</summary>
public int Fee { get; set; }
    public int Wage { get; set; }

    /// <summary>Whether the offer got accepted or rejected etc. Corresponds to C: gint status.</summary>
    public TransferOfferStatus Status { get; set; }
}

/// <summary>
/// Enumeration for the status of a transfer offer. Corresponds to C enum TransferOfferStatus.
/// </summary>
public enum TransferOfferStatus
{
    NotConsidered = 0, // TRANSFER_OFFER_NOT_CONSIDERED
    Accepted,          // TRANSFER_OFFER_ACCEPTED
    Rejected,          // TRANSFER_OFFER_REJECTED
    Rejected2,         // TRANSFER_OFFER_REJECTED2
    End                // TRANSFER_OFFER_END
}

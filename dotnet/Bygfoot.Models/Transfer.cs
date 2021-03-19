using System;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	public enum TransferOfferStatus
	{
		TRANSFER_OFFER_NOT_CONSIDERED = 0,
		TRANSFER_OFFER_ACCEPTED,
		TRANSFER_OFFER_REJECTED,
		TRANSFER_OFFER_REJECTED2,
		TRANSFER_OFFER_END
	}

	public class TransferOffer
	{
		/** The team that makes the offer. */
		public Team tm;
		/** Transfer fee and wage offer. */
		public int fee, wage;
		/** Whether the offer got accepted or rejected etc. */
		public TransferOfferStatus status;
	}

	/** Structure representing a player on the transfer list. */
	public class Transfer
	{
		/** Team of the player. */
		public Team tm;
		/** Id of player in the team. */
		public int id;
		/** Time until player gets removed from the list. */
		public int time;
		/** Estimated fees and wages for different scout qualities. */
		public int[] fee = new int[(int)Quality.QUALITY_END];
		public int[] wage = new int[(int)Quality.QUALITY_END];
		/** Offers for the player. */
		public List<TransferOffer> offers;
	}
}

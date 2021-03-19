using System;
namespace Bygfoot.Model
{
	/** Structure describing a commentary
     * for the live game.  */
	public class LGCommentary
	{
		/** The commentary text (possibly containing tokens). */
		public string text;
		/** A condition (if not fulfilled, the commentary doesn't get shown). */
		public string condition;
		/** Priority of the commentary (compared to
             * the other ones for the same event).
             * The higher the priority the higher the
             * probability that the commentary gets picked. */
		public int priority;
		/** An id to keep track of already used commentaries in the
             * live game (so as not to use the same one too frequently). */
		public int id;
	}
}

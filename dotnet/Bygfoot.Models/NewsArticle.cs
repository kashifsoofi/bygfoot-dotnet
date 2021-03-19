using System;
using System.Collections;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	/** Enumeration of news article types. */
	public enum NewsArticleTypes
	{
		NEWS_ARTICLE_TYPE_MATCH = 0, /**< Article about a match. */
		NEWS_ARTICLE_TYPE_FINANCES, /**< Article about user finances. */
		NEWS_ARTICLE_TYPE_STAR_PLAYER_TRANSFER, /**< Article about good players appearing on the transfer list. */
		NEWS_ARTICLE_TYPE_LEAGUE_CHAMPION, /**< Article about teams clinching the championship. */
		NEWS_ARTICLE_TYPE_CUP_QUALIFICATION, /**< Article about teams clinching cup berths. */
		NEWS_ARTICLE_TYPE_RELEGATION, /**< Article about teams getting relegated. */
		NEWS_ARTICLE_TYPE_END
	}

	/** Structure containing a news title or subtitle with tokens.  */
	public class NewsText
	{
		public string text;
		/** Priority of the text (compared to
         * the other ones for the same article type).
         * The higher the priority the higher the
         * probability that the text gets picked. */
		public int priority;
		/** An id to keep track of already used texts
         * (so as not to use the same one too frequently). */
		public int id;
		/** A condition (if not fulfilled, the title or subtitle
         * doesn't get considered). */
		public string condition;
	}

	/** Structure describing a news paper article with tokens.  */
	public class NewsArticle
	{
		/** Possible article titles (possibly containing tokens). */
		public ArrayList titles;
		/** Possible article subtitles (possibly containing tokens). */
		public ArrayList subtitles;
		/** A condition (if not fulfilled, the article doesn't get shown). */
		public string condition;
		/** Priority of the article. */
		public int priority;
		/** An id to avoid repetitions. */
		public int id;
	}

	/** Structure holding an article without tokens (ie. the real deal that's displayed). */
	public class NewsPaperArticle
	{
		public int weekNumber, weekRoundNumber;
		public int titleId, subtitleId;
		public int clid, cupRound;
		public string titleSmall, title, subtitle;
		public int id;
		public int userIdx;
	}

	/** Structure holding the newspaper for the game. */
	public class NewsPaper
	{
		/** The array of created articles. */
		public List<NewsPaperArticle> articles;
	}
}

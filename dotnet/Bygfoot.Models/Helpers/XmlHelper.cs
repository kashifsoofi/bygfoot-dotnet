using System;
namespace Bygfoot.Helpers
{
	public class XmlHelper
	{
		public const string TAG_DEF_NAME = "name";
		public const string TAG_DEF_SHORT_NAME = "short_name";
		public const string TAG_DEF_SID = "sid";
		public const string TAG_DEF_SYMBOL = "symbol";
		public const string TAG_DEF_WEEK_GAP = "week_gap";
		public const string TAG_DEF_PROPERTY = "property";
		public const string TAG_DEF_YELLOW_RED = "yellow_red";
		public const string TAG_DEF_WEEK_BREAK = "break_in";
		public const string ATT_DEF_NAME_WEEK_BREAK_LENGTH = "length";
		public const string TAG_DEF_SKIP_WEEKS_WITH = "skip_weeks_with";

		/** Starting values for tag enums in the various xml loading source files. */
		public const int TAG_START_MISC = 1000;
		public const int TAG_START_LEAGUE = 2000;
		public const int TAG_START_CUP = 3000;
		public const int TAG_START_TEAMS = 4000;
		public const int TAG_START_FIXTURES = 5000;
		public const int TAG_START_TABLE = 6000;
		public const int TAG_START_USERS = 7000;
		public const int TAG_START_LIVE_GAME = 8000;
		public const int TAG_START_PLAYERS = 9000;
		public const int TAG_END_PLAYERS = 9900;
		public const int TAG_START_LEAGUE_STAT = 10000;
		public const int TAG_START_SEASON_STATS = 11000;
		public const int TAG_START_LEAGUES_CUPS = 12000;
		public const int TAG_START_TRANSFERS = 20000;
		public const int TAG_START_JOBS = 21000;
		public const int TAG_START_NEWS_PAPER = 22000;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
namespace Bygfoot.Model
{
	/** Events happening during a live game.  Some of these events should always
     * be ordered in blocks as they can occur.  For example LIVE_GAME_EVENT_POST
     * until LIVE_GAME_EVENT_CROSS_BAR is a group of actions that can all occur
     * in a scoring chance.  We pick one of these randomly in this case.
     * @see #LiveGameEvent
     * @see #LiveGameUnit */
	public enum LiveGameEventType
	{
		/** This is the 'main' event, nothing in
         * particular is happening; one of the teams
         * is in possession of the ball. */
		LIVE_GAME_EVENT_GENERAL = 0, /* 0 */
		LIVE_GAME_EVENT_START_MATCH, /* 1 */
		LIVE_GAME_EVENT_HALF_TIME, /* 2 */
		LIVE_GAME_EVENT_EXTRA_TIME, /* 3 */
		LIVE_GAME_EVENT_END_MATCH, /* 4 */
		LIVE_GAME_EVENT_LOST_POSSESSION, /* 5 */
		LIVE_GAME_EVENT_SCORING_CHANCE, /* 6 */
		LIVE_GAME_EVENT_HEADER, /* 7 */
		LIVE_GAME_EVENT_PENALTY, /* 8 */
		LIVE_GAME_EVENT_FREE_KICK, /* 9 */
		LIVE_GAME_EVENT_GOAL, /* 10 */
		LIVE_GAME_EVENT_OWN_GOAL, /* 11 */
								  /* Start missed scoring chance block */
		LIVE_GAME_EVENT_POST, /* 12 */
		LIVE_GAME_EVENT_MISS, /* 13 */
		LIVE_GAME_EVENT_SAVE, /* 14 */
		LIVE_GAME_EVENT_KEEPER_PUSHED_IN_CORNER, /* 15 */
		LIVE_GAME_EVENT_CROSS_BAR, /* 16 */
		LIVE_GAME_EVENT_PLAYER_PUSHED_IN_CORNER, /* 17 */
												 /* End missed scoring chance block */
		LIVE_GAME_EVENT_CORNER_KICK, /* 18 */
		LIVE_GAME_EVENT_FOUL, /* 19 */
		LIVE_GAME_EVENT_FOUL_YELLOW, /* 20 */
		LIVE_GAME_EVENT_FOUL_RED, /* 21 */
		LIVE_GAME_EVENT_FOUL_RED_INJURY, /* 22 */
		LIVE_GAME_EVENT_SEND_OFF, /* 23 */
		LIVE_GAME_EVENT_INJURY, /* 24 */
								/** An injury that permits the player to
								 * continue after some brief time. */
		LIVE_GAME_EVENT_TEMP_INJURY, /* 25 */
		LIVE_GAME_EVENT_PENALTIES, /* 26 */
		LIVE_GAME_EVENT_STADIUM, /* 27 */
		LIVE_GAME_EVENT_STADIUM_BREAKDOWN, /* 28 */
		LIVE_GAME_EVENT_STADIUM_RIOTS, /* 29 */
		LIVE_GAME_EVENT_STADIUM_FIRE, /* 30 */
		LIVE_GAME_EVENT_SUBSTITUTION, /* 31 */
		LIVE_GAME_EVENT_STRUCTURE_CHANGE, /* 32 */
		LIVE_GAME_EVENT_STYLE_CHANGE_ALL_OUT_DEFEND, /* 33 */
		LIVE_GAME_EVENT_STYLE_CHANGE_DEFEND, /* 34 */
		LIVE_GAME_EVENT_STYLE_CHANGE_BALANCED, /* 35 */
		LIVE_GAME_EVENT_STYLE_CHANGE_ATTACK, /* 36 */
		LIVE_GAME_EVENT_STYLE_CHANGE_ALL_OUT_ATTACK, /* 37 */
		LIVE_GAME_EVENT_BOOST_CHANGE_ANTI, /* 38 */
		LIVE_GAME_EVENT_BOOST_CHANGE_OFF, /* 39 */
		LIVE_GAME_EVENT_BOOST_CHANGE_ON, /* 40 */
		LIVE_GAME_EVENT_END /* 41 */
	}

	public enum LiveGameUnitArea
	{
		LIVE_GAME_UNIT_AREA_DEFEND = 0,
		LIVE_GAME_UNIT_AREA_MIDFIELD,
		LIVE_GAME_UNIT_AREA_ATTACK,
		LIVE_GAME_UNIT_AREA_END
	}

	/** Indices for the time variable of th
     * #LiveGameUnit struct.  */
	public enum LiveGameUnitTime
	{
		LIVE_GAME_UNIT_TIME_FIRST_HALF = 0,
		LIVE_GAME_UNIT_TIME_SECOND_HALF,
		LIVE_GAME_UNIT_TIME_EXTRA_TIME,
		LIVE_GAME_UNIT_TIME_PENALTIES,
		LIVE_GAME_UNIT_TIME_END
	}

	/** Indices for the values in #LiveGameStats. */
	public enum LiveGameStatValue
	{
		LIVE_GAME_STAT_VALUE_GOALS_REGULAR = 0,
		LIVE_GAME_STAT_VALUE_SHOTS,
		LIVE_GAME_STAT_VALUE_SHOT_PERCENTAGE,
		LIVE_GAME_STAT_VALUE_POSSESSION,
		LIVE_GAME_STAT_VALUE_PENALTIES,
		LIVE_GAME_STAT_VALUE_FOULS,
		LIVE_GAME_STAT_VALUE_CARDS,
		LIVE_GAME_STAT_VALUE_REDS,
		LIVE_GAME_STAT_VALUE_INJURIES,
		LIVE_GAME_STAT_VALUE_END
	}

	public enum LiveGameStatArray
	{
		LIVE_GAME_STAT_ARRAY_SCORERS_FOR_DISPLAY = 0,
		LIVE_GAME_STAT_ARRAY_SCORERS,
		LIVE_GAME_STAT_ARRAY_YELLOWS,
		LIVE_GAME_STAT_ARRAY_REDS,
		LIVE_GAME_STAT_ARRAY_INJURED,
		LIVE_GAME_STAT_ARRAY_END
	}

	/** Indices for the team_value array.
     * @see game_get_values() */
	public enum GameTeamValue
	{
		GAME_TEAM_VALUE_GOALIE = 0,
		GAME_TEAM_VALUE_DEFEND,
		GAME_TEAM_VALUE_MIDFIELD,
		GAME_TEAM_VALUE_ATTACK,
		GAME_TEAM_VALUE_END
	}

	/** Some stats for a live game like ball possession,
     * shots on goal etc. */
	public class LiveGameStats
	{
		public float possession;
		public int[,] values = new int[2, (int)LiveGameStatValue.LIVE_GAME_STAT_VALUE_END];
		public Player[,] players = new Player[2, (int)LiveGameStatArray.LIVE_GAME_STAT_ARRAY_END];
	}

	/** A struct telling us what's happening at
     * a certain moment in a game. */
	public class LiveGameEvent
	{
		/** @see #LiveGameEventType */
		public LiveGameEventType type;
		/** Verbosity value. The lower the more important the event. */
		public int verbosity;
		/** Information about a team and two players involved in the event. */
		public int team, player, player2;
		/** The commentary for the event. */
		public string commentary;
		/** Id of the commentary. */
		public int commentaryId;
	}

	/** A struct representing a fraction of a live game. */
	public class LiveGameUnit
	{
		/** Tells us which of the teams is in possession
         * of the ball. */
		public int possession;
		/** The area of the pitch the ball is currently in. */
		public int area;
		/** Which minute of the game and which part of
         * the game. If 'minute' is -1 we have an event
         * like a substitution that doesn't count as a 
         * match time consuming event. @see #GameUnitTime */
		public int minute, time;

		/** The match result at the time of this unit. */
		public int[] result = new int[2];

		/** The event belonging to the game unit. */
		public LiveGameEvent gameEvent;
	}

	/** A structure storing team settings during a live game
     * pause (so that we know what users have changed in pauses. */
	public class LiveGameTeamState
	{
		public int structure, style;
		public bool boost;
		public int[] playerIds = new int[11];
	}

	public class LiveGame
	{
		/** The fixture that belongs to the game. */
		public Fixture fix;
		/** Integer determining the fixture (needed because fixture pointers
         * can change when new fixtures are added to the fixtures array). */
		public int fixId;
		/** The names of the teams stored for later use (when the fixture
         * already got freed). */
		public string[] teamNames = new string[2];
		/** Attendance stored for later use (when the fixture
         * already got freed). */
		public int attendance;
		/** Substitutions left for the teams. */
		public int[] subsLeft = new int[2];
		/** The team that started the game, 0 or 1. */
		public int startedGame;
		/** We keep track of the stadium events because
         * there shouldn't be more than one of them in a game. */
		public int stadiumEvent;
		/** Attacking, defending etc. values for both teams.
         * @see #GameTeamValue */
		public float[,] teamValues = new float[2, (int)GameTeamValue.GAME_TEAM_VALUE_END];
		/** The home advantage factor. */
		public float home_advantage;
		/** The array of units. @see #GameUnit */
		public List<LiveGameUnit> units;
		/** Match statistics. @see #LiveGameStats */
		public LiveGameStats stats;
		public LiveGameTeamState[] teamState = new LiveGameTeamState[2];
		/** Ids of strategy actions applied. Actions only get applied once. */
		public ArrayList[] actionIds = new ArrayList[2];
	}
}

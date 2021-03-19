using System;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	/**
     * Player positions.
     * */
	public enum PlayerPos
	{
		PLAYER_POS_GOALIE = 0,
		PLAYER_POS_DEFENDER,
		PLAYER_POS_MIDFIELDER,
		PLAYER_POS_FORWARD,
		PLAYER_POS_ANY,
		PLAYER_POS_END
	}

	/** Streaks a player can go on. */
	public enum PlayerStreak
	{
		PLAYER_STREAK_COLD = -1,
		PLAYER_STREAK_NONE,
		PLAYER_STREAK_HOT
	}

	/**
     * Cards in different cups are counted separately for players;
     * for each league or cup the cards are stored in such a struct.
     * */
	public class PlayerCard
	{
		/** Numerical id of the league or cup. */
		public int clid;
		/** Number of yellow cards. */
		public int yellow;
		/** Number of weeks the player is banned. */
		public int red;
	}

	/**
     * Goals and games in different leagues and cups are counted separately for players.
     * */
	public class PlayerGamesGoals
	{
		/** Numerical id of the league or cup. */
		public int clid;
		/** Number of games the player played. */
		public int games;
		/** Number of goals (scored for field players or conceded for goalies). */
		public int goals;
		/** Number of shots (taken or faced). */
		public int shots;
	}

	public enum PlayerInjury
	{
		PLAYER_INJURY_NONE = 0,
		PLAYER_INJURY_CONCUSSION,
		PLAYER_INJURY_PULLED_MUSCLE,
		PLAYER_INJURY_HAMSTRING,
		PLAYER_INJURY_GROIN,
		PLAYER_INJURY_FRAC_ANKLE,
		PLAYER_INJURY_RIB,
		PLAYER_INJURY_LEG,
		PLAYER_INJURY_BROK_ANKLE,
		PLAYER_INJURY_ARM,
		PLAYER_INJURY_SHOULDER,
		PLAYER_INJURY_LIGAMENT,
		PLAYER_INJURY_CAREER_STOP,
		PLAYER_INJURY_END
	}

	/** Enum for different player data. */
	public enum PlayerValue
	{
		PLAYER_VALUE_GAMES = 0,
		PLAYER_VALUE_GOALS,
		PLAYER_VALUE_SHOTS,
		PLAYER_VALUE_CARD_YELLOW,
		PLAYER_VALUE_CARD_RED,
		PLAYER_VALUE_END
	}

	/** Enumeration for the yellow/red
     * card status during the live game. */
	public enum PlayerCardStatus
	{
		PLAYER_CARD_STATUS_NONE = 0,
		PLAYER_CARD_STATUS_YELLOW,
		PLAYER_CARD_STATUS_RED
	}

	/**
     * Representation of a player.
     * @see #PlayerAttributes
     * */
	public class Player
	{
		public string name;

		public PlayerPos Position { get; set; } /**< Position. @see #PlayerPos */
		public PlayerPos CurrentPosition { get; set; } /**< Current position. @see #PlayerPos */
		public int Health { get; set; } /**< Health. An integer signifying an injury or
           good health. @see #PlayerInjury */
		public int Recovery { get; set; } /**< Weeks until the player gets healthy. */
		public int Id { get; set; } /**< Id of the player within the team. */
		public int Value { get; set; } /**< Value of the player. */
		public int Wage { get; set; } /**< Wage of the player. */
		public int Offers { get; set; } /**< Number of times the player received a contract offer. */
		public int Streak { get; set; } /**< The streak the player is on. */
		public int CardStatus { get; set; } /**< The card status of the player during a live game. */

		public float Skill { get; set; } /**< Skill. Between 0 and a constant (specified in the constants file). */
		public float CurrentSkill { get; set; } /**< Current Skill. */
		public float Talent { get; set; } /**< Talent. The peak ability (which isn't always reached). */
		public float[] etal = new float[(int)Quality.QUALITY_END];  /**< Estimated talent (the user never sees the actual talent).
                  Depends on scout quality. */
		public float Fitness { get; set; } /**< Fitness. Between 0 and 1. */
		public float LSU { get; set; } /**< Last skill update. Number of weeks since the player skill was last updated. */
		public float Age { get; set; } /**< Age in years. */
		public float PeakAge { get; set; } /**< Age at which the player reaches his peak ability. */
		public float PeakRegion { get; set; } /**< Region around the peak age during which the player's
            ability is at the peak (in years). */
		public float Contract { get; set; } /**< The years until the player's contract expires. */
		public float StreakProb { get; set; } /**< This number determines how probable it is that a player
            goes on a hot/cold streak. Between -1 and 1. */
		public float StreakCount { get; set; } /**< How many weeks the streak goes (or how
             long a new streak may not begin if the value
             is negative). */

		/** Whether the player participated in the team's last match. */
		public bool Participation { get; set; }

		/** Array of games and goals; one item per league and cup.
         * @see PlayerGamesGoals */
		public List<PlayerGamesGoals> GamesGoals { get; set; }

		/** Array of cards; one item per league and cup.
         * @see PlayerCard*/
		public List<PlayerCard> Cards { get; set; }

		/** Career goals, games etc. */
		public int[] career = new int[(int)PlayerValue.PLAYER_VALUE_END];

		/** Pointer to the player's team. */
		public Team Team { get; set; }
	}

	/** Enum for player attributes that can be shown in a player list. */
	public enum PlayerListAttributeValue
	{
		PLAYER_LIST_ATTRIBUTE_NAME = 0,
		PLAYER_LIST_ATTRIBUTE_CPOS,
		PLAYER_LIST_ATTRIBUTE_POS,
		PLAYER_LIST_ATTRIBUTE_CSKILL,
		PLAYER_LIST_ATTRIBUTE_SKILL,
		PLAYER_LIST_ATTRIBUTE_FITNESS,
		PLAYER_LIST_ATTRIBUTE_GAMES,
		PLAYER_LIST_ATTRIBUTE_SHOTS,
		PLAYER_LIST_ATTRIBUTE_GOALS,
		PLAYER_LIST_ATTRIBUTE_STATUS,
		PLAYER_LIST_ATTRIBUTE_CARDS,
		PLAYER_LIST_ATTRIBUTE_AGE,
		PLAYER_LIST_ATTRIBUTE_ETAL,
		PLAYER_LIST_ATTRIBUTE_VALUE,
		PLAYER_LIST_ATTRIBUTE_WAGE,
		PLAYER_LIST_ATTRIBUTE_CONTRACT,
		PLAYER_LIST_ATTRIBUTE_TEAM,
		PLAYER_LIST_ATTRIBUTE_LEAGUE_CUP,
		PLAYER_LIST_ATTRIBUTE_END
	}

	public enum PlayerInfoAttributeValue
	{
		PLAYER_INFO_ATTRIBUTE_NAME = 0,
		PLAYER_INFO_ATTRIBUTE_POS,
		PLAYER_INFO_ATTRIBUTE_CPOS,
		PLAYER_INFO_ATTRIBUTE_SKILL,
		PLAYER_INFO_ATTRIBUTE_CSKILL,
		PLAYER_INFO_ATTRIBUTE_FITNESS,
		PLAYER_INFO_ATTRIBUTE_ETAL,
		PLAYER_INFO_ATTRIBUTE_AGE,
		PLAYER_INFO_ATTRIBUTE_HEALTH,
		PLAYER_INFO_ATTRIBUTE_VALUE,
		PLAYER_INFO_ATTRIBUTE_WAGE,
		PLAYER_INFO_ATTRIBUTE_CONTRACT,
		PLAYER_INFO_ATTRIBUTE_GAMES_GOALS,
		PLAYER_INFO_ATTRIBUTE_YELLOW_CARDS,
		PLAYER_INFO_ATTRIBUTE_BANNED,
		PLAYER_INFO_ATTRIBUTE_STREAK,
		PLAYER_INFO_ATTRIBUTE_CAREER,
		PLAYER_INFO_ATTRIBUTE_OFFERS,
		PLAYER_INFO_ATTRIBUTE_END
	}

	/** A struct telling us which player attributes to show in a player list.
     * @see #PlayerListAttributeValue*/
	public class PlayerListAttribute
	{
		public bool[] onOff = new bool[(int)PlayerListAttributeValue.PLAYER_LIST_ATTRIBUTE_END];
	}
}

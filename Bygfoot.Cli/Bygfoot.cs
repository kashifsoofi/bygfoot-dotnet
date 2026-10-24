namespace Bygfoot.Cli;

using System;
using Models;

public static class Bygfoot
{
    public const string Version = "2.3.3";
    public const string Year = "2005 - 2011";
    public const string HomeDirName = ".bygfoot";
    public const int Small = 10000;
    public const int Big = 1000000;
    public const int IdLeagueStart = 1000;
    public const int IdCupStart = 7000;

    public enum ExitCode
    {
        Ok = 0,
        General,
        NoCountryFiles,
        FileOpenFailed,
        PrintError,
        NoLeagues,
        ChooseTeamError,
        FixtureWriteError,
        UserFired,
        FirstWeekError,
        OptionNotFound,
        PointerNotFound,
        IntNotFound,
        FileNotFound,
        NoSupportDir,
        CupRoundError,
        LoadTeamDef,
        DefSort,
        PromRel,
        StrategyError,
        BetError,
        End
    }

    public static int PlayerIdNew() => Variables.Counters[(int)Counters.CountPlayerId]++;
    public static int TeamIdNew() => Variables.Counters[(int)Counters.CountTeamId]++;
    public static int CupIdNew() => Variables.Counters[(int)Counters.CountCupId]++;
    public static int LeagueIdNew() => Variables.Counters[(int)Counters.CountLeagueId]++;
    public static int FixtureIdNew() => Variables.Counters[(int)Counters.CountFixId]++;
    public static int LgCommentaryIdNew() => Variables.Counters[(int)Counters.CountLgCommId]++;
    public static int NewsTitleIdNew() => Variables.Counters[(int)Counters.CountNewsTitleId]++;
    public static int NewsSubtitleIdNew() => Variables.Counters[(int)Counters.CountNewsSubtitleId]++;
    public static int NewsArticleIdNew() => Variables.Counters[(int)Counters.CountNewsArticleId]++;

    public static League GetLeague(int i) => Variables.Country.Leagues[i];
    public static Cup GetCup(int i) => Variables.Country.Cups[i];
    public static Cup GetAllCup(int i) => Variables.Country.AllCups[i];

    public static int Stat0 { get => Variables.Status[0]; set => Variables.Status[0] = value; }
    public static int Stat1 { get => Variables.Status[1]; set => Variables.Status[1] = value; }
    public static int Stat2 { get => Variables.Status[2]; set => Variables.Status[2] = value; }
    public static int Stat3 { get => Variables.Status[3]; set => Variables.Status[3] = value; }
    public static int Stat4 { get => Variables.Status[4]; set => Variables.Status[4] = value; }
    public static int Stat5 { get => Variables.Status[5]; set => Variables.Status[5] = value; }
}

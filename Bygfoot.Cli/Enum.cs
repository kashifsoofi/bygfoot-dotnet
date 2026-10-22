namespace Bygfoot.Cli;

/// <summary>Some counters.
/// Corresponds to C enum Counters in enums.h.</summary>
public enum Counters
{
    CountAutosave = 0,
    CountAutosaveFile,
    CountTeamId,
    CountPlayerId,
    CountCupId,
    CountLeagueId,
    CountFixId,
    CountLgCommId,
    CountNewsTitleId,
    CountNewsSubtitleId,
    CountNewsArticleId,
    CountShowDebug,
    CountHintNumber,
    CountNewsShown,
    CountNewNews,
    CountEnd
}

/// <summary>Scout and physio qualities.
/// Corresponds to C enum Quality in enums.h.</summary>
public enum Quality
{
    QualityBest = 0,
    QualityGood,
    QualityAverage,
    QualityBad,
    QualityEnd
}

/// <summary>Indices for the finances variable.
/// Corresponds to C enum FinanceValue in enums.h.</summary>
public enum FinanceValue
{
    /// <summary>Prize money at the end of the season.</summary>
    FinPrize = 0,
    /// <summary>User's debts.</summary>
    FinDebts,
    /// <summary>User's money.</summary>
    FinMoney,
    /// <summary>Ticket income (weekly).</summary>
    FinTickets,
    /// <summary>Journey costs (weekly).</summary>
    FinJourney,
    /// <summary>Wages (weekly).</summary>
    FinWages,
    /// <summary>Scout wage (weekly).</summary>
    FinScout,
    /// <summary>Physio wage (weekly).</summary>
    FinPhysio,
    /// <summary>Transfer fees income (weekly).</summary>
    FinTransfersIn,
    /// <summary>Transfer fees spent (weekly).</summary>
    FinTransfersOut,
    /// <summary>Stadium improvement (weekly).</summary>
    FinStadium,
    /// <summary>Bills for riots or fire or so (weekly).</summary>
    FinStadBills,
    FinEnd
}

/// <summary>Enum used to determine what kind of fixtures or
/// tables to show (e.g. when an arrow is clicked on).
/// Corresponds to C enum ShowType in enums.h.</summary>
public enum ShowType
{
    ShowTeam = 0,
    ShowCurrent,
    ShowNext,
    ShowPrevious,
    ShowNextLeague,
    ShowPreviousLeague,
    ShowEnd
}

/// <summary>Possible values for a status variable.
/// Corresponds to C enum Status0Value in enums.h.</summary>
public enum Status0Value
{
    StatusNone = 0,
    StatusMain,
    StatusShowLiveGame,
    StatusLiveGamePause,
    StatusLiveGameChange,
    StatusShowTeamList,
    StatusShowPlayerInfo,
    StatusBrowseTeams,
    StatusTeamSelection,
    StatusShowLastMatch,
    StatusShowLastMatchPause,
    StatusShowLastMatchAbort,
    StatusShowLastMatchStats,
    StatusShowFixtures,
    StatusShowFixturesWeek,
    StatusShowTables,
    StatusShowFinances,
    StatusShowTransferList,
    StatusShowUserHistory,
    StatusGetLoan,
    StatusPayLoan,
    StatusShowEvent,
    StatusJobOfferSuccess,
    StatusJobOfferFireFinance,
    StatusJobOfferFireFailure,
    StatusTransferOfferUser,
    StatusTransferOfferCpu,
    StatusCustomStructure,
    StatusShowLeagueResults,
    StatusShowSeasonResults,
    StatusShowLeagueStats,
    StatusShowSeasonHistory,
    StatusShowPlayerList,
    StatusFirePlayer,
    StatusUserManagement,
    StatusShowPreview,
    StatusSaveGame,
    StatusLoadGame,
    StatusLoadGameSplash,
    StatusQueryUnfit,
    StatusQueryQuit,
    StatusQueryUserNoTurn,
    StatusGenerateTeams,
    StatusSponsorContinue,
    StatusShowYa,
    StatusSetYaPercentage,
    StatusQueryKickYouth,
    StatusSelectMmFileLoad,
    StatusSelectMmFileAdd,
    StatusSelectMmFileImport,
    StatusSelectMmFileExport,
    StatusPlaceBet,
    StatusShowJobExchange,
    StatusJobExchangeShowTeam,
    StatusSplash,
    StatusEnd
}

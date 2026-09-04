namespace MGFGame
{
    /// <summary>
    /// Represents the different states/screens in the game
    /// </summary>
    public enum GameState
    {
        StartScreen,
        Playing,
        Leaderboard,
        LeaderboardFromEnd, // Leaderboard accessed from end screen
        Rules,
        Credits,
        EndScreen
    }
}
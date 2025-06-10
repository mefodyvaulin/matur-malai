using System.Collections.Generic;

public static class Statistic
{
    public static SaveAllData saveAllData;
    public static float sessionScore;
    public static List<int> LastGamesScore
    {
        get => saveAllData.lastScores;
        set => saveAllData.lastScores = value;
    }

    public static int playerSessionMoney;
    public static int PlayersMoney
    {
        get => saveAllData.playersMoney;
        set => saveAllData.playersMoney = value;
    }

    private static int BestScore
    {
        get => saveAllData.bestScore;
        set => saveAllData.bestScore = value;
    }

    public static void SaveStat()
    {
        SetScore();
        SetMoney();
    }

    private static void SetScore()
    {
        LastGamesScore.Add((int)sessionScore);
        if (LastGamesScore.Count == 6) LastGamesScore.RemoveAt(0);
        if (sessionScore > BestScore)
        {
            BestScore = (int)sessionScore;
        }
        sessionScore = 0;
    }

    private static void SetMoney()
    {
        PlayersMoney += playerSessionMoney;
        playerSessionMoney = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameSummary : MonoBehaviour
{

    //Player
    public static int playerDefeated;
    public static int playerPoints;
    public static int playerUnits;
    public static int playerMoney;
    public static int playerMisses;

    //AI
    public static int aiDefeated;
    public static int aiPoints;
    public static int aiUnits;
    public static int aiMoney;
    public static int aiMisses;

    [SerializeField] private TextMeshProUGUI[] playerStats;
    [SerializeField] private TextMeshProUGUI[] aiStats;
    [SerializeField] private TextMeshProUGUI[] generalStats;

    private void Start()
    {

        ResetAIStats();
        ResetPlayerStats();        

    }

    private void ResetPlayerStats()
    {
        //Reset player stats
        playerDefeated =
        playerPoints =
        playerMoney =
        playerUnits =
        playerMisses = 0;
    }

    private void ResetAIStats()
    {
        //Reset AI stats
        aiDefeated =
        aiPoints =
        aiMoney =
        aiUnits =
        aiMisses = 0;
    }

    public void SetStats()
    {
        playerStats[0].text = playerDefeated.ToString();
        playerStats[1].text = playerPoints.ToString();
        playerStats[2].text = playerMoney.ToString();
        playerStats[3].text = playerUnits.ToString();
        playerStats[4].text = playerMisses.ToString();

        aiStats[0].text = aiDefeated.ToString();
        aiStats[1].text = aiPoints.ToString();
        aiStats[2].text = aiMoney.ToString();
        aiStats[3].text = aiUnits.ToString();
        aiStats[4].text = aiMisses.ToString();

        generalStats[0].text = CalculateTime(TurnSystem.Instance.GetDuration()).ToString();
        generalStats[1].text = TurnSystem.Instance.GetTurnNumber().ToString();
    }

    private string CalculateTime(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        return timeText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSummary : MonoBehaviour
{
    CurrentGameState state;

    //Stats to track

    //General
    private float duration;
    private int turns;

    //Player
    private int playerDefeated;
    private int playerPoints;
    private int playerUnits;
    private int playerMoney;
    private int playerMisses;

    //AI
    private int aiDefeated;
    private int aiPoints;
    private int aiUnits;
    private int aiMoney;
    private int aiMisses;


    private void Update()
    {
       if (state == CurrentGameState.PLAYING)
       {
            duration += Time.deltaTime;
       }
    }
}

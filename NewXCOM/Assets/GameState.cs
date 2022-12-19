using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum CurrentState
{
    PLAYING,
    OVER
}

public class GameState : MonoBehaviour
{
    CurrentState state;
    [SerializeField] Transform canvases;

    void Awake()
    {
        state = CurrentState.PLAYING;
    }

    private void Start()
    {
        BaseGameOver.OnAnyBaseDestroyed += BaseGameOver_OnAnyBaseDestroyed;
    }

    public void BaseGameOver_OnAnyBaseDestroyed(object sender, Unit unit)
    {
        //Se acaba la partida
        state = CurrentState.OVER;

        //Se calculan los stats
        canvases.GetChild(0).GetComponent<GameSummary>().SetStats();

        //Se muestran
        ShowGameSummaryAndHideOtherCanvases();

        if (unit.IsEnemy())
        {
            Debug.Log("Ganaste");
        }

        else
        {
            Debug.Log("Perdiste");
        }
    }

    public void ShowGameSummaryAndHideOtherCanvases()
    {
        canvases.GetChild(0).gameObject.SetActive(true);

        for (int i = 1; i < canvases.childCount; i++)
        {
            canvases.GetChild(i).gameObject.SetActive(false);
        }
    }

    public CurrentState GetCurrenState()
    {
        return state;
    }
}

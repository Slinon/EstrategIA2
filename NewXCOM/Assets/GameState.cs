using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum CurrentGameState
{
    PLAYING,
    OVER,
}

public class GameState : MonoBehaviour
{
    CurrentGameState gameState;
    HealthSystem healthSystem;

    private void Awake()
    {
        gameState = CurrentGameState.PLAYING;
        //Tal vez alguna lógica de inicialización que haga falta puede ir aquí.
    }

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs empty)
    {
        gameState = CurrentGameState.OVER;

        if (gameObject.tag is "MainBase")
        {
            Debug.Log("Derrota...");
        }

        if (gameObject.tag is "EnemyMainBase")
        {
            Debug.Log("Victoria!");
        }

    }
}

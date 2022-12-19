using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    public static TurnSystem Instance { get; private set; }       // Instancia del singleton

    public event EventHandler OnTurnChanged;                      // Evento cuando se cambia de turno

    private int turnNumber;                                       // Numero del turno actual
    private bool isPlayerTurn;                                    // Booleano para saber de quien es el turno
    private float duration;

    private MoneySystem moneySystem;
    private GameState gameState;

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un TurnSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

        // Establecemos los atributos
        turnNumber = 1;
        isPlayerTurn = true;

    }

    //@GRG 
    //Modificaciones para que cada turno se reciba dinero:
    //Llamar a GiveTakeMoney() de Money System
    private void Start()
    {

        moneySystem = FindObjectOfType<MoneySystem>();
        gameState = FindObjectOfType<GameState>();

    }

    //GRG
    //trackear duración de la partida
    private void Update()
    {
        if (gameState.GetCurrenState() == CurrentState.OVER)
        {
            return;
        }

        duration += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Time.timeScale = 2;
        }
        else { Time.timeScale = 1; }

    }
   
    // @IGM --------------------------------
    // Metodo para pasar al turno siguiente.
    // -------------------------------------
    public void NextTurn()
    {

        // Aumentamos el numero de turno
        turnNumber++;

        // Cambiamos el turno
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {
            //Si es el turno del jugador, dar dinero
            moneySystem.GiveTakeMoney(moneySystem.MoneyPerTurn(), moneySystem.player);
            //Time.timeScale = 1;

        }

        else
        {
            //Si es el turno de la IA, dar dinero
            moneySystem.GiveTakeMoney(moneySystem.MoneyPerTurn(), moneySystem.enemyAI);
            
        }

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnTurnChanged != null)
        {

            // Lanzamos el evento
            OnTurnChanged(this, EventArgs.Empty);

        }

    }

    // @IGM ------------------
    // Getter el turno actual.
    // -----------------------
    public int GetTurnNumber()
    {

        return turnNumber;

    }

    // @IGM -------------------------------------------------
    // Funcion para saber si estamos en el turno del jugador.
    // ------------------------------------------------------
    public bool IsPlayerTurn()
    {

        return isPlayerTurn;

    }

    //GRG
    //Getter duración
    public float GetDuration()
    {
        return duration;
    }

}

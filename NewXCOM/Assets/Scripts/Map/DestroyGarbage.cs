using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGarbage : MonoBehaviour
{

    // @IGM ------------------------------------------
    // Maquina de estados de eliminar basira del mapa.
    // -----------------------------------------------
    private enum State
    {

        WaitingBeforeDestroy,
        Destroying

    }

    [SerializeField] private float timeAfterDestroy;        // Tiempo que pasará antes de destruir el objeto
    [SerializeField] private float destroyingTimer;         // Tiempo que pasará mientras se destruye el objeto

    private float timer;                                    // Tiempo actual
    private bool startTimer;                                // Booleano para indicar cuando se empieza el timer
    private State state;                                     // Estado de la basura

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    void Start()
    {

        // Nos suscribimos a los eventos
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        // Ponemos el booleano a false
        startTimer = false;
        state = State.WaitingBeforeDestroy;
        timer = timeAfterDestroy;

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    void Update()
    {

        // Comprobamos si no ha empezado el timer
        if (!startTimer)
        {

            // Saltamos el frame
            return;

        }

        // Comprobamos que el estado sea el de destruir
        if (state == State.Destroying)
        {

            // Calulamos la escala en referencia al tiempo
            float scale = timer / destroyingTimer;

            // Aplicamos la escala
            transform.localScale = new Vector3(scale, scale, scale);

        }

        // Restamos el timer
        timer -= Time.deltaTime;

        // Comprobamos si el timer ha llegado a 0
        if (timer <= 0)
        {

            // Pasamos al siguiente estado
            NextState();

        }

    }

    // @IGM -------------------------------------
    // Handler del evento cuando cambia el turno.
    // ------------------------------------------
    private void TurnSystem_OnTurnChanged(object sender,EventArgs empty)
    {

        // Iniciamos el timer
        startTimer = true;

    }

    // @IGM -------------------------------------------------
    // Metodo para ejecutar el siguiente estado de la basura.
    // ------------------------------------------------------
    private void NextState()
    {

        // Comprobamos el estado de la accion
        switch (state)
        {

            case State.WaitingBeforeDestroy:

                // Cambiamos el estado y los parametros
                state = State.Destroying;
                timer = destroyingTimer;

                break;

            case State.Destroying:

                Destroy(this.gameObject);

                break;

        }

    }

}

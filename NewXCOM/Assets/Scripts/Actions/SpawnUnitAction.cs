using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitAction : BaseAction
{

    // @IGM -----------------------------------------------------
    // Maquina de estados de de la accion de spawnear una unidad.
    // ----------------------------------------------------------
    private enum State
    {

        WaitingBeforeSpawn,
        WaitingAfterSpawn

    }

    public event EventHandler OnSpawnActionStarted;                 // Evento cuando la accion de spawnear empieza
    public event EventHandler OnSpawnActionCompleted;               // Evento cuando la accion de spawnear se completa
    public static event EventHandler<Vector3> OnAnyUnitSpawned;     // Evento cuando cualquier unidad dispara

    [SerializeField] private Transform unitSpawned;                 // Unidad que queremos spawnear
    [SerializeField] private int maxSpawnDistance;                  // Distancia maxima de spawn

    private Vector3 spawnPoint;                                     // Punto donde spawnea la unidad
    private State state;                                            // Estado actual de la accion
    private float stateTimer;                                       // Timer de la maquina de estados

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {

        // Comprobamos si la accion se ha activado
        if (!isActive)
        {

            // No hacemos nada si la accion esta inactiva
            return;

        }

        // Restamos el timer
        stateTimer -= Time.deltaTime;

        // Comprobamos el estado de la accion
        switch (state)
        {

            case State.WaitingBeforeSpawn:

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnSpawnActionStarted != null)
                {

                    // Lanzamos el evento
                    OnSpawnActionStarted(this, EventArgs.Empty);

                }

                break;
            case State.WaitingAfterSpawn:

                break;

        }

        // Comprobamos si el timer ha llegado a 0
        if (stateTimer <= 0)
        {

            // Pasamos al siguiente estado
            NextState();

        }

    }

    // @IGM ---------------------------------
    // Metodo para pasar al siguiente estado.
    // --------------------------------------
    private void NextState()
    {

        // Comprobamos el estado de la accion
        switch (state)
        {

            case State.WaitingBeforeSpawn:

                // Cambiamos el estado y los parametros
                state = State.WaitingAfterSpawn;
                float afterSpawnStateTime = 0.5f;
                stateTimer = afterSpawnStateTime;

                // Spawneamos la unidad
                Instantiate(unitSpawned, spawnPoint, Quaternion.identity);

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnAnyUnitSpawned != null)
                {

                    // Lanzamos el evento
                    OnAnyUnitSpawned(this, spawnPoint);

                }

                break;

            case State.WaitingAfterSpawn:

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnSpawnActionCompleted != null)
                {

                    // Lanzamos el evento
                    OnSpawnActionCompleted(this, EventArgs.Empty);

                }

                // Terminamos la accion
                ActionComplete();

                break;

        }

    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public override string GetActionName()
    {

        return "Spawn Unit";

    }

    // @IGM ------------------------------------------------------------------------------
    // Funcion para saber que accion de la IA se puede hacer en la poscicion seleccionada.
    // -----------------------------------------------------------------------------------
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        // Devolvemos la accion de la IA
        return new EnemyAIAction
        {

            gridPosition = gridPosition,
            actionValue = 100

        };

    }

    // @IGM -------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde moverse.
    // ------------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {

        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxSpawnDistance; x <= maxSpawnDistance; x++)
        {

            for (int z = -maxSpawnDistance; z <= maxSpawnDistance; z++)
            {

                // Creamos la posicion alrededor de la posicion del jugador
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // Comprobamos si la posicion esta fuera de la malla
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Comprobamos si la posicion tiene unidades dentro
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Lo añadimos a la lista
                validGridPositionList.Add(testGridPosition);

            }

        }

        return validGridPositionList;

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        // Asignamos las variables
        state = State.WaitingBeforeSpawn;
        spawnPoint = LevelGrid.Instance.GetWorldPosition(gridPosition);
        float beforeSpawnStateTime = 0.7f;
        stateTimer = beforeSpawnStateTime;

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

}
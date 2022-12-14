using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitAction : BaseAction
{

    [SerializeField] protected int unitCost;
    [SerializeField] protected Transform unitSpawned;                 // Unidad que queremos spawnear

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

    private int maxSpawnDistanceHeight;                  // Distancia maxima de spawn
    private int maxSpawnDistanceWidht;
    private GameObject[] interactionSpheres;

    private Vector3 spawnPoint;                                     // Punto donde spawnea la unidad
    private State state;                                            // Estado actual de la accion
    private float stateTimer;                                       // Timer de la maquina de estados

    public override int MoneyCost()
    {
        return unitCost;
    }

    private void Start()
    {
        interactionSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        maxSpawnDistanceHeight = 7;
        maxSpawnDistanceWidht = 12;
    }

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

                if (unit.IsEnemy())
                {
                    MoneySystem.Instance.GiveTakeMoney(-unitCost, MoneySystem.Instance.enemyAI);
                }

                else
                {
                    MoneySystem.Instance.GiveTakeMoney(-unitCost, MoneySystem.Instance.player);
                }

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

        return "Spawn ";

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
            actionValue = baseAIValue + GetTargetValueAtPosition(gridPosition)

        };

    }

    // @IGM -------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde moverse.
    // ------------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return GetCapturedPositionList(false);
    }

    // Si el bool paiting es false ignorará las posiciones con obstáculos y unidades
    // Si es false las tendrá en cuenta. Uso para pintar areas de las zonas capturadas
    public List<GridPosition> GetCapturedPositionList(bool painting)
    {
        InteractSphere.InControlState state = InteractSphere.InControlState.Player;

        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Creamos la lista posiciones
        List<GridPosition> allPositionsList = new List<GridPosition>(); // Posicion unidad y puntos capturados

        // Creamos la lista spawns distances
        List<int> maxSpawnDistanceListWidth = new List<int>(); // Spawn Distance unidad y puntos capturados
        List<int> maxSpawnDistanceListHeight = new List<int>(); // Spawn Distance unidad y puntos capturados

        // Recuperamos la posicion de la unidad
        // GridPosition unitGridPosition = unit.GetGridPosition();
        allPositionsList.Add(unit.GetGridPosition());

        maxSpawnDistanceListWidth.Add(maxSpawnDistanceWidht);
        maxSpawnDistanceListHeight.Add(maxSpawnDistanceHeight);

        // Comprobar puntos capturados y su distancia maxima alrededor

        if (unit.IsEnemy())
        {
            state = InteractSphere.InControlState.Enemy;
        }

        foreach (GameObject child in interactionSpheres)
        {
            InteractSphere sphere = child.GetComponent<InteractSphere>();

            if (sphere.GetInControlState() == state)
            {
                allPositionsList.Add(sphere.GetGridPosition());
                maxSpawnDistanceListWidth.Add(sphere.GetMaxCaptureDistanceWidth());
                maxSpawnDistanceListHeight.Add(sphere.GetMaxCaptureDistanceHeight());
            }
        }

        for (int i = 0; i < allPositionsList.Count; i++)
        {
            // Recorremos todas las posiciones validas alrededor de la malla
            for (int x = -maxSpawnDistanceListWidth[i]; x <= maxSpawnDistanceListWidth[i]; x++) // width
            {
                for (int z = -maxSpawnDistanceListHeight[i]; z <= maxSpawnDistanceListHeight[i]; z++) // height
                {

                    // Creamos la posicion alrededor de la posicion del jugador
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = allPositionsList[i] + offsetGridPosition;

                    // Comprobamos si la posicion esta fuera de la malla
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {

                        // La saltamos
                        continue;

                    }

                    // Comprobamos si la posicion es en la que esta la unidad
                    if (allPositionsList[i] == testGridPosition)
                    {

                        // La saltamos
                        continue;

                    }

                    // Comprobamos si la posicion tiene unidades dentro
                    if (!painting && LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {

                        // La saltamos
                        continue;

                    }

                    // Comprobamos si la posicion es un obstaculo
                    if (!painting && !Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    {

                        // La saltamos
                        continue;

                    }

                    // Comprobamos que la posicion no esta ya en la lista
                    if (!validGridPositionList.Contains(allPositionsList[i]))
                    {
                        // Lo añadimos a la lista
                        validGridPositionList.Add(testGridPosition);
                    }
                }
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

    // @IGM ------------------------------------------------
    // Funcion para calcular la mejor posicion de la accion.
    // -----------------------------------------------------
    public override int GetTargetValueAtPosition(GridPosition gridPosition)
    {

        return 0;

    }

}
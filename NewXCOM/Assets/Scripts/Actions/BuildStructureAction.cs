using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStructureAction : BaseAction
{

    // @IGM -----------------------------------------------------
    // Maquina de estados de de la accion de spawnear una unidad.
    // ----------------------------------------------------------
    private enum State
    {

        WaitingBeforeSpawn,
        WaitingAfterSpawn

    }

    public event EventHandler OnStartBuilding;
    public event EventHandler OnStopBuilding;

    [SerializeField] private int maxBuildDistance;          // Distancia maxima de construcción
    [SerializeField] private GameObject structure;          // shield
    [SerializeField] private LayerMask obstacleLayerMask;   // Capa de los obstaculos
    [SerializeField] private int maxStructureCount;         // Limite de estructuras que se pueden construir con esta accion

    private State state;                                    // Estado actual de la accion
    private float stateTimer;                               // Timer de la maquina de estados
    private Vector3 spawnPoint;                             // Punto donde spawnea la unidad
    private int structureCount;                             // Numero de torres que se pueden construir


    // Update is called once per frame
    void Update()
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
                if (OnStartBuilding != null)
                {

                    // Lanzamos el evento
                    OnStartBuilding(this, EventArgs.Empty);

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

                // Comprobamos que no hemos superado el maximo de estructuras
                if (GetStructureCount() < GetMaxStructureCount())
                {

                    // Spawneamos la unidad
                    Unit newStructure = Instantiate(structure, spawnPoint, Quaternion.identity).GetComponent<Unit>();

                    // Le quitamos los puntos de accion
                    newStructure.SpendActionPoints(newStructure.GetActionPoints());

                    // Asignamos la estructura a esta unidad
                    if (newStructure.TryGetComponent(out Structure turret))
                    {

                        turret.SetUnit(unit);

                    }

                    // Incrementamos el numero de estructuras generadas por esta unidad
                    AddStructureCount();

                }

                break;

            case State.WaitingAfterSpawn:

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnStopBuilding != null)
                {

                    // Lanzamos el evento
                    OnStopBuilding(this, EventArgs.Empty);

                }

                // Terminamos la accion
                ActionComplete();

                break;

        }

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        // Asignamos las variables
        state = State.WaitingBeforeSpawn;
        spawnPoint = LevelGrid.Instance.GetWorldPosition(gridPosition);
        float beforeSpawnStateTime = 0.7f;
        stateTimer = beforeSpawnStateTime;

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxBuildDistance; x <= maxBuildDistance; x++)
        {

            for (int z = -maxBuildDistance; z <= maxBuildDistance; z++)
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

                // Comprobamos si la posicion es un obstaculo
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
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

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {

            gridPosition = gridPosition,
            actionValue = baseAIValue + LevelGrid.Instance.GetHeatMapValueAtGridPosition(gridPosition)

        };
    }

    public override string GetActionName()
    {

        return "Turret";

    }

    // @IGM ------------------------------------------------
    // Funcion para calcular la mejor posicion de la accion.
    // -----------------------------------------------------
    public override int GetTargetValueAtPosition(GridPosition gridPosition)
    {

        // Calculamos la distancia de la torre
        int maxTurretRange = structure.GetComponentInChildren<ShootAction>().GetMaxShootDistance();

        int allyTurretsInRange = 0;
        int enemiesInRange = 0;

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxTurretRange; x <= maxTurretRange; x++)
        {

            for (int z = -maxTurretRange; z <= maxTurretRange; z++)
            {

                // Creamos la posicion alrededor de la posicion del jugador
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                // Comprobamos si la posicion esta fuera de la malla
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Calculamos la distancia de la posicion a probar
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                // Comprobamos si la distancia es mayor a la de diparo
                if (testDistance > maxTurretRange)
                {

                    // La saltamos
                    continue;

                }

                // Comprobamos si la posicion no tiene unidades dentro
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Recuperamos el target de la posicion
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                // Comprobamos si el target esta en el mismo equipo que la unidad
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {

                    // Comprobamos si la unidad es una torre
                    if (targetUnit.tag == "Turret")
                    {

                        allyTurretsInRange++;

                    }

                    // La saltamos
                    continue;

                }

                // Definimos un offset para poder disparar por encima de obstaculos bajos
                float unitShoulderHeight = 1.7f;

                // Calculamos la direccion de disparo
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                Unit unitPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                // Definimos un offset para poder disparar por encima de obstaculos bajos
                if (targetUnit.GetCoverType() == CoverType.Covered)
                {
                    if (unitPosition.GetCoverType() == CoverType.Covered)
                    {
                        unitShoulderHeight = 0.6f;
                    }
                    else { unitShoulderHeight = 1.7f; }
                }

                // Comprobamos si la unidad no tiene visual del objetivo
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDirection,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstacleLayerMask))
                {

                    // La saltamos
                    continue;

                }

                enemiesInRange++;

            }

        }

        return allyTurretsInRange * (-100) + enemiesInRange * 10;

    }

    // @GRG -----------------------------------------
    // Getter de la distancia maxima de construccion.
    // ----------------------------------------------
    public int GetMaxBuildDistance()
    {

        return maxBuildDistance;

    }

    // @IGM -------------------------------
    // Getter de la torre que se construye.
    // ------------------------------------
    public GameObject GetStructure()
    {

        return structure;

    }

    // @IGM --------------------------------------------------------
    // Getter de la capacidad maxima de construccion de estructuras.
    // -------------------------------------------------------------
    public int GetMaxStructureCount()
    {

        return maxStructureCount;

    }

    // @IGM --------------------------------------------
    // Getter de la cantidad de estructuras construidas.
    // -------------------------------------------------
    public int GetStructureCount()
    {

        return structureCount;

    }

    // @IGM ------------------------------------------
    // Metodo para añadir una estructura en la cuenta.
    // -----------------------------------------------
    public void AddStructureCount()
    {

        structureCount++;

    }

    // @IGM --------------------------------------------
    // Metodo para eliminar una estructura en la cuenta.
    // -------------------------------------------------
    public void DeleteStructureCount()
    {

        structureCount--;

    }

}

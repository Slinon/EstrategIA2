using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStructureAction : BaseAction
{
    [SerializeField] private int maxBuildDistance;      // Distancia maxima de construcción
    [SerializeField] private GameObject structure;      // shield
    [SerializeField] LayerMask obstacleLayerMask;       // Capa de los obstaculos

    public event EventHandler OnStartBuilding;
    public event EventHandler OnStopBuilding;


    // Update is called once per frame
    void Update()
    {
        // Comprobamos si la accion se ha activado
        if (!isActive)
        {

            // No hacemos nada si la accion esta inactiva
            return;

        }

        if (OnStopBuilding != null)
        {
            OnStopBuilding(this, EventArgs.Empty);
        }

        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Instantiate(structure, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

        if (OnStartBuilding != null)
        {

            // Lanzamos el evento
            OnStartBuilding(this, EventArgs.Empty);

        }

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

        return "Shield";

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

}

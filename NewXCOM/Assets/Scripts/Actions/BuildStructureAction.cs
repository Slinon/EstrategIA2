using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildShieldAction : BaseAction
{
    [SerializeField] private int maxBuildDistance;      // Distancia maxima de construcción
    [SerializeField] private GameObject shield;         // shield

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
        Instantiate(shield, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

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
            actionValue = 200,

        };
    }

    public override string GetActionName()
    {

        return "Shield";

    }
}

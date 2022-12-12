using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;                // Evento cuando la unida se empieza a mover
    public event EventHandler OnStopMoving;                 // Evento cuando la unidad se termina de mover
    public event EventHandler OnPositionChanged;

    [SerializeField] private int maxMoveDistance;           // Distancia maxima del movimiento de la unidad

    private List<Vector3> positionList;                     // Lista de posiciones por las se mueve la unidad
    private int currentPositionIndex;                       // Inidice de la posicion actual del movimiento

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

        // Actualizamos la posicion objetivo
        Vector3 targetPosition = positionList[currentPositionIndex];

        // Establecemos la direccion
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        // Rotamos la unidad hacia la direccion
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

        float stoppingDistance = 0.1f;

        // Comprobamos si la distancia al objetivo es mayor a la de parado
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            float moveSpeed = 4f;
            // Movemos la unidad
            transform.position += moveDirection * moveSpeed * Time.deltaTime;



        }
        else
        {

            // La unidad esta sobre la posicion objetivo
            // Actualizamos el indice
            currentPositionIndex++;


            // Comprobamos si es la ultima posicion
            if (currentPositionIndex >= positionList.Count)
            {

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnStopMoving != null)
                {

                    // Lanzamos el evento
                    OnStopMoving(this, EventArgs.Empty);

                }

                // Terminamos la accion
                ActionComplete();

            }

        }

    }

    // @IGM ------------------------------------------
    // Metodo para iniciar el movimiento de la unidad.
    // -----------------------------------------------
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        // Buscamos el camino en posiciones de la malla
        List<GridPosition> pathGridPositionList = Pathfinding.Instance
            .FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        // Inicializamos el indice del movimiento
        currentPositionIndex = 0;

        // Creamos la lista de posiciones
        positionList = new List<Vector3>();

        // Buscamos los puntos de cada posicion de la malla
        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {

            // A�adimos la posicion en la lista
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));

        }

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnStartMoving != null)
        {

            // Lanzamos el evento
            OnStartMoving(this, EventArgs.Empty);

        }

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM -------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde moverse.
    // ------------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {

        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion en la malla de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {

            for (int z = -maxMoveDistance; z <= maxMoveDistance; z ++)
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

                // Comprobamos si la posicion es en la que esta la unidad
                if (unitGridPosition == testGridPosition)
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

                // Comprobamos si la posicion es alcanzable
                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Creamos un multiplicador de distancias
                int pathfindingDistanceMultiplier = 10;

                // Comprobamos si el camino es demasiado largo
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                {

                    // La saltamos
                    continue;

                }

                // Lo a�adimos a la lista
                validGridPositionList.Add(testGridPosition);

            }

        }

        return validGridPositionList;

    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public override string GetActionName()
    {

        return "Move";

    }

    // @IGM ------------------------------------------------------------------------------
    // Funcion para saber que accion de la IA se puede hacer en la poscicion seleccionada.
    // -----------------------------------------------------------------------------------
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        // Calculamos la cantidad de de objetivos en la posicion actual
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        // Devolvemos la accion de la IA
        return new EnemyAIAction
        {

            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10

        };

    }

}

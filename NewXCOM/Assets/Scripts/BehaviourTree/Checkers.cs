using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Checkers : MonoBehaviour
{

    public static Checkers Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Debug.LogError("Hay más de un checker");
            Destroy(gameObject);
        }
    }

    public bool IsCloseToSphere(Unit unit)
    {
        int maxMoveDistance = unit.GetComponent<MoveAction>().MaxMoveDistance();
        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion en la malla de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {

            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
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

                // Comprobamos si no hay un elemento interactuable en la posicion
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null)
                {

                    // La saltamos
                    continue;

                }

                // Si es un objeto de posesion comprobamos si el estado es diferente al suyo
                InteractSphere interactSphere = interactable as InteractSphere;
                if (interactSphere != null &&
                    (int)interactSphere.GetInControlState() == Convert.ToInt32(unit.IsEnemy()))
                {

                    // La saltamos
                    continue;

                }

                // Hemos detectado una esfera
                return true;

            }

        }

        return false;

    }

    public bool IsSphereNearby(Unit unit)
    {
        int maxInteractDistance = 1;
        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {

            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
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

                // Comprobamos si no hay un elemento interactuable en la posicion
                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null)
                {

                    // La saltamos
                    continue;

                }

                // Si es un objeto de posesion comprobamos si el estado es diferente al suyo
                InteractSphere interactSphere = interactable as InteractSphere;
                if (interactSphere != null &&
                    (int)interactSphere.GetInControlState() == Convert.ToInt32(unit.IsEnemy()))
                {

                    // La saltamos
                    continue;

                }

                // Hemos detectado una esfera
                return true;

            }

        }

        return false;

    }


    public int GetRemainingActionPoints(Unit unit)
    {
        return unit.GetActionPoints();
    }

    public bool AreEnemiesNearby(Unit unit)
    {
        return unit.GetComponent<ShootAction>().GetTargetCountAtPosition(unit.GetGridPosition()) > 0;
    }


    public bool IsEnemyPointBlank(Unit unit)
    {
        int maxSwordDistance = 1;
        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {

            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
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

                // Comprobamos si la posicion no tiene unidades dentro
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Recuperamos el target de la posicion
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                // Comprobamos si el target esta en el mismo equipo que la unidad que va a disparar
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {

                    // La saltamos
                    continue;

                }

                return true;

            }

        }

        return false;
    }

    public bool CouldBeKiled(Unit unit)
    {

        List<Unit> enemies = UnitManager.Instance.GetEnemyUnitList();

        foreach (Unit enemy in enemies)
        {
            if ((unit.GetHealthNormalized() * 100f) < enemy.GetComponent<ShootAction>().GetShootDamage())
            {
                return true;
            }
        }

        return false;
    }
}


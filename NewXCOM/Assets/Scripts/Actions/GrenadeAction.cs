using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField] private int maxThrowDistance;                  // Distancia maxima de lanzado
    [SerializeField] private Transform grenadeProjectilePrefab;     // Prefab de la granada
    [SerializeField] LayerMask obstacleLayerMask;               // Capa de los obstaculos

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

    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public override string GetActionName()
    {

        return "Grenade";

    }

    // @IGM ------------------------------------------------------------------------------
    // Funcion para saber que accion de la IA se puede hacer en la poscicion seleccionada.
    // -----------------------------------------------------------------------------------
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = baseAIValue + GetTargetValueAtPosition(gridPosition)
        };

    }

    // @IGM ------------------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde lanzar una granada.
    // -----------------------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {

        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recuperamos la posicion de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {

            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
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

                // Calculamos la distancia de la posicion a probar
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                // Comprobamos si la distancia es mayor a la de diparo
                if (testDistance > maxThrowDistance)
                {

                    // La saltamos
                    continue;

                }

                // Comprobamos si hay un obstaculo alto entre la unidad y el objetivo
                if (ObstacleInTheWay(unitGridPosition, testGridPosition))
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

    // @IGM ---------------------------
    // Metodo para realizar una accion.
    // --------------------------------
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        // Intanciamos la granada
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);

        // Establecemos los valores de la granada
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, onGrenadeBehaviourComplete);

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM -----------------------------------------------------
    // Metodo para indicar que la granada ha cumplido su funcion.
    // ----------------------------------------------------------
    private void onGrenadeBehaviourComplete()
    {

        // Terminamos la accion
        ActionComplete();

    }

    // @IGM ----------------------------------------------------------------
    // Funcion que devuelve el radio de explosion en posiciones de la malla.
    // ---------------------------------------------------------------------
    public int GetGridDamageRadius()
    {

        float damageRadius = grenadeProjectilePrefab
            .GetComponent<GrenadeProjectile>().GetDamageRadius();

        return Mathf.RoundToInt(damageRadius / LevelGrid.Instance.GetCellSize());

    }

    // @IGM ----------------------------------------------------------------------
    // Funcion para saber si hay un obstaculo grande que impida lanzar la granada.
    // ---------------------------------------------------------------------------
    private bool ObstacleInTheWay(GridPosition unitGridPosition, GridPosition targetGridPosition)
    {

        // Obtenemos la posicion de la unidad
        var unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);

        // Obtenemos la posicion del objetivo
        var targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        // Calculamos la direccion
        var directionToTarget = (targetWorldPosition - unitWorldPosition).normalized;

        // Calculamos la distancia al objetivo
        var distanceToTarget = Vector3.Distance(unitWorldPosition, targetWorldPosition);

        // Definimos un offset para poder disparar por encima de obstaculos bajos
        float unitShoulderHeight = 1.7f;

        // Creamos el raycast
        var offset = Vector3.up * unitShoulderHeight;
        var ray = new Ray(unitWorldPosition + offset, directionToTarget);

        // Comprueba si hay un obstaculo en la direccion
        return Physics.Raycast(ray, distanceToTarget, obstacleLayerMask);
    }

    // @IGM ------------------------------------
    // Getter de la distancia maxima de lanzado.
    // -----------------------------------------
    public int GetMaxThrowDistance()
    {

        return maxThrowDistance;

    }

    // @IGM ------------------------------------------------------------
    // Funcion para calcular la mejor posicion donde lanzar una granada.
    // -----------------------------------------------------------------
    public int GetTargetValueAtPosition(GridPosition gridPosition)
    {

        // Reseteamos el valor de la posicion en la malla
        int targetValue = 0;

        // Calculamos el radio de la explosión en casillas
        int explosionRadious = GetGridDamageRadius();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -explosionRadious; x <= explosionRadious; x++)
        {

            for (int z = -explosionRadious; z <= explosionRadious; z++)
            {

                // Calculamos la posición alrededor de la zona de impacto de la granada
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

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

                    // Le restamos valor a esa posicion
                    targetValue -= 15;

                }
                else
                {

                    // Le sumamos valor a esa posicion
                    targetValue += 5;

                }

            }

        }

        return targetValue;

    }

}

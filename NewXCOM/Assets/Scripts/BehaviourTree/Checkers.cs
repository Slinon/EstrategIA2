using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Checkers : MonoBehaviour
{

    public static Checkers Instance { get; private set; }       // Instancia del singleton

    [SerializeField] LayerMask obstacleLayerMask;               // Capa de los obstaculos

    // @GRG ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un Checkers!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

    }

    // @GRG ------------------------------------------------------------------------
    // Funcion para comprobar si estamos a un movimiento de un objeto interactuable.
    // -----------------------------------------------------------------------------
    public bool IsCloseToSphere(Unit unit)
    {

        int maxMoveDistance;

        // Comprobamos si la unidad tiene la accion de movimiento
        if (unit.TryGetComponent(out MoveAction moveAction))
        {

            // Recuperamos la distancia maxima de movimiento
            maxMoveDistance = moveAction.GetMaxMoveDistance();

        }
        else
        {

            // La unidad no se puede mover, por lo que no puede interactuar
            return false;

        }

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

        // No hemos detectado una esfera
        return false;

    }

    // @GRG -------------------------------------------------------------------
    // Funcion para comprobar que tenemos en rango de interacción a una esfera.
    // ------------------------------------------------------------------------
    public bool IsSphereNearby(Unit unit)
    {

        int maxInteractDistance;

        // Comprobamos si la unidad tiene la accion de interactuar
        if (unit.TryGetComponent(out InteractAction interactAction))
        {

            // Recuperamos la distancia maxima de movimiento
            maxInteractDistance = interactAction.GetMaxInteractDistance();

        }
        else
        {

            // La unidad no se puede mover, por lo que no puede interactuar
            return false;

        }

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

        // No hemos detectado ninguna esfera
        return false;

    }

    // @GRG --------------------------------------------------
    // Funcion para saber cuantos puntos de acción nos quedan.
    // -------------------------------------------------------
    public int GetRemainingActionPoints(Unit unit)
    {
        
        // Devolvemos la cantidad de puntos que le quedan ala unidad
        return unit.GetActionPoints();

    }

    // @GRG -------------------------------------
    // Funcion para saber si hay enemigos a tiro.
    // ------------------------------------------
    public bool AreEnemiesNearby(Unit unit)
    {

        // Comprobamos si la unidad tiene la accion de disparo
        if (unit.TryGetComponent(out ShootAction shootAction))
        {

            // Comprobamos si hay enemigos en el rango de disparo
            if (shootAction.GetTargetCountAtPosition(unit.GetGridPosition()) > 0)
            {

                return true;

            }

        }

        // No hay enemigos o la unidad no puede disparar
        return false;

    }

    // @GRG -----------------------------------------------
    // Funcion para comprobar si tenemos un enemigo a mele.
    // ----------------------------------------------------
    public bool IsEnemyPointBlank(Unit unit)
    {

        int maxSwordDistance;

        // Comprobamos si la unidad tiene la accion de interactuar
        if (unit.TryGetComponent(out SwordAction swordAction))
        {

            // Recuperamos la distancia maxima de movimiento
            maxSwordDistance = swordAction.GetMaxSwordDistance();

        }
        else
        {

            // La unidad no puede pegar espadazo, por lo que no tiene enemigos con los que interactuar
            return false;

        }

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

                // Tenemos un enemigo a mele
                return true;

            }

        }

        // No tenemos ningun enemigo a mele
        return false;

    }

    // @GRG --------------------------------------------------------------------
    // Funcion para saber si podemos morir por las unides que nos tienen a tiro.
    // -------------------------------------------------------------------------
    public bool CouldBeKilled(Unit unit)
    {

        // Recuperamos la lista de unidades enemigas
        List<Unit> enemies = UnitManager.Instance.GetEnemyUnitList();

        // Recorremos la lista de unidades enemigas
        foreach (Unit enemy in enemies)
        {

            // Recuperamos las posiciones de las unidades aliadas que tiene el enemigo a tiro
            List<GridPosition> allyUnitsGridPosition = enemy.GetAction<ShootAction>().GetValidActionGridPositionList();

            // Recorremos la lista de las posiciones aliadas
            foreach (GridPosition allyGridPosition in allyUnitsGridPosition)
            {

                // Recuperamos la unidad aliada
                Unit ally = LevelGrid.Instance.GetUnitListAtGridPosition(allyGridPosition)[0];

                // Comprobamos si la unidad aliada es nuestra unidad
                if (ally == unit)
                {

                    // Comrpobamos si el enemigo la puede matar de un tiro
                    if ((unit.GetHealthNormalized() * 100f) < enemy.GetComponent<ShootAction>().GetShootDamage())
                    {

                        return true;

                    }

                }

            }
            
        }

        // Las unidades enemigos no pueden matarnos de un tiro
        return false;

    }

    // @IGM ------------------------------------------------
    // Funcion para saber si es correcto lanzar una granada.
    // -----------------------------------------------------
    public bool IsValidGrenade(Unit unit)
    {

        // Comprobamos si la unidad tiene la accion de lanzar granada
        if (!unit.TryGetComponent(out GrenadeAction grenadeAction))
        {

            // La unidad no puede lanzar granada
            return false;

        }

        int explosionRadious = grenadeAction.GetGridDamageRadius() - 1;
 
        // Recorremos la lista de posiciones en las que se puede lanzar una granada
        foreach (GridPosition gridPosition in grenadeAction.GetValidActionGridPositionList())
        {
            
            // Creamos la cantidad de enemigos y aliados que estan a rango de la explosion
            int enemiesInRange = 0;
            int alliesInRange = 0;

            // Recorremos todas las posiciones validas alrededor de la malla
            for (int x = -explosionRadious; x <= explosionRadious; x++)
            {

                for (int z = -explosionRadious; z <= explosionRadious; z++)
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

                        // Añadimos la unidad a los aliados afectados
                        alliesInRange++;

                    }
                    else
                    {

                        // La unidad es enemiga, la añadimos a los enemigos afectados
                        enemiesInRange++;

                    }

                }

            }

            // Comprobamos si es factible lanzar una granada en esta posicion
            if (alliesInRange == 0 && enemiesInRange > 1)
            {

                return true;

            }

        }

        return false;

    }

    public bool IsValidStructure(Unit unit)
    {

        // Comprobamos si la unidad tiene la accion de construir torre
        if (!unit.TryGetComponent(out BuildStructureAction buildStructureAction))
        {

            // La unidad no puede lanzar granada
            return false;

        }

        // Calculamos la distancia de la torre
        int maxTurretRange = buildStructureAction.GetStructure()
            .GetComponentInChildren<ShootAction>().GetMaxShootDistance();

        // Recorremos la lista de posiciones en las que se puede construir una torre
        foreach (GridPosition gridPosition in buildStructureAction.GetValidActionGridPositionList())
        {

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
            Debug.Log(unit.name + ": " + allyTurretsInRange);
            if (allyTurretsInRange == 0 && enemiesInRange > 2)
            {

                return true;

            }

        }

        return false;

    }

    // @IGM ----------------------------------------------------------------------
    // Funcion para saber si la unidad esta colocada en la mejor posicion posible.
    // ---------------------------------------------------------------------------
    public bool UnitInBestPosition(Unit unit)
    {

        // Comprobamos si la unidad tiene accion de movimiento
        if (!unit.TryGetComponent(out MoveAction unitMoveAction))
        {

            // Evitamos hacer todo el calculo
            return true;

        }

        // Recumeramos el valor de la posicion actual de la unidad
        int unitGridPositionValue = unitMoveAction.GetTargetValueAtPosition(unit.GetGridPosition());

        // Recorremos la lista de posibles posiciones de la unidad
        foreach (GridPosition gridPosition in unitMoveAction.GetValidActionGridPositionList())
        {

            // Comprobamos que el valor de la posible posicion sea mayor que el valor de la posicion de la unidad
            if (unitGridPositionValue < unitMoveAction.GetTargetValueAtPosition(gridPosition))
            {

                return false;

            }

        }

        return true;

    }

}


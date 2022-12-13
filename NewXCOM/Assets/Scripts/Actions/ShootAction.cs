using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    // @IGM ------------------------------------------
    // Maquina de estados de de la accion de disparar.
    // -----------------------------------------------
    private enum State
    {

        Aiming,
        Shooting,
        Cooloff,

    }
    private ProbabilitySystem ps;                               // Funci�n probabilidad @EMF

    public event EventHandler<Unit> OnShoot;                    // Evento cuando la unidad dispara
    public static event EventHandler<Unit> OnAnyShoot;          // Evento cuando cualquier unidad dispara

    [SerializeField] private int maxShootDistance;              // Distancia maxima de disparo
    [SerializeField] LayerMask obstacleLayerMask;               // Capa de los obstaculos

    [SerializeField] private int shootDamage = 40;              // Da�o del disparo @EMF
    [SerializeField] private int hitProbability = 80;           // Probabilidad de acierto @EMF
    [SerializeField] private int criticalProbability = 50;      // Probabilidad de cr�tico @EMF
    [SerializeField] private float criticalPercentage = 0.2f;   // Porcentaje cr�tico @EMF

    private State state;                                        // Estado actual de la accion
    private Unit targetUnit;                                    // Unidad ala que vamos a disparar
    private bool canShootBullet;                                // Booleano para indicar que la unidad puede disparar
    private float stateTimer;                                   // Timer de la maquina de estados

    private UnitManager unitManager;
    private Pathfinding pathFinding;



    private void Start()
    {
        ps = ProbabilitySystem.Instance;
        unitManager = UnitManager.Instance;
        pathFinding = Pathfinding.Instance;
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

            case State.Aiming:

                //Calculamos la direccion de apuntado
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                // Rotamos la unidad hacia la direccion
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);

                break;
            case State.Shooting:

                // Comprobamos si podemos disparar
                if (canShootBullet)
                {

                    // Disparamos
                    Shoot();
                    canShootBullet = false;

                }

                break;
            case State.Cooloff:
                break;

        }

        if (stateTimer <= 0)
        {

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

            case State.Aiming:

                // Cambiamos el estado y los parametros
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;

                break;
            case State.Shooting:

                // Cambiamos el estado y los parametros
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;

                break;
            case State.Cooloff:

                // Terminamos la accion
                ActionComplete();

                break;

        }

    }

    // @IGM ----------------
    // Metodo para disparar.
    // ---------------------
    private void Shoot()
    {

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyShoot != null)
        {

            // Lanzamos el evento
            OnAnyShoot(this, targetUnit);

        }

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnShoot != null)
        {

            // Lanzamos el evento
            OnShoot(this, targetUnit);

        }

        //  Hacemos daño a la unidad

        // Debug ------------------------------------------------------------------

        

        Vector2 damagetmp = (ps.CheckDamageProbability(shootDamage, criticalProbability,
            criticalPercentage, hitProbability, unit.GetDistanceBetweenUnits(this.unit, targetUnit), maxShootDistance));

        int porcentaje_acierto = ps.GetProbabiltyByDistance(hitProbability, unit.GetDistanceBetweenUnits(this.unit, targetUnit), maxShootDistance);

        targetUnit.Damage(damagetmp);

        Debug.Log("damage: " + damagetmp.x + " tipo: " + damagetmp.y + " distancia: " + pathFinding.CalculateDistance(this.unit.GetGridPosition(), targetUnit.GetGridPosition()) / 10 + " %: " + porcentaje_acierto);

        // Debug -------------------------------------------------------------------
    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public override string GetActionName()
    {
        return "Shoot";
    }

    // @IGM --------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde disparar.
    // -------------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {

        // Recuperamos la posicion en la malla de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Buscamos las posiciones validas donde disparar
        return GetValidActionGridPositionList(unitGridPosition);

    }

    // @IGM --------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde disparar.
    // -------------------------------------------------------------------
    private List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        // Creamos la lista
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
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
                if (testDistance > maxShootDistance)
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

                // Definimos un offset para poder disparar por encima de obstaculos bajos
                float unitShoulderHeight = 1.7f;

                // Calculamos la direccion de disparo
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Unit unitPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                // Definimos un offset para poder disparar por encima de obstaculos bajos

                if(targetUnit.GetCoverType() == CoverType.Covered)
                {
                    if(unitPosition.GetCoverType() == CoverType.Covered)
                    {
                        unitShoulderHeight = 0.6f;
                    }else{unitShoulderHeight = 1.7f;}         
                }

                // Comprobamos si la unidad no tiene visual del objetivo
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDirection,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstacleLayerMask))
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

    // @IGM ----------------------------------
    // Metodo para hacer disparar a la unidad.
    // ---------------------------------------
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        // Asignamos la unidad a la que vamos a disparar
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        // Establecemos los parametros iniciales
        state = State.Aiming;
        canShootBullet = true;
        float aimingStateTime = 0.5f;
        stateTimer = aimingStateTime;

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM ---------------------------
    // Getter del objetivo del disparo.
    // --------------------------------
    public Unit GetTargetUnit()
    {

        return targetUnit;

    }

    // @IGM ------------------------------------
    // Getter de la distancia maxima de disparo.
    // -----------------------------------------
    public int GetMaxShootDistance()
    {

        return maxShootDistance;

    }

    // @IGM ------------------------------------------------------------------------------
    // Funcion para saber que accion de la IA se puede hacer en la poscicion seleccionada.
    // -----------------------------------------------------------------------------------
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        // Recuperamos la unidad objetivo
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        // Devolvemos la accion de la IA
        return new EnemyAIAction
        {

            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)

        };

    }

    // @IGM --------------------------------------------
    // Funcion para saber cuantas unidades estan a tiro.
    // -------------------------------------------------
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {

        return GetValidActionGridPositionList(gridPosition).Count;

    }

    public int GetShootHitProbability()
    {
        return hitProbability;
    }
}
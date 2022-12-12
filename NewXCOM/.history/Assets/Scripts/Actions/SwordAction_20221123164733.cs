using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{

    // @IGM -------------------------------------------------
    // Maquina de estados de de la accion de dar un espadazo.
    // ------------------------------------------------------
    private enum State
    {

        SwingingSwordBeforeHit,
        SwingingSwordAfterHit

    }
    private ProbabilitySystem ps;                        // Funci�n probabilidad @EMF

    public event EventHandler OnSwordActionStarted;     // Evento cuando la accion de espadazo empieza
    public event EventHandler OnSwordActionCompleted;   // Evento cuando la accion de espadazo termina
    public static event EventHandler OnAnySwordHit;     // Evento cuando cualquier espadazo golpea

    [SerializeField] private int maxSwordDistance;      // Distancia maxima de dar un espadazo
    [SerializeField] private int swordDamage;           // Da�o del espadazo

    [SerializeField] private int criticalProbability = 15;      // Probabilidad de cr�tico @EMF
    [SerializeField] private float criticalPercentage = 0.2f;   // Porcentaje cr�tico @EMF

    private State state;                                // Estado actual de la accion
    private float stateTimer;                           // Timer de la maquina de estados
    private Unit targetUnit;                            // Unidad a la que vamos a disparar


    private void Start()
    {
        ps = ProbabilitySystem.Instance;
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

            case State.SwingingSwordBeforeHit:

                //Calculamos la direccion de apuntado
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                // Rotamos la unidad hacia la direccion
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);

                break;
            case State.SwingingSwordAfterHit:

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

            case State.SwingingSwordBeforeHit:

                // Cambiamos el estado y los parametros
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;

                // Da�amos a la unidad objetivo
                targetUnit.Damage(ps.CheckDamageProbability(swordDamage, criticalProbability, criticalPercentage));

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnAnySwordHit != null)
                {

                    // Lanzamos el evento
                    OnAnySwordHit(this, EventArgs.Empty);

                }

                break;

            case State.SwingingSwordAfterHit:

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnSwordActionCompleted != null)
                {

                    // Lanzamos el evento
                    OnSwordActionCompleted(this, EventArgs.Empty);

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

        return "Sword";

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
            actionValue = 200,

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

        // Establecemos los parametros iniciales
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnSwordActionStarted != null)
        {

            // Lanzamos el evento
            OnSwordActionStarted(this, EventArgs.Empty);

        }

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM -------------------------------------
    // Getter de la distancia maxima de espadazo.
    // ------------------------------------------
    public int GetMaxSwordDistance()
    {

        return maxSwordDistance;

    }

    // @IGM ---------------------------
    // Getter del objetivo del disparo.
    // --------------------------------
    public Unit GetTargetUnit()
    {

        return targetUnit;

    }

}
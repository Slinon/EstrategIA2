using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;        // Evento cuando empieza una accion
    public static event EventHandler OnAnyActionCompleted;      // Evento cuando empieza una accion

    protected Unit unit;                                        // Unidad que se tiene que mover
    protected bool isActive;                                    // Booleano para indicar si la accion se ha activado
    protected Action onActionComplete;                          // Accion para cambiar el esrtado del actionSystem

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    protected virtual void Awake()
    {

        // Asignamos la unidad
        unit = GetComponent<Unit>();

    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public abstract string GetActionName();

    // @IGM ---------------------------
    // Metodo para realizar una accion.
    // --------------------------------
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    // @IGM -------------------------------------------
    // Funcion para comprobar si la posicion es valida.
    // ------------------------------------------------
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {

        // Recuperamos la lista de posiciones validas
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();

        // Comprobamos si la posicion esta en la lista
        return validGridPositionList.Contains(gridPosition);

    }

    // @IGM -------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde moverse.
    // ------------------------------------------------------------------
    public abstract List<GridPosition> GetValidActionGridPositionList();

    // @IGM ---------------------------------------
    // Funcion que devuelve el coste de las accion.
    // --------------------------------------------
    public virtual int GetActionPointsCost()
    {

        return 1;

    }

    // @IGM --------------------------
    // Metodo para empezar una accion.
    // -------------------------------
    protected void ActionStart(Action onActionComplete)
    {

        //Iniciamos la accion
        isActive = true;
        this.onActionComplete = onActionComplete;

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyActionStarted != null)
        {

            // Lanzamos el evento
            OnAnyActionStarted(this, EventArgs.Empty);

        }

    }

    // @IGM ---------------------------
    // Metodo para terminar una accion.
    // --------------------------------
    protected void ActionComplete()
    {

        // Finalizamos la accion
        isActive = false;
        onActionComplete();

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyActionCompleted != null)
        {

            // Lanzamos el evento
            OnAnyActionCompleted(this, EventArgs.Empty);

        }

    }

    // @IGM ---------------
    // Getter de la unidad.
    // --------------------
    public Unit GetUnit()
    {

        return unit;

    }

    // @IGM --------------------------------------------------------
    // Funcion para saber cual es la mejor accion a tomar por la IA.
    // -------------------------------------------------------------
    public EnemyAIAction GetBestEnemyAIAction()
    {

        // Creamos la lista de acciones disponibles de la IA
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        // Creamos la lista de posiciones validas
        List<GridPosition> validActionPositionList = GetValidActionGridPositionList();

        // Recorremos la lista de posiciones validas
        foreach (GridPosition gridPosition in validActionPositionList)
        {

            // Creamos la accion de la IA
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);

            // La añadimos en la IA
            enemyAIActionList.Add(enemyAIAction);

        }

        // Comprobamos que hay acciones en la lista
        if (enemyAIActionList.Count > 0)
        {

            // Ordenamos la lista por el valor de la accion
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);

            // Devolvemos la accion con mejor coste
            return enemyAIActionList[0];

        }
        else
        {

            // No hay acciones posible de la IA
            return null;

        }

    }

    // @IGM ------------------------------------------------------------------------------
    // Funcion para saber que accion de la IA se puede hacer en la poscicion seleccionada.
    // -----------------------------------------------------------------------------------
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    // @GRG ------------------------------------------------------------------------------
    // Funcion para saber si esta acción está ligada a un coste monetario del moneySystem
    // -----------------------------------------------------------------------------------
    public virtual bool ThisActionCostsMoney()
    {
        return false;
    }

    public virtual int MoneyCost()
    {
        return 0;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    // IGM ------------------------
    // Maquina de estados de la IA.
    // ----------------------------
    private enum State
    {

        WaitingForEnemyTurn,
        TakingTurn,
        Busy

    }

    private State state;                                    // Estado actual de la IA
    private float timer;

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Establecemos el estado inicial
        state = State.WaitingForEnemyTurn;

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {

        // Comprobamos si es el turno del enemigo
        if (TurnSystem.Instance.IsPlayerTurn())
        {

            // No hacemos nada
            return;

        }

        // Comprobamos el estado
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {

                    // Comprobamos si podemos realizar una accion enemiga
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {

                        // Cambiamos el estado a ocupado
                        state = State.Busy;

                    }
                    else
                    {

                        // No quedan enemigos con acciones que puedan hacer
                        // Terminamos el tueno del enemigo
                        TurnSystem.Instance.NextTurn();

                    }
  
                }
                break;
            case State.Busy:
                break;
            default:
                break;
        }

    }

    // @IGM ---------------------------------------------------
    // Metodo para establecer el estado a ejecturando el turno.
    // --------------------------------------------------------
    private void SetStateTakingTurn()
    {

        timer = 0.5f;
        state = State.TakingTurn;

    }

    // @IGM --------------------------------------
    // Handler del eventoi cuando cambia el turno.
    // -------------------------------------------
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {

        if (!TurnSystem.Instance.IsPlayerTurn())
        {

            state = State.TakingTurn;
            timer = 2f;

        }

    }

    // @IGM ---------------------------------------------
    // Funcion para intentar ejecutar una accion enemiga.
    // --------------------------------------------------
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {

        // Recorremos la lista de unidades enemigas
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {

            // Intentamos realizar una accion enemiga sobre la unidad
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {

                return true;

            }

        }

        // No podemos realizar mas acciones
        return false;

    }

    // @IGM ---------------------------------------------
    // Funcion para intentar ejecutar una accion enemiga.
    // --------------------------------------------------
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {

        // Creamos la mejor accion que puede hacer la IA
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        // Recorremos la lista de posibles acciones
        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {

            // Comprobamos si el enemigo no se puede costear la accion
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {

                // Omitimos la accion
                continue;

            }

            // Comprobamos si es la primera accion potencial a ser la mejor
            if (bestEnemyAIAction == null)
            {

                // Asignamos la accion
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;

            }
            else
            {

                // Comprobamos si la nueva accion es mejor que la anterior
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {

                    // Asignamos la accion
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;

                }

            }

        }

        // Comprobamos si tenemos la mejor accion seleccionada y podemos costearla
        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {

            // Lanzamos la accion
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;

        }
        else
        {

            // No hay acciones que se puedan tomar
            return false;

        }

    }

}

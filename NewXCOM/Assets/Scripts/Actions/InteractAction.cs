using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{

    public static event EventHandler OnAnyInteractionCompleted; // Evento cuando una interaccion ha terminado

    [SerializeField] private int maxInteractDistance;           // Distancia maxima de dar un espadazo

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

        return "Interact";

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
            actionValue = baseAIValue + GetTargetValueAtPosition(gridPosition)

        };

    }

    // @IGM -------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde moverse.
    // -----------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {

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

                // Lo añadimos a la lista
                validGridPositionList.Add(testGridPosition);

            }

        }

        return validGridPositionList;

    }

    // @IGM ------------------------------------
    // Metodo para hacer la accion de la unidad.
    // -----------------------------------------
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        // Seleccionamos la puerta
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);

        // Interactuamos con ella
        interactable.Interact(OnInteractComplete, unit);

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM -----------------------------------
    // Getter del coste en puntos de la accion.
    // ----------------------------------------
    public override int GetActionPointsCost()
    {

        return 1;

    }

    // @IGM -------------------------------------------------
    // Metodo para notificar que la interaccion ha terminado.
    // ------------------------------------------------------
    private void OnInteractComplete()
    {

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyInteractionCompleted != null)
        {

            // Lanzamos el evento
            OnAnyInteractionCompleted(this, EventArgs.Empty);

        }

        // Terminamos la accion
        ActionComplete();

    }

    // @GRG ----------------------------------------
    // Getter de la distancia maxima de interaccion.
    // ---------------------------------------------
    public int GetMaxInteractDistance()
    {

        return maxInteractDistance;

    }

    // @IGM ------------------------------------------------
    // Funcion para calcular la mejor posicion de la accion.
    // -----------------------------------------------------
    public override int GetTargetValueAtPosition(GridPosition gridPosition)
    {

        return 0;

    }

}

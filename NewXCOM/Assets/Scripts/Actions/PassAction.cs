using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassAction : BaseAction
{

    private float timeElapsed;                             


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

        // Rotamos la unidad
        timeElapsed += Time.deltaTime;

        // Comprobamos si ya se ha girado una vuelta completa
        if (timeElapsed >= 1f)
        {

            // Terminamos la accion
            ActionComplete();

        }

    }

    // @IGM ---------------------------
    // Metodo para realizar una accion.
    // --------------------------------
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        Debug.Log(unit.name + " pasa este turno");
        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public override string GetActionName()
    {

        return "Pass";

    }

    // @IGM -------------------------------------------------------------
    // Funcion que devuelve la lista de posiciones validas donde moverse.
    // ------------------------------------------------------------------
    public override List<GridPosition> GetValidActionGridPositionList()
    {

        // Almacenamos la posicion de la unidad
        GridPosition unitGridPosition = unit.GetGridPosition();

        // Devolvemos la lista con la posicion de la unidad
        return new List<GridPosition> { unitGridPosition };


    }

    // @IGM -----------------------------------
    // Getter del coste en puntos de la accion.
    // ----------------------------------------
    public override int GetActionPointsCost()
    {
        return 1;
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

    // @IGM ------------------------------------------------
    // Funcion para calcular la mejor posicion de la accion.
    // -----------------------------------------------------
    public override int GetTargetValueAtPosition(GridPosition gridPosition)
    {

        return 0;

    }

}

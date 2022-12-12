using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

    private float totalSpinAmount;                             // Cantidad total de giro de la unidad


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
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;

        // Comprobamos si ya se ha girado una vuelta completa
        if (totalSpinAmount >= 360f)
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

        // Establecemos el valor de giro a 0
        totalSpinAmount = 0;

        // Empezamos la accion
        ActionStart(onActionComplete);

    }

    // @IGM -------------------------------------
    // Funcion para saber el nombre de la accion.
    // ------------------------------------------
    public override string GetActionName()
    {

        return "Spin";

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    // @IGM --------------------------
    // Enumerador de estados de la IA.
    // -------------------------------
    public enum StateBehaviour
    {

        UltraDefensive,
        Defensive,
        Neutral,
        Ofensive

    }

    [SerializeField] private int baseMagnitude;                     // Importancia de las bases en los comportamientos de la IA
    [SerializeField] private int spheresMagnitude;                  // Importancia de los puntos de toma para la IA
    [SerializeField] private HeatMapMaker heatMapMaker;             // Creador de los mapas de influencia
    [SerializeField] private InteractSphere closeSphere;            // Esfera mas cercana a la base de la IA
    [SerializeField] private InteractSphere farSphere;              // Esfera mas lejada a la base de la IA

    private StateBehaviour state;                                   // Estado de comportamiento general de la IA

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        InteractAction.OnAnyInteractionCompleted += InteractAction_OnAnyInteractionCompleted;
        Unit.OnAnyUnitMoved += Unit_OnAnyUnitMoved;

        // Empezamos con el modo defensivo
        StartDefensiveMode();

    }

    // @IGM ----------------------------------------------------------
    // Handler del evento cuando una unidad interactua con una esfera.
    // ---------------------------------------------------------------
    private void InteractAction_OnAnyInteractionCompleted(object sender, EventArgs empty)
    {

        // Elegimos el estado conveniente
        ChooseState();

    }

    // @IGM -----------------------------------------
    // Handler del evento cuando una unidad se mueve.
    // ----------------------------------------------
    private void Unit_OnAnyUnitMoved(object sender, EventArgs empty)
    {

        // Comporbamos si hay peligro para la base aliada
        if (IsPlayerOffensiveModeOn())
        {

            // Modo ultra defensivo
            state = StateBehaviour.UltraDefensive;
            StartUltraDefensiveMode();

        }
        else
        {

            // No hay peligro, elegimos el estado dependiendo de las tomas
            ChooseState();

        }

    }

    // @IGM ---------------------------------
    // Metodo para elegir el estado correcto.
    // --------------------------------------
    private void ChooseState()
    {

        // Comprobamos si la esfera mas cercana no está bajo nuestro control
        if (closeSphere.GetInControlState() != InteractSphere.InControlState.Enemy)
        {

            // Modo defensivo
            state = StateBehaviour.Defensive;
            StartDefensiveMode();

        }
        else if (farSphere.GetInControlState() != InteractSphere.InControlState.Enemy)
        {

            // Esfera cercana bajo control IA pero esfera lejana fuera de control
            // Modo neutral
            state = StateBehaviour.Neutral;
            StartNeutralMode();

        }
        else
        {

            // Dos esferas bajo control de la IA
            // Modo ofensivo
            state = StateBehaviour.Ofensive;
            StartOfensiveMode();

        }

    }

    // @IGM --------------------------------------------
    // Metodo para gestionar el modo ultra defensivo de la IA.
    // -------------------------------------------------
    private void StartUltraDefensiveMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(-baseMagnitude);

        // Establecemos valor a las esferas
        heatMapMaker.SetSpheresMoveValue(0);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(baseMagnitude);

    }

    // @IGM --------------------------------------------
    // Metodo para gestionar el modo defensivo de la IA.
    // -------------------------------------------------
    private void StartDefensiveMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(-baseMagnitude);

        // Establecemos valor a las esferas
        heatMapMaker.SetSpheresMoveValue(spheresMagnitude);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(baseMagnitude);

    }

    // @IGM ------------------------------------------
    // Metodo para gestionar el modo neutral de la IA.
    // -----------------------------------------------
    private void StartNeutralMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(-baseMagnitude);

        // Establecemos valor a las esferas
        heatMapMaker.SetSpheresMoveValue(spheresMagnitude);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(-baseMagnitude);

    }

    // @IGM -------------------------------------------
    // Metodo para gestionar el modo ofensivo de la IA.
    // ------------------------------------------------
    private void StartOfensiveMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(baseMagnitude);

        // Establecemos valor a las esferas
        heatMapMaker.SetSpheresMoveValue(spheresMagnitude);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(-baseMagnitude);

    }

    // @IGM ----------------------------------
    // Funcion para detectar ofensiva enemiga.
    // ---------------------------------------
    private bool IsPlayerOffensiveModeOn()
    {

        // Recorremos las posiciones de la malla en las que está situada la base
        int minHeight = LevelGrid.Instance.GetHeight() - 9;
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = minHeight; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Recuperamos la posicion de esa casilla
                GridPosition gridPosition = new GridPosition(x, z);

                // Comprobamos si en esa posicion no ha una unidad
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition))
                {

                    continue;

                }

                // Comporbamos si la unidad es aliada
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
                if (targetUnit.IsEnemy())
                {

                    continue;

                }

                // Hay una unidad enemiga cerca de la base
                return true;

            }

        }

        return false;

    }

    // @IGM ----------------------------------------
    // Getter del estado de comportamiento de la IA.
    // ---------------------------------------------
    public StateBehaviour GetState()
    {

        return state;

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] HeatMapMaker heatMapMaker;             // Creador de los mapas de influencia
    [SerializeField] InteractSphere closeSphere;            // Esfera mas cercana a la base de la IA
    [SerializeField] InteractSphere farSphere;              // Esfera mas lejada a la base de la IA

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        InteractAction.OnAnyInteractionCompleted += InteractAction_OnAnyInteractionCompleted;

        // Empezamos con el modo defensivo
        StartDefensiveMode();

    }

    // @IGM ----------------------------------------------------------
    // Handler del evento cuando una unidad interactua con una esfera.
    // ---------------------------------------------------------------
    private void InteractAction_OnAnyInteractionCompleted(object sender, EventArgs empty)
    {

        // Comprobamos si la esfera mas cercana no está bajo nuestro control
        if (closeSphere.GetInControlState() != InteractSphere.InControlState.Enemy)
        {

            // Modo defensivo
            StartDefensiveMode();

        }
        else if (farSphere.GetInControlState() != InteractSphere.InControlState.Enemy)
        {

            // Esfera cercana bajo control IA pero esfera lejana fuera de control
            // Modo neutral
            StartNeutralMode();

        }
        else
        {

            // Dos esferas bajo control de la IA
            // Modo ofensivo
            StartOfensiveMode();

        }
        

    }

    // @IGM --------------------------------------------
    // Metodo para gestionar el modo defensivo de la IA.
    // -------------------------------------------------
    private void StartDefensiveMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(-20);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(20);

    }

    // @IGM ------------------------------------------
    // Metodo para gestionar el modo neutral de la IA.
    // -----------------------------------------------
    private void StartNeutralMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(-20);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(-20);

    }

    // @IGM -------------------------------------------
    // Metodo para gestionar el modo ofensivo de la IA.
    // ------------------------------------------------
    private void StartOfensiveMode()
    {

        // Ponemos valor negativo a la base enemiga
        heatMapMaker.SetEnemyBaseMoveValue(20);

        // Ponemos valor positivo a la base aliada
        heatMapMaker.SetAllyBaseMoveValue(-20);

    }

}

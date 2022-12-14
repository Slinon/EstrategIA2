using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAIManager : MonoBehaviour
{
    Unit enemyBase;
    [SerializeField] private int maxAIValueAction;                  // Valor maximo de una accion para la IA
    [SerializeField] private int minAIValueAction;                  // Valor minimode una accion para la IA

    private int soldierCost, scoutCost, granadierCost, engineerCost, berserkCost, juggernautCost;

    // @GRG ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos la unidad
        enemyBase = GetComponent<Unit>();

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        EnemyAI.OnAnyAIUnitIsSelected += EnemyAI_OnAnyAIUnitIsSelected;

        //Conseguir los costes
        GetUnitCosts();

    }

    // @IGM ------------------------------------------------------------
    // Handler del evento cuando se ha seleccionado una unidad de la IA.
    // -----------------------------------------------------------------
    private void EnemyAI_OnAnyAIUnitIsSelected(object sender, EventArgs empty)
    {

        // Asignamos los valores de las acciones
        CheckBaseConditions();
       
    }


    //------------------------------
    //@GRG Comprobar las condiciones
    //------------------------------
    private void CheckBaseConditions()
    {

        // Comprobamos si la unidad es una unidad del jugador
        if (enemyBase == null || !enemyBase.IsEnemy())
        {

            return;

        }

        // Reestablecemos los valores de las acciones
        RestartValues();


        //Si estoy pobre...
        if (!BaseCheckers.Instance.CanSpawnAnyUnit())
        {
            //Doy la media vuelta, danza kuduro.
            Debug.Log("No tengo dinero para spawnear más tropas, paso");

            SetValues(enemyBase.GetComponent<PassAction>(), maxAIValueAction, minAIValueAction);  

            return;
        }

        //Si tengo dinero para plantar...
        else
        {
            //Si tengo demasiadas tropas
            if (BaseCheckers.Instance.AmountOfUnitsDeployed() >= 8)
            {
                //Ahorro
                Debug.Log("Tengo demasiadas tropas en juego, ahorro.");

                SetValues(enemyBase.GetComponent<PassAction>(), maxAIValueAction, minAIValueAction);

                return;
            }


            else if (BaseCheckers.Instance.AmountOfUnitsDeployed() >= 4)
            {
                Debug.Log("Tengo una cantidad de tropas aceptable, voy a plantar la más cara");

                //Juggernaut
                if (MoneySystem.Instance.enemyAI.money >= juggernautCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnJuggernaut>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Berserk
                else if (MoneySystem.Instance.enemyAI.money >= berserkCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnBerserk>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Engineer
                else if (MoneySystem.Instance.enemyAI.money >= engineerCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnEngineer>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Granadier
                else if (MoneySystem.Instance.enemyAI.money >= granadierCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnGranadier>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Scout
                else if (MoneySystem.Instance.enemyAI.money >= scoutCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnScout>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Soldadito
                else
                {
                    SetValues(enemyBase.GetComponent<SpawnSoldier>(), maxAIValueAction, minAIValueAction);
                    return;
                }
               
            }

            else
            {
                Debug.Log("Tengo muy pocas unidades en juego, voy a plantar varias unidades baratujas");

                SetValues(enemyBase.GetComponent<SpawnSoldier>(), maxAIValueAction, minAIValueAction);

                return;
            }

        }
    }


    // @GRG ------------------------------------------------
    // Metodo para reestablecer los valores de las acciones.
    // -----------------------------------------------------
    private void RestartValues()
    {

        // Recorremos la lista de acciones validas
        foreach (BaseAction action in enemyBase.GetBaseActionArray())
        {

            // Establecemos su valor de IA base a 0
            action.SetBaseAIValue(0);

        }

    }

    // @IGM ----------------------------------------------
    // Metodo para establecer los valores de las acciones.
    // ---------------------------------------------------
    private void SetValues(BaseAction chosenAction, int maxValue, int minValue)
    {

        // Recorremos la lista de acciones validas
        foreach (BaseAction action in enemyBase.GetBaseActionArray())
        {

            if (action == chosenAction)
            {
                Debug.Log(chosenAction.GetActionName() + action.GetActionName());
                action.SetBaseAIValue(maxValue);

            }
            else
            {

                action.SetBaseAIValue(minValue);

            }

        }

    }

    //------------------------------
    //@GRG Fetchear el coste de cada tropa
    //------------------------------
    public void GetUnitCosts()
    {
        soldierCost = enemyBase.GetComponent<SpawnSoldier>().MoneyCost();
        scoutCost = enemyBase.GetComponent<SpawnScout>().MoneyCost();
        granadierCost = enemyBase.GetComponent<SpawnGranadier>().MoneyCost();
        engineerCost = enemyBase.GetComponent<SpawnEngineer>().MoneyCost();
        berserkCost = enemyBase.GetComponent<SpawnBerserk>().MoneyCost();
        juggernautCost = enemyBase.GetComponent<SpawnJuggernaut>().MoneyCost();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAIManager : MonoBehaviour
{

    public static event EventHandler<Unit> OnAnyUnitSpawnedAction;  // Evento cuando se lanza una accion 

    [SerializeField] private int maxAIValueAction;                  // Valor maximo de una accion para la IA
    [SerializeField] private int minAIValueAction;                  // Valor minimode una accion para la IA
    [SerializeField] private EnemyManager enemyManager;             // Manager de la IA general

    private Unit enemyBase;
    private int scoutCost, granadierCost, engineerCost, berserkCost, juggernautCost;

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

            // Si tengo menos de 3 unidades en juego
            if (BaseCheckers.Instance.AmountOfUnitsDeployed() < 3)
            {

                Debug.Log("Tengo muy pocas unidades en juego, voy a plantar varias unidades baratujas");

                SetValues(enemyBase.GetComponent<SpawnScout>(), maxAIValueAction, minAIValueAction);

                return;

            }
            else
            {

                // Tengo muchas unidades en juego
                // Compruebo el estado de la IA general
                if (enemyManager.GetState() == EnemyManager.StateBehaviour.UltraDefensive 
                    || enemyManager.GetState() == EnemyManager.StateBehaviour.Defensive)
                {

                    // Estado ultra defensivo o defensivo
                    //Engineer
                    if (MoneySystem.Instance.enemyAI.money >= engineerCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnEngineer>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    //Granadier
                    else if (MoneySystem.Instance.enemyAI.money >= granadierCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnGranadier>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    //Scout
                    else if (MoneySystem.Instance.enemyAI.money >= scoutCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnScout>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    // No dineros
                    else
                    {

                        //Doy la media vuelta, danza kuduro.
                        Debug.Log("No tengo dinero para spawnear más tropas, paso");
                        SetValues(enemyBase.GetComponent<PassAction>(), maxAIValueAction, minAIValueAction);
                        return;

                    }

                }
                else if(enemyManager.GetState() == EnemyManager.StateBehaviour.Neutral)
                {

                    // Estado neutral
                    //Berserk
                    if (MoneySystem.Instance.enemyAI.money >= berserkCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnBerserk>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    //Engineer
                    else if (MoneySystem.Instance.enemyAI.money >= engineerCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnEngineer>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    // No dineros
                    else
                    {

                        //Doy la media vuelta, danza kuduro.
                        Debug.Log("No tengo dinero para spawnear más tropas, paso");
                        SetValues(enemyBase.GetComponent<PassAction>(), maxAIValueAction, minAIValueAction);
                        return;

                    }

                }
                else if (enemyManager.GetState() == EnemyManager.StateBehaviour.Ofensive)
                {

                    //Juggernaut
                    if (MoneySystem.Instance.enemyAI.money >= juggernautCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnJuggernaut>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    //Berserk
                    else if (MoneySystem.Instance.enemyAI.money >= berserkCost)
                    {
                        SetValues(enemyBase.GetComponent<SpawnBerserk>(), maxAIValueAction, minAIValueAction);
                        return;
                    }

                    // No dineros
                    else
                    {

                        //Doy la media vuelta, danza kuduro.
                        Debug.Log("No tengo dinero para spawnear más tropas, paso");
                        SetValues(enemyBase.GetComponent<PassAction>(), maxAIValueAction, minAIValueAction);
                        return;

                    }

                }

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
        scoutCost = enemyBase.GetComponent<SpawnScout>().MoneyCost();
        granadierCost = enemyBase.GetComponent<SpawnGranadier>().MoneyCost();
        engineerCost = enemyBase.GetComponent<SpawnEngineer>().MoneyCost();
        berserkCost = enemyBase.GetComponent<SpawnBerserk>().MoneyCost();
        juggernautCost = enemyBase.GetComponent<SpawnJuggernaut>().MoneyCost();
    }

}

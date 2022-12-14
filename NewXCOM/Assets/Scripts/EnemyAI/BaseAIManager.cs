using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAIManager : MonoBehaviour
{
    Unit enemyBase;
    [SerializeField] private int maxAIValueAction;                  // Valor maximo de una accion para la IA
    [SerializeField] private int minAIValueAction;                  // Valor minimode una accion para la IA

    private int soldierCost, granadierCost, scoutCost, ingenieerCost, berserkCost, juggernautCost;

    // @GRG ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos la unidad
        enemyBase = GetComponent<Unit>();

        //Implementar una forma de obtener el coste de las tropas

    }


    //------------------------------
    //@GRG Comprobar las condiciones
    //------------------------------
    private void CheckBaseConditions()
    {
        //Si estoy pobre...
        if (!BaseCheckers.Instance.CanSpawnAnyUnit())
        {
            //Doy la media vuelta, danza kuduro.
            Debug.Log("No tengo dinero para spawnear más tropas, paso");

            SetValues(enemyBase.GetComponent<SpinAction>(), maxAIValueAction, minAIValueAction);  

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

                SetValues(enemyBase.GetComponent<SpinAction>(), maxAIValueAction, minAIValueAction);

                return;
            }


            else if (BaseCheckers.Instance.AmountOfUnitsDeployed() >= 4)
            {
                Debug.Log("Tengo una cantidad de tropas aceptable, voy a plantar la más cara");

                //Lógica para comprobar cual es la mejor tropa.
                //Tal vez podria hacer un switch, pero parece más sencillo así

                //Juggernaut
                if (MoneySystem.Instance.enemyAI.money >= juggernautCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Berserk
                else if (MoneySystem.Instance.enemyAI.money >= berserkCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Ingenieer
                else if (MoneySystem.Instance.enemyAI.money >= ingenieerCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Granadier
                else if (MoneySystem.Instance.enemyAI.money >= granadierCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Scout
                else if (MoneySystem.Instance.enemyAI.money >= scoutCost)
                {
                    //Falta añadir una forma de saber que es esa SpawnUnitAction y no otra.
                    SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);
                    return;
                }

                //Soldadito
                else
                {
                    SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);
                    return;
                }
               
            }

            else
            {
                Debug.Log("Tengo muy pocas unidades en juego, voy a plantar varias unidades baratujas");

                //Lógica para spawnear soldaditos
                //como le puedo indicar que de todas las SpawnUnitAction quiero una en concreto?
                //Debería cambiarlo y que fueran acciones independientes? meter un booleano 
                //Ejemplo: isScout y que busque ese booleano para saber si es la acción que quiero?

                SetValues(enemyBase.GetComponent<SpawnUnitAction>(), maxAIValueAction, minAIValueAction);

                return;
            }

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
}

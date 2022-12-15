using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCheckers : MonoBehaviour
{
    public static BaseCheckers Instance { get; private set; }
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

    }


    //@GRG --------------------------------------------------------------------
    // Función para saber si tengo dinero como para spawnear al menos una tropa
    //-------------------------------------------------------------------------
    public bool CanSpawnAnyUnit()
    {
        Debug.Log(MoneySystem.Instance.enemyAI.money);
        return MoneySystem.Instance.enemyAI.money >= 300;
    }

    //@GRG --------------------------------------------------------------------
    // Función para saber la cantidad de enemigos en juego
    //-------------------------------------------------------------------------
    public int AmountOfUnitsDeployed()
    {
        return UnitManager.Instance.GetEnemyUnitList().Count;
    }

}

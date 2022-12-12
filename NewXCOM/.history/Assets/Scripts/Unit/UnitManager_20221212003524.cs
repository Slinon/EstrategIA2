using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    public static UnitManager Instance { get; private set; }         // Instancia del singleton

    private List<Unit> unitList;                                    // Lista de unidades
    private List<Unit> friendlyUnitList;                            // Lista de unidades aliadas
    private List<Unit> enemyUnitList;                               // Lista de unidades enemigas

    public static event EventHandler OnAnyUnitMovedGridPosition;

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un UnitManager!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

        // Inicializamos las listas
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDied += Unit_OnAnyUnitDied;
        Unit.OnAnyUnitMoved += Unit_OnAnyUnitMoved;

    }

    // @IGM ----------------------------------------
    // Handler del evento cuando aparece una unidad.
    // ---------------------------------------------
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs empty)
    {

        // Recuperamos la unidad
        Unit unit = sender as Unit;

        // La a�adimos a la lista de unidades
        unitList.Add(unit);

        // Comprobamos si la unidad es un enemigo
        if (unit.IsEnemy())
        {

            // A�adimos la unidad a la lista de enemigos
            enemyUnitList.Add(unit);

        }
        else
        {

            // A�adimos la unidad a la lista de aliados
            friendlyUnitList.Add(unit);

        }

    }

    // @IGM --------------------------------------
    // Handler del evento cuando muere una unidad.
    // -------------------------------------------
    private void Unit_OnAnyUnitDied(object sender, EventArgs empty)
    {

        // Recuperamos la unidad
        Unit unit = sender as Unit;

        // La borramos de la lista de unidades
        unitList.Remove(unit);

        // Comprobamos si la unidad es un enemigo
        if (unit.IsEnemy())
        {

            // Borramos la unidad de la lista de enemigos
            enemyUnitList.Remove(unit);

        }
        else
        {

            // Borramos la unidad de la lista de aliados
            friendlyUnitList.Remove(unit);

            // Comprobamos si es la unidad seleccionada
            if (unit == UnitActionSystem.Instance.GetSelectedUnit())
            {

                // Comprobamos si aun quedan unidades restantes
                if (friendlyUnitList.Count > 0)
                {

                    UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[0]);

                }

            }

        }

    }

    // @IGM --------------------------
    // Getter de la lista de unidades.
    // -------------------------------
    public List<Unit> GetUnitList()
    {

        return unitList;

    }

    // @IGM -----------------------------------
    // Getter de la lista de unidades aliadaas.
    // ----------------------------------------
    public List<Unit> GetFriendlyUnitList()
    {

        return friendlyUnitList;

    }

    // @IGM -----------------------------------
    // Getter de la lista de unidades enemigas.
    // ----------------------------------------
    public List<Unit> GetEnemyUnitList()
    {

        return enemyUnitList;

    }

    // @VJT --------------------------------------
    // Handler del evento cuando se mueve cualquier unidad
    // -------------------------------------------
    private void Unit_OnAnyUnitMoved(object sender, EventArgs empty)
    {
        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyUnitMovedGridPosition != null)
        {

            // Lanzamos el evento
            OnAnyUnitMovedGridPosition(this, EventArgs.Empty);

        }
    }

    // @VJT --------------------------------------
    // Escconde los enemigos
    // -------------------------------------------
    public void hideOrShowEnemies(List<Vector2Int> revealedPositionGrid)
    {
        foreach (Unit enemy in enemyUnitList)
        {
            if(revealedPositionGrid.Contains(enemy.GetGridPosition()))
            {
                enemy.SetIsVisible(false);
            }
            enemy.SetIsVisible(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridSystem<GridObject> gridSystem;          // Malla del que proviene
    private GridPosition gridPosition;                  // Posicion en la malla
    private List<Unit> unitList;                        // Lista de unidades que contiene
    private IInteractable interactable;                 // Puerta que contiene la posicion

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {

        // Asignamos los atributos
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();

    }

    // @IGM --------------------------
    // Funcion sustituda del ToString.
    // -------------------------------
    public override string ToString()
    {

        string unitString = "";
        foreach (Unit unit in unitList)
        {

            unitString += unit + "\n";

        }
        return gridPosition.ToString() + "\n" + unitString;

    }

    // @IGM ------------------------------------
    // Metodo para añadir una unidad a la celda.
    // -----------------------------------------
    public void AddUnit(Unit unit)
    {

        // Añadimos la unidad
        unitList.Add(unit);

    }

    // @IGM --------------------------------------
    // Metodo para eliminar una unidad a la celda.
    // -------------------------------------------
    public void RemoveUnit(Unit unit)
    {

        // Eliminamos la unidad
        unitList.Remove(unit);

    }

    // @IGM --------------------------------------
    // Getter de la lista de unidades de la celda.
    // -------------------------------------------
    public List<Unit> GetUnitList()
    {

        return unitList;

    }

    // @IGM ---------------------------------------------
    // Funcion para comprobar si la celda tiene unidades.
    // --------------------------------------------------
    public bool HasAnyUnit()
    {

        return unitList.Count > 0;

    }

    // @IGM --------------------------------------------------
    // Getter de la primera unidad que ha entrado en la celda.
    // -------------------------------------------------------
    public Unit GetUnit()
    {

        // Comprobamos si hay alguna unidad en la celda
        if (HasAnyUnit())
        {

            return unitList[0];

        }
        else
        {

            // No hay ninguna unidad
            return null;

        }

    }

    // @IGM -----------------------------------------------
    // Getter del objeto interactuable que hay en la celda.
    // ----------------------------------------------------
    public IInteractable GetInteractable()
    {

        return interactable;

    }

    // @IGM -----------------------------------
    // Setter de la puerta que hay en la celda.
    // ----------------------------------------
    public void SetInteractable(IInteractable interactable)
    {

        this.interactable = interactable;

    }

}

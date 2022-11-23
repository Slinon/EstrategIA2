using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance { get; private set; }      // Instancia del singleton

    public event EventHandler OnAnyUnitMovedGridPosition;       // Evento cuano una unidad se cambia de posicion en la malla

    [SerializeField] private Transform gridDebugObjectPrefab;   // Prefab del nodo que se genera en la malla
    [SerializeField] private int width;                         // Alto de la malla
    [SerializeField] private int height;                        // Ancho de la malla
    [SerializeField] private float cellSize;                    // Tamaño de la celda

    private GridSystem<GridObject> gridSystem;                  // Malla que utilizamos

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un LevelGrid!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

        // Creamos la malla
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, 
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos el pathfinding
        Pathfinding.Instance.Setup(width, height, cellSize);

    }

    // @IGM ---------------------------------
    // Metodo para añadir una unidad al nodo.
    // --------------------------------------
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {

        // Añadimos la unidad
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);

    }

    // @IGM ------------------------------------------
    // Funcion para saber que unidades hay en el nodo.
    // -----------------------------------------------
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {

        // Recuperamos la unidad
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();

    }

    // @IGM ------------------------------------
    // Metodo para eliminar una unidad del nodo.
    // -----------------------------------------
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {

        // Eliminamos la unidad
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);

    }

    // @IGM ------------------------------------------
    // Metodo para mover una unidad de un nodo a otro.
    // -----------------------------------------------
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {

        // Eliminamos la unidad del nodo de donde viene
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        // Añadimos la unidad al nodo al que va
        AddUnitAtGridPosition(toGridPosition, unit);

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyUnitMovedGridPosition != null)
        {

            // Lanzamos el evento
            OnAnyUnitMovedGridPosition(this, EventArgs.Empty);

        }

    }

    // @IGM ---------------------------------------------------------------------
    // Funcion para saber la posicion en la malla desde una posicion en el mundo.
    // --------------------------------------------------------------------------
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {

        // Llamamos al metodo de la malla que lo calcula
        return gridSystem.GetGridPosition(worldPosition);

    }

    // @IGM -------------------------------------------------------------------
    // Funcion para saber la posicion del mundo desde una posicion de la malla.
    // ------------------------------------------------------------------------
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {

        // Llamamos al metodo de la malla que lo calcula
        return gridSystem.GetWorldPosition(gridPosition);

    }

    // @IGM -------------------------------------------------------
    // Funcion para comprobar si la posicion de la malla es valida.
    // ------------------------------------------------------------
    public bool IsValidGridPosition(GridPosition gridPosition)
    {

        // Llamamos al metodo de la malla que lo comprueba
        return gridSystem.IsValidGridPosition(gridPosition);

    }

    // @IGM ---------------
    // Getter de la altura.
    // --------------------
    public int GetWidth()
    {

        // Llamamos al metodo de la malla que lo calcula
        return gridSystem.GetWidth();

    }

    // @IGM ----------------
    // Getter de la anchura.
    // ---------------------
    public int GetHeight()
    {

        // Llamamos al metodo de la malla que lo calcula
        return gridSystem.GetHeight();

    }

    // @IGM ----------------------
    // Getter del tamaño de celda.
    // ---------------------------
    public float GetCellSize()
    {

        // Llamamos al metodo de la malla que lo calcula
        return gridSystem.GetCellSize();

    }

    // @IGM -------------------------------------------------------
    // Funcion que comprueba si una celda tiene una unidad ocupada.
    // ------------------------------------------------------------
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {

        // Comprobamos si la celda tiene unidades
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();

    }

    // @IGM ------------------------------------------------
    // Funcion para saber que unidad esta ocupando la celda.
    // -----------------------------------------------------
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {

        // Comprobamos si la celda tiene unidades
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();

    }

    // @IGM ----------------------------------------------
    // Getter del objeto interacuable que hay en la celda.
    // ---------------------------------------------------
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();

    }

    // @IGM ----------------------------------------------
    // Setter del objeto interacuable que hay en la celda.
    // ---------------------------------------------------
    public void SetInteractableAtGridPosition(IInteractable interactable, GridPosition gridPosition)
    {

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);

    }

}

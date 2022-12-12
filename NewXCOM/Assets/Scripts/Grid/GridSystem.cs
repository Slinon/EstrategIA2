using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{

    private int width;                                      // Ancho de la malla
    private int height;                                     // Alto de la malla
    private float cellSize;                                 // Tama�o de la celda
    private TGridObject[,] gridObjectArray;                  // Matriz de celdas

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public GridSystem(int width, int height, float cellSize, 
        Func<GridSystem<TGridObject>, GridPosition, TGridObject> CreateGridObject)
    {

        // Asignamos los atributos
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        // Creamos el array
        gridObjectArray = new TGridObject[width, height];

        // Recorremos la malla
        for (int x = 0; x < width; x++)
        {

            for (int z = 0; z < height; z++)
            {

                // Creamos la celda
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = CreateGridObject(this, gridPosition);

            }

        }

    }

    // @IGM ------------------------------------------------------------------
    // Funcion para averiguar la posicion global dada la posicion en la malla.
    // -----------------------------------------------------------------------
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {

        // Multiplicamos la posicion por el tama�o de celda
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;

    }

    // @IGM ---------------------------------------------------------------------
    // Funcion para saber la posicion en la malla desde una posicion en el mundo.
    // --------------------------------------------------------------------------
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {

        // Dividimos la posicion por el tama�o de celda
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize));

    }

    // @IGM -------------------------------------------------------
    // Metodo para crear los objetos visuales de Debug de la malla.
    // ------------------------------------------------------------
    public void CreateDebugObjects(Transform debugPrefab)
    {

        // Recorremos la malla
        for (int x = 0; x < width; x++)
        {

            for (int z = 0; z < height; z++)
            {

                // Creamos la celda
                GridPosition gridPosition = new GridPosition(x, z);

                // Instanciamos el objeto
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();

                // Le asignamos al objeto la posicion en la malla
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));

            }

        }

    }

    // @IGM ------------------------------
    // Getter de la posicion de una celda.
    // -----------------------------------
    public TGridObject GetGridObject(GridPosition gridPosition)
    {

        return gridObjectArray[gridPosition.x, gridPosition.z];

    }

    // @IGM -------------------------------------------------------
    // Funcion para comprobar si la posicion de la malla es valida.
    // ------------------------------------------------------------
    public bool IsValidGridPosition(GridPosition gridPosition)
    {

        // Comprobamos que la posicion est�a dentro de los limites de la malla
        return gridPosition.x >= 0 && gridPosition.z >= 0 && 
            gridPosition.x < width && gridPosition.z < height;

    }

    // @IGM ----------------
    // Getter de la anchura.
    // ---------------------
    public int GetHeight()
    {

        return height;

    }

    // @IGM ---------------
    // Getter de la altura.
    // --------------------
    public int GetWidth()
    {

        return width;

    }

    // @IGM ----------------------
    // Getter del tama�o de celda.
    // ---------------------------
    public float GetCellSize()
    {

        return cellSize;

    }

}

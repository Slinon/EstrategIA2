using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{

    private GridPosition gridPosition;                          // Posicion en la malla

    private int gCost;                                          // Coste del inicio al nodo
    private int hCost;                                          // Coste heuristico del nodo
    private int fCost;                                          // Coste de la suma de g y h
    private PathNode cameFromPathNode;                          // Nodo del que viene

    private bool isWalkable = true;                             // Booleano para saber si se puede caminar por el nodo
    private int heapIndex;                                      // Indice en el monton

    // @IGM -------------------
    // Constructor de la clase.
    // ------------------------
    public PathNode(GridPosition gridPosition)
    {

        // Asignamos los atributos
        this.gridPosition = gridPosition;

    }

    // @IGM -----------------------------
    // Get/Set de la interfaz del monton.
    // ----------------------------------
    public int HeapIndex
    {

        // Getter
        get
        {

            return heapIndex;

        }

        // Setter
        set
        {

            heapIndex = value;

        }

    }

    // @IGM --------------------------
    // Funcion sustituda del ToString.
    // -------------------------------
    public override string ToString()
    {
        return gridPosition.ToString();
    }

    // @IGM -----------------
    // Getter del coste de G.
    // ----------------------
    public int  GetGCost()
    {

        return gCost;

    }

    // @IGM -----------------
    // Getter del coste de H.
    // ----------------------
    public int GetHCost()
    {

        return hCost;

    }

    // @IGM -----------------
    // Getter del coste de F.
    // ----------------------
    public int GetFCost()
    {

        return fCost;

    }

    // @IGM --------------------------
    // Getter de la posicion del nodo.
    // -------------------------------
    public GridPosition GetGridPosition()
    {

        return gridPosition;

    }

    // @IGM -----------------
    // Getter del nodo padre.
    // ----------------------
    public PathNode GetCameFromPathNode()
    {

        return cameFromPathNode;

    }

    // @IGM -----------------
    // Setter del coste de G.
    // ----------------------
    public void SetGCost(int gCost)
    {

        this.gCost = gCost;

    }

    // @IGM -----------------
    // Setter del coste de H.
    // ----------------------
    public void SetHCost(int hCost)
    {

        this.hCost = hCost;

    }

    // @IGM --------------------
    // Setter del el nodo padre.
    // -------------------------
    public void SetCameFromPathNode(PathNode pathNode)
    {

        this.cameFromPathNode = pathNode;

    }

    // @IGM ------------------------
    // Setter de booleano caminable.
    // -----------------------------
    public void SetIsWalkable(bool isWalkable)
    {

        this.isWalkable = isWalkable;

    }

    // @IGM ------------------------------
    // Metodo para calcular el coste de F.
    // -----------------------------------
    public void CalculateFCost()
    {

        // Sumamos el coste de g y h
        fCost = gCost + hCost;

    }

    // @IGM ------------------------------
    // Metodo para resetear el nodo padre.
    // -----------------------------------
    public void ResetCameFromPathNode()
    {

        cameFromPathNode = null;

    }

    // @IGM --------------------------------------
    // Funcion para saber si el nodo es caminable.
    // -------------------------------------------
    public bool IsWalkable()
    {

        return isWalkable;

    }

    

    // @IGM ----------------------
    // Metodo para comparar nodos.
    // ---------------------------
    public int CompareTo(PathNode nodeToCompare)
    {

        // Comparamos el coste de f
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        // Comprobamos que es la misma comparacion
        if (compare == 0)
        {

            // Comparamos el coste de h
            compare = hCost.CompareTo(nodeToCompare.hCost);

        }

        // Devolvemos la comparacion negativa
        return -compare;

    }
}

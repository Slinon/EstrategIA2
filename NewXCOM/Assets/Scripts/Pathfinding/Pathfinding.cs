using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    private const int MOVE_STRAIGHT_COST = 10;                  // Constante coste moverse recto
    private const int MOVE_DIAGONAL_COST = 14;                  // Constante coste moverse en diagonal

    public static Pathfinding Instance { get; private set; }    // Instancia del singleton

    [SerializeField] private Transform gridDebugObjectPrefab;   // Prefab del objeto que usaremos de debug
    [SerializeField] private LayerMask obstaclesLayerMask;      // Prefab del objeto que usaremos de debug

    private int width;                                          // Alto de la malla
    private int height;                                         // Ancho de la malla
    private float cellSize;                                     // Tamaño de la celda
    GridSystem<PathNode> gridSystem;                            // Malla que usa el algoritmo

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un Pathfinding!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

    }

    // @IGM --------------------------------------------
    // Metodo para setear las variables del pathfinding.
    // -------------------------------------------------
    public void Setup(int width, int height, float cellSize)
    {

        // Asignamos las variables
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        // Creamos la malla
        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        // Recorremos la malla de nodos
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {

            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {

                // Recuperamos la posicion en el mundo del nodo alcual
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                // Lanzamos un raycast para saber si hay un obstaculo
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask))
                {

                    // Marcamos que el nodo es un nodo obstaculo
                    GetNode(x, z).SetIsWalkable(false);

                }


            }

        }

    }

    // @IGM ---------------------------------------------
    // Metodo que calcula el camino mas corto a un punto.
    // --------------------------------------------------
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {

        // Creamos las listas de nodos
        Heap<PathNode> openPathNodes = new Heap<PathNode>(width * height);
        HashSet<PathNode> closedPathNodes = new HashSet<PathNode>();

        // Establecemos el nodo inicial y el final
        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openPathNodes.Add(startNode);

        // Recorremos la malla de nodos
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {

            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {

                // Recuperamos el nodo actual
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                // Inicializamos los parametros del nodo
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();

            }

        }

        // Estabelcemos los parametros del nodo inicial
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        // Empezamos el bucle
        while (openPathNodes.Count > 0)
        {

            // Cojemos el primer nodo
            PathNode currentNode = openPathNodes.RemoveFirst();

            // Cerramos el nodo
            closedPathNodes.Add(currentNode);

            // Comprobamos si el nodo es el nodo final
            if (currentNode == endNode)
            {

                // Devolvemos el camino
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);

            }         

            // Recorremos la lista de nodos vecinos
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {

                // Comprobamos si el nodo esta cerrado
                if (closedPathNodes.Contains(neighbourNode))
                {
                    // Lo saltamos
                    continue;

                }

                // Comprobamos si el nodo es un obstaculo
                if (!neighbourNode.IsWalkable())
                {

                    // Lo añadimos a la lista de nodos cerrados
                    closedPathNodes.Add(neighbourNode);

                    // Lo saltamos
                    continue;

                }

                // Calculamos el coste de movernos al vecino
                int newMovementCostToNeighbour = currentNode.GetGCost() + 
                    CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                // Comprobamos que el coste es menor que el coste g
                if (newMovementCostToNeighbour < neighbourNode.GetGCost())
                {

                    // Actualizamos los costes
                    neighbourNode.SetGCost(newMovementCostToNeighbour);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    // Asignamos el nodo del que procede
                    neighbourNode.SetCameFromPathNode(currentNode);

                    // Comrpobamos si el nodo aun no esta abierto
                    if (!openPathNodes.Contains(neighbourNode))
                    {

                        // Abrimos el nodo vecino
                        openPathNodes.Add(neighbourNode);

                    }

                }

            }

        }

        // No hemos encontrado camino
        pathLength = 0;
        return null;

    }

    // @IGM --------------------------------------------------
    // Metodo para calcular la distancia entre dos posiciones.
    // -------------------------------------------------------
    public int CalculateDistance(GridPosition GridPositionA, GridPosition GridPositionB)
    {

        // Calculamos la distancia euclidea
        int xDistance = Mathf.Abs(GridPositionA.x - GridPositionB.x);
        int yDistance = Mathf.Abs(GridPositionA.z - GridPositionB.z);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;

    }

    // @IGM ----------------------------------------------
    // Metodo para encontrar el nodo con menor coste de F.
    // ---------------------------------------------------
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {

        // Asignamos el menor coste de F
        PathNode lowestFCostPathNode = pathNodeList[0];

        // Recorremos la lista de nodos
        for (int i = 0; i < pathNodeList.Count; i++)
        {

            // Comprobamos si el coste es menor que el anterior
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                // Lo actualizamos
                lowestFCostPathNode = pathNodeList[i];

            }

        }

        return lowestFCostPathNode;

    }

    // @IGM ----------------------------------------------------
    // Funcion que devuelve el nodo de una posicion en la malla.
    // ---------------------------------------------------------
    private PathNode GetNode(int x, int z)
    {

        return gridSystem.GetGridObject(new GridPosition(x, z));

    }

    // @IGM ----------------------------------------------------------
    // Funcion que devualve la lista de nodos vecinos del nodo actual.
    // ---------------------------------------------------------------
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {

        // Creamos la lista de vecinos
        List<PathNode> neighbourList = new List<PathNode>();

        // Recuperamos la posicion en la malla
        GridPosition gridPosition = currentNode.GetGridPosition();

        // Comprobamos si los vecinos no se salen de la malla
        if (gridPosition.x - 1 >= 0)
        {

            // Recuperamos el vecino de la izquierda
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if (gridPosition.z - 1 >= 0)
            {

                // Recuperamos el vecino de la izquierda abajo
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));

            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {

                // Recuperamos el vecino de la izquierda arriba
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));

            }

        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {

            // Recuperamos el vecino de la derecha
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z - 1 >= 0)
            {

                // Recuperamos el vecino de la derecha abajo
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));

            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {

                // Recuperamos el vecino de la derecha arriba
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));

            }

        }

        if (gridPosition.z - 1 >= 0)
        {

            // Recuperamos el vecino de abajo
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));

        }

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {

            // Recuperamos el vecino de arriba
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));

        }
        

        // Devolvemos la malla
        return neighbourList;

    }

    // @IGM -------------------------------------------------------
    // Funcion que devuelve el camino calculado por el pathfinding.
    // ------------------------------------------------------------
    private List<GridPosition> CalculatePath(PathNode endNode)
    {

        // Creamos la lista empezando por el final
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        // Comprobamos si hemos llegado al principio
        while (currentNode.GetCameFromPathNode() != null)
        {

            // Añadimos el nodo a la lista
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();

        }

        // Le damos la vuelta a la lista
        pathNodeList.Reverse();

        // Creamos la lista de posiciones
        List<GridPosition> gridPositionList = new List<GridPosition>();

        // Recorremos la lista de posiciones
        foreach (PathNode pathNode in pathNodeList)
        {

            // Añadimos la posicion del nodo a la lista
            gridPositionList.Add(pathNode.GetGridPosition());

        }

        // Devolvemos la lista
        return gridPositionList;

    }

    // @IGM -------------------------------------------------
    // Metodo para hacer una posicion una posicion caminable.
    // ------------------------------------------------------
    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {

        // Establecemmos si una posicion es caminable o no
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);

    }

    // @IGM -----------------------------------------------
    // Funcion para comprobar si una posicion es caminable.
    // ----------------------------------------------------
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {

        // Comprobamos si la habitacion es caminable
        return gridSystem.GetGridObject(gridPosition).IsWalkable();

    }

    // @IGM ------------------------------------------------
    // Funcion para saber si una posicion tiene camino o no.
    // -----------------------------------------------------
    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {

        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;

    }

    // @IGM ------------------------------------------------
    // Funcion para recuperar el tamaño del camino a seguir.
    // -----------------------------------------------------
    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {

        // Calculamos el coste de movimientos
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;

    }
        
}

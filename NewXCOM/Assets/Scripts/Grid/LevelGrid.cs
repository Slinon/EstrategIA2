using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }              // Instancia del singleton

    public event EventHandler OnAnyUnitMovedGridPosition;               // Evento cuano una unidad se cambia de posicion en la malla
    public event EventHandler<GridPosition> OnAnyHeatMapValueChanged;   // Evento cuando un valor del mapa de influencia cambia

    [SerializeField] private Transform gridDebugObjectPrefab;           // Prefab del nodo que se genera en la malla
    [SerializeField] private int width;                                 // Alto de la malla
    [SerializeField] private int height;                                // Ancho de la malla
    [SerializeField] private float cellSize;                            // Tama�o de la celda
    [SerializeField] LayerMask obstacleLayerMask;

    private CoverType coverType;
    private GridSystem<GridObject> gridSystem;                          // Malla que utilizamos
    private bool hasLeft;
    private bool hasRight;
    private bool hasFront;
    private bool hasBack;

    RandomizeMap randomizer;

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
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);


        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(new GridPosition(x,z));

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(GetGridPosition(worldPosition))) {
                    // Something at this position, cover?
                    if (TryGetComponent(out CoverObject coverObject)) {
                        gridSystem.GetGridObject(new GridPosition(x,z)).SetCoverType(coverObject.GetCoverType());
                    }
                }
            }
        }
        
    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {
        //Fetcheamos el randomizador
        randomizer = GetComponent<RandomizeMap>();
        //Instanciamos los obstaculos
        randomizer.FillWithObstacles();
        // Asignamos el pathfinding
        Pathfinding.Instance.Setup(width, height, cellSize);

    }

    // @IGM ---------------------------------
    // Metodo para a�adir una unidad al nodo.
    // --------------------------------------
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {

        // A�adimos la unidad
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

        // A�adimos la unidad al nodo al que va
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
    // Getter del tama�o de celda.
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

    // @IGM ------------------------------------------------------
    // Getter del valor del mapa de influencia que tiene la celda.
    // -----------------------------------------------------------
    public int GetHeatMapValueAtGridPosition(GridPosition gridPosition)
    {

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetHeatMapValue();

    }

    // @IGM ----------------------------------------------
    // Setter del objeto interacuable que hay en la celda.
    // ---------------------------------------------------
    public void SetHeatMatValueAtGridPosition(int value, GridPosition gridPosition)
    {

        // Comrpobamos que es una posicion valida
        if (!gridSystem.IsValidGridPosition(gridPosition))
        {

            return;

        }

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetHeatMapValue(value);

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyHeatMapValueChanged != null)
        {

            // Lanzamos el evento
            OnAnyHeatMapValueChanged(this, gridPosition);

        }

    }

    // @IGM ---------------------------------------------------------------------
    // Metodo para aumentar el valor del punto caliente en el mapa de influencia.
    // --------------------------------------------------------------------------
    public void AddValue(GridPosition gridPosition, int value)
    {

        // Comrpobamos que es una posicion valida
        if (!gridSystem.IsValidGridPosition(gridPosition))
        {

            return;

        }

        SetHeatMatValueAtGridPosition(GetHeatMapValueAtGridPosition(gridPosition) + value, gridPosition);

    }

    // @IGM ---------------------------------------------------------
    // Metodo para añadir un punto caliente en el mapa de influencia.
    // --------------------------------------------------------------
    public void AddValue(GridPosition gridPosition, int value, int fullValueRange, int totalRange)
    {

        // Calculamos la cantidad de dispersion en las casillas
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

        // Recorremos la malla dentro del rango dado
        for (int x = 0; x < totalRange; x++)
        {

            for (int z = 0; z < totalRange - x; z++)
            {

                // Calculamos el radio
                int radius = x + z;
                int addValueAmount = value;

                // Comprobamos si el radio ha superado al rango de valos maximo
                if (radius > fullValueRange)
                {

                    // Restamos el valor
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange); 

                }

                // Añadimos el valor al mapa de influencia
                GridPosition testGridPosition = new GridPosition(gridPosition.x + x, gridPosition.z + z);    
                AddValue(testGridPosition, addValueAmount);

                // Comprobamos que no sea la primera fila
                if (x != 0)
                {

                    // Añadimos el valor al mapa de influencia
                    testGridPosition = new GridPosition(gridPosition.x - x, gridPosition.z + z);
                    AddValue(testGridPosition, addValueAmount);

                }

                // Comprobamos que no sea la primera fila
                if (z != 0)
                {

                    // Añadimos el valor al mapa de influencia
                    testGridPosition = new GridPosition(gridPosition.x + x, gridPosition.z - z);
                    AddValue(testGridPosition, addValueAmount);

                    // Comprobamos que no sea la primera fila
                    if (x != 0)
                    {

                        // Añadimos el valor al mapa de influencia
                        testGridPosition = new GridPosition(gridPosition.x - x, gridPosition.z - z);
                        AddValue(testGridPosition, addValueAmount);

                    }

                }

            }

        }

    }
    
    // @IGM -----------------------------------------
    // Getter del valor maximo del mapade influencia.
    // ----------------------------------------------
    public int GetMaxHeatMapValue() 
    {

        int heatMapMaxValue = 0;

        // Recorremos la malla
        for (int x = 0; x < width; x++)
        {

            for (int z = 0; z < height - x; z++)
            {

                // Creamos la posicion de la malla
                GridPosition gridPosition = new GridPosition(x, z);

                // Comprobamos que el valor del mapa de influencia que tenemos es menor que el de la posicion actual
                if (heatMapMaxValue < GetHeatMapValueAtGridPosition(gridPosition))
                {

                    heatMapMaxValue = GetHeatMapValueAtGridPosition(gridPosition);

                }

            }

        }

        return heatMapMaxValue;
    
    }

    // @IGM -------------------------------------
    // Metodo para limpiar el mapa de influencia.
    // ------------------------------------------
    public void ClearHeatMap()
    {

        // Recorremos la malla
        for (int x = 0; x < width; x++)
        {

            for (int z = 0; z < height; z++)
            {

                // Establecemos el valor del mapa de influencia a 0 en esa casilla
                GridPosition gridPosition = new GridPosition(x, z);
                SetHeatMatValueAtGridPosition(0, gridPosition);

            }

        }

    }

    public void GetWidthHeight(out int width, out int height) 
    {
        width = gridSystem.GetWidth();
        height = gridSystem.GetHeight();
    }

    public void SetCoverTypeAtGridPosition(CoverType coverType, GridPosition gridPosition)
    {
        
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetCoverType(coverType);
    }

    public void SetCoverType(CoverType coverType) 
    {
        this.coverType = coverType;
    }

    public CoverType GetCoverType() 
    {
        return coverType;
    }

    public CoverType GetCoverTypeAtPosition(Vector3 worldPosition) 
    {
        return gridSystem.GetGridObject(LevelGrid.Instance.GetGridPosition(worldPosition)).GetCoverType();
        
    }

    public CoverType GetUnitCoverType(Vector3 worldPosition) 
    {
        
        gridSystem.GetGridPosition(worldPosition);
        


        //Comprueba que no se salga de la malla por la izquierda
        if((Mathf.RoundToInt(worldPosition.x)/2) - 1 > -1)
        {
            //Comprueba que el grid de la izquierda esta libre
            if(Pathfinding.Instance.IsWalkableGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) - 1, (Mathf.RoundToInt(worldPosition.z)/2) + 0)))
            {
                //Esta libre entonces tiene izquierda
                hasLeft = true;
            //Comprueba que si esta ocupado no sea una unidad
            }else if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) - 1, (Mathf.RoundToInt(worldPosition.z)/2) + 0)))
                {
                    //Si no es una unidad es una cobertura
                    hasLeft = false;
                }else{hasLeft = true;}
        }else{hasLeft = false;}
        

        //Debug.Log("hasLeft es " + hasLeft + " y su posicion es " + new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) - 1, (Mathf.RoundToInt(worldPosition.z)/2) + 0) + " de la unidad de " + gridSystem.GetGridPosition(worldPosition));




        //Comprueba que no se salga de la malla por la derecha
        if((Mathf.RoundToInt(worldPosition.x)/2) + 1 < width)
        {
            //Comprueba que el grid de la derecha esta ocupado
            if(Pathfinding.Instance.IsWalkableGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 1, (Mathf.RoundToInt(worldPosition.z)/2) + 0)))
            {
                hasRight = true;

            //Comprueba que si esta ocupado no sea una unidad
            }else if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 1, (Mathf.RoundToInt(worldPosition.z)/2) + 0)))
                {
                    //Si no es una unidad es una cobertura
                    hasRight = false;
                }else{hasRight = false;}
        }else{hasRight = true;}
        

        //Debug.Log("hasRight es " + hasRight + " y su posicion es " + new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 1 , (Mathf.RoundToInt(worldPosition.z)/2) + 0) + " de la unidad de " + gridSystem.GetGridPosition(worldPosition));




        //Comprueba que no se salga de la malla por enfrente(Arriba)
        if((Mathf.RoundToInt(worldPosition.z)/2) + 1 < height)
        {
            //Comprueba que el grid de la delante esta ocupado
            if(Pathfinding.Instance.IsWalkableGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) + 1)))
            {
                //Comprueba que lo que ocupa el grid no sea una unidad, si es unidad es false, sino true
                hasFront=true;

            //Comprueba que si esta ocupado no sea una unidad
            }else if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) + 1)))
                {
                    //Si no es una unidad es una cobertura
                    hasFront = false;                 
                }else{hasFront = false;}
        }else{hasFront = true;}


        //Debug.Log("hasFront es " + hasFront + " y su posicion es " + new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) + 1) + " de la unidad de " + gridSystem.GetGridPosition(worldPosition));



        //Comprueba que no se salga de la malla por atras(Abajo)
        if((Mathf.RoundToInt(worldPosition.z)/2) - 1 > -1)
        {
            //Comprueba que el grid de la delante esta ocupado
            if(Pathfinding.Instance.IsWalkableGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) - 1)))
            {
                //Comprueba que lo que ocupa el grid no sea una unidad, si es unidad es false, sino true
                hasBack = true;

            //Comprueba que si esta ocupado no sea una unidad    
            }else if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) - 1)))
                {
                    //Si no es una unidad es una cobertura
                    if((Mathf.RoundToInt(worldPosition.z)/2) - 1 <= -1)
                    {
                        hasBack=true;
                    }else{
                
                        hasBack = false;}
                }else{hasBack = true;}
        }else{hasBack = false;}


        //Debug.Log("hasBack es " + hasBack + " y su posicion es " + new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) - 1) + " de la unidad de " + gridSystem.GetGridPosition(worldPosition));



        CoverType leftCover, rightCover, frontCover, backCover;
        leftCover = rightCover = frontCover = backCover = CoverType.Covered;

        if(hasLeft) leftCover = gridSystem.GetGridObject(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) - 1 , (Mathf.RoundToInt(worldPosition.z)/2) + 0)).GetCoverType();
        if(hasRight) rightCover = gridSystem.GetGridObject(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 1, (Mathf.RoundToInt(worldPosition.z)/2) + 0)).GetCoverType();
        if(hasFront) frontCover = gridSystem.GetGridObject(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) + 1)).GetCoverType();
        if(hasBack) backCover = gridSystem.GetGridObject(new GridPosition((Mathf.RoundToInt(worldPosition.x)/2) + 0, (Mathf.RoundToInt(worldPosition.z)/2) - 1)).GetCoverType();


        if (leftCover == CoverType.Covered ||
            rightCover == CoverType.Covered ||
            frontCover == CoverType.Covered ||
            backCover == CoverType.Covered) 
        {
            return CoverType.Covered;
        }

        return CoverType.None;
    }



}

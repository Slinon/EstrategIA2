using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    // @IGM ---------------------------------------------
    // Enumerador con los colores soportados de la malla.
    // --------------------------------------------------
    public enum GridVisualType
    {

        White,
        Blue,
        Red,
        RedSoft,
        Yellow, 
        YellowSoft,
        Purple,
        Green,
        GreenSoft

    }

    // @IGM ---------------------------------------
    // Estructura del tipo de material de la malla.
    // ............................................
    [Serializable]
    public struct GridVisualTypeMaterial
    {

        public GridVisualType gridVisualType;                                               // Tipo de color
        public Material material;                                                           // Material

    }

    public static GridSystemVisual Instance { get; private set; }                           // Instancia del singleton

    [SerializeField] private Transform gridSystemVisualSinglePrefab;                        // Prefab del visual de una celda
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;       // Lista de materiales disponibles para las acciones

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;                          // Array de elementos visuales de la malla

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un GridSystemVisual!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        Unit.OnAnyUnitDied += Unit_OnAnyUnitDied;
        InteractAction.OnAnyActionCompleted += InteractAction_OnAnyActionCompleted;

        // Instanciamos el array
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()];

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Instanciamos el prefab
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, 
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                // Asignamos el elemento al array
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform
                    .GetComponent<GridSystemVisualSingle>();

            }

        }

        // Actualizamos el visual de la malla
        UpdateGridVisual();

    }

    // @IGM --------------------------------
    // Metodo para ocultar todas las celdas.
    // -------------------------------------
    public void HideAllGridPosition()
    {

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Ocultamos el elemento
                gridSystemVisualSingleArray[x, z].Hide();

            }

        }

    }

    // @IGM -----------------------------------
    // Metodo para mostrar las celdas en rango.
    // ----------------------------------------
    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {

        // Creamos la lista de posiciones
        List<GridPosition> gridPositionList = new List<GridPosition>();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -range; x <= range; x++)
        {

            for (int z = -range; z <= range; z++)
            {

                // Calculamos la posicion a testear
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                // Comprobamos si la posicion esta fuera de la malla
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // Calculamos la distancia de la posicion a probar
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                // Comprobamos si la distancia es mayor a la de diparo
                if (testDistance > range)
                {

                    // La saltamos
                    continue;

                }

                // A�adimos la posicion a la lista
                gridPositionList.Add(testGridPosition);

            }

        }

        // Mostramos las celdas disponibles
        ShowGridPositionList(gridPositionList, gridVisualType);

    }

    // @IGM --------------------------------------------
    // Metodo para mostrar las celdas en rango cuadrado.
    // -------------------------------------------------
    public void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {

        // Creamos la lista de posiciones
        List<GridPosition> gridPositionList = new List<GridPosition>();

        // Recorremos todas las posiciones validas alrededor de la malla
        for (int x = -range; x <= range; x++)
        {

            for (int z = -range; z <= range; z++)
            {

                // Calculamos la posicion a testear
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                // Comprobamos si la posicion esta fuera de la malla
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {

                    // La saltamos
                    continue;

                }

                // A�adimos la posicion a la lista
                gridPositionList.Add(testGridPosition);

            }

        }

        // Mostramos las celdas disponibles
        ShowGridPositionList(gridPositionList, gridVisualType);

    }

    // @IGM --------------------------------------
    // Metodo para mostrar las celdas disponibles.
    // -------------------------------------------
    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {

        // Recorremos la lista de posiciones
        foreach (GridPosition gridPosition in gridPositionList)
        {

            // Mostramos las celdas de las posiciones
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z]
                .Show(GetGridVisualTypeMaterial(gridVisualType));

            Debug.Log("material assigned: ")

        }

    }

    // @IGM -----------------------------------------------
    // Metodo para actualizar la visualizacion de la malla.
    // ----------------------------------------------------
    private void UpdateGridVisual()
    {

        // Ocultamos la malla
        HideAllGridPosition();

        // Recuperamos la accion y la unidad seleccionada
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        // Comprobamos cual es la unidad seleccionada
        GridVisualType gridVisualType;
        switch (selectedAction)
        {

            default:
            
            // Accion de moverse
            case MoveAction moveAction:

                // Color blanco
                gridVisualType = GridVisualType.White;

                break;

            // Accion de rodar
            case SpinAction spinAction:

                // Color azul
                gridVisualType = GridVisualType.Blue;

                break;

            // Accion de disparo
            case ShootAction shootAction:

                // Color rojo
                gridVisualType = GridVisualType.Red;

                // Mostramos el rango de disparo
                ShowGridPositionRange(selectedUnit.GetGridPosition(), 
                    shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);

                break;

            // Accion de lanzar granada
            case GrenadeAction grenadeAction:

                // Color morado suave
                gridVisualType = GridVisualType.Yellow;

                break;

            // Accion de atacar con espada
            case SwordAction swordAction:

                // Color amarillo
                gridVisualType = GridVisualType.Red;

                // Mostramos el rango del espadazo
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(),
                    swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);

                break;

            // Accion de interactuar
            case InteractAction interactAction:

                // Color amarillo
                gridVisualType = GridVisualType.Purple;

                break;

        }

        // Mostramos a donde se puede mover
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);

    }

    // @IGM ------------------------------------------------------
    // Handler del evento cuando cambiamos de accion seleccionada.
    // -----------------------------------------------------------
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs empty)
    {

        // Actualizamos la vista de la malla
        UpdateGridVisual();

    }

    // @IGM ---------------------------------------------------------------------
    // Handler del evento cuando cambiamos de posicion en la malla de una unidad.
    // --------------------------------------------------------------------------
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs empty)
    {

        // Actualizamos la vista de la malla
        UpdateGridVisual();

    }

    // @IGM --------------------------------------
    // Handler del evento cuando una unidad muere.
    // -------------------------------------------
    private void Unit_OnAnyUnitDied(object sender, EventArgs empty)
    {

        // Actualizamos la vista de la malla
        UpdateGridVisual();

    }

    // @IGM -------------------------------------------------
    // Handler del evento cuando una interacion ha terminado.
    // ------------------------------------------------------
    private void InteractAction_OnAnyActionCompleted(object sender, EventArgs empty)
    {

        // Actualizamos la vista de la malla
        UpdateGridVisual();

    }

    // @IGM -----------------------------------------------
    // Funcion para conseguir el material en base al color.
    // ----------------------------------------------------
    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {

        // Recorremos la lista de tipos de materiales
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {

            // Comrpobamos si concuerdan los grid visuals
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {

                // Devolvemos el material
                return gridVisualTypeMaterial.material;

            }

        }

        // Se ha producido un error
        Debug.LogError("No se ha encontrado GridVisualTypeMaterial para el GridVisualType " + gridVisualType);
        return null;

    }

}

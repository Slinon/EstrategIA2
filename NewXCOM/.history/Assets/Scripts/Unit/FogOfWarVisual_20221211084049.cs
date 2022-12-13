using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisual : MonoBehaviour
{
    public static FogOfWarVisual Instance {get; private set;}
    [SerializeField] private Transform hiddenGridSingle;        // Prefab con el material de "oculto"
    [SerializeField] private Transform exposedGridSingle;

    private FogOfWarVisualSingle[,] fogOfWarSingleArray;     
    

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    private void Start() {
        // Instanciamos el array
        fogOfWarSingleArray = new FogOfWarVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()];

        // Recorremos la malla
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {

            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {

                // Instanciamos el prefab de oculto
                GridPosition gridPosition = new GridPosition(x, z);
                Transform fogOfWarVisualSingleTransform = Instantiate(hiddenGridSingle, 
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                // Asignamos el elemento al array
                fogOfWarSingleArray[x, z] = fogOfWarVisualSingleTransform
                    .GetComponent<FogOfWarVisualSingle>();

            }

        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, Material material)
    {

        // Recorremos la lista de posiciones
        foreach (GridPosition gridPosition in gridPositionList)
        {

            // Mostramos las celdas de las posiciones
            fogOfWarSingleArray[gridPosition.x, gridPosition.z]
                .Show(material);

        }

    }

    public void ShowEverythinglol(List<Vector2Int> revealedGridPositions)
    {
        List<GridPosition> list = revealedGridPositions.ConvertAll(x => (GridPosition) x);
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                // Instanciamos el prefab para cada posicion
                GridPosition gridPosition = new GridPosition(x, z);
                if(list.Contains(gridPosition))
                {
                    //Debug.Log("position revealed");
                    continue;
                }

                Instantiate(hiddenGridSingle, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
            }
        }
    }

    public void HideAllGridPositions()
    {
        // Para cada celda del grid level:
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                // Instanciamos el prefab para cada posicion
                GridPosition gridPosition = new GridPosition(x, z);
                Instantiate(hiddenGridSingle, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
            }
        }
            // Esconder enemigos e interactuables
        
        
    }

    public void ShowGridPosition(List<Vector2Int> revealedGridPosition)
    {
        List<GridPosition> list = revealedGridPosition.ConvertAll(x => (GridPosition) x);
        //List<GridPosition> list = new List<GridPosition>();
        //foreach (Vector2Int pos in revealedGridPosition)
        //{
        //    list.Add(pos);
        //}
        
        //GridSystemVisual.Instance.ShowGridPositionList(list, GridSystemVisual.GridVisualType.White);

        // Recorremos la lista de posiciones
        foreach (GridPosition gridPosition in list)
        {
            //Debug.Log("position test: "  + gridSystemVisualSingleArray[gridPosition.x, gridPosition.z]);
            // Mostramos las celdas de las posiciones
            Instantiate(exposedGridSingle, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

        }
    }
}

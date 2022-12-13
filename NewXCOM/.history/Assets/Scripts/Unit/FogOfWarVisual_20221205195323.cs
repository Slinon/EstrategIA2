using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisual : MonoBehaviour
{
    public static FogOfWarVisual Instance {get; private set;}
    [SerializeField] private Transform hiddenGridSingle;        // Prefab con el material de "oculto"
    

    void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        HideAllGridPositions();
    }

    public void ShowEverythinglol(List<GridPosition> revealedGridPositions)
    {
        foreach (GridPosition gridPosition in revealedGridPositions)
        {
            
        }
    }

    public void HideAllGridPositions()
    {
        Debug.Log("Hiding all grid cells");
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
        List<GridPosition> list = revealedGridPosition.ConvertAll<GridPosition>(x => Grid);
        List<GridPosition> list = new List<GridPosition>();
        foreach (Vector2Int pos in revealedGridPosition)
        {
            list.Add(pos);
        }

        
        GridSystemVisual.Instance.ShowGridPositionList(list, GridSystemVisual.GridVisualType.White);
    }
}

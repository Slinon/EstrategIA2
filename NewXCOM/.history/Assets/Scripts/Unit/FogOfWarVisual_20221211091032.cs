using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisual : MonoBehaviour
{
    public static FogOfWarVisual Instance {get; private set;}
    [SerializeField] private Transform FogOfWarGridSingle;        // Prefab con el material de "oculto"
    [SerializeField] private Material exposedMaterial;
    [SerializeField] private Material hiddenMaterial;

    private FogOfWarVisualSingle[,] fogOfWarSingleArray;     
    

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;

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
                Transform fogOfWarVisualSingleTransform = Instantiate(FogOfWarGridSingle, 
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                // Asignamos el elemento al array
                fogOfWarSingleArray[x, z] = fogOfWarVisualSingleTransform
                    .GetComponent<FogOfWarVisualSingle>();

            }

        }
    }

    public void ShowForOfWar(List<Vector2Int> revealedGridPositions)
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
                    fogOfWarSingleArray[x, z].Show(exposedMaterial);
                    continue;
                }

                fogOfWarSingleArray[x, z].Show(hiddenMaterial);
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
                Instantiate(FogOfWarGridSingle, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
            }
        }
            // Esconder enemigos e interactuables
        
        
    }
}

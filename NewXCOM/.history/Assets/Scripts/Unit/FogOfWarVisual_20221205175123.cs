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
    }

    void Update()
    {
        
    }

    public void HideAllGridPositions()
    {
        Debug.Log("Hiding all grid cells");
        // Para cada celda del grid level:
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                Instance(hiddenGridSingle, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity)
            }
        }
            // Cambiar la textura
            // Esconder enemigos e interactuables
            
        
    }

    public void ShowGridPosition(List<Vector2Int> revealedGridPosition)
    {
        Debug.Log("showing cells:");
        foreach(Vector2Int gridPosition in revealedGridPosition)
        {
            Debug.Log(gridPosition);
        }
        //para cada celda:
            //cambiar la textura
            //mostrar enemigos e interactuables 
    }
}

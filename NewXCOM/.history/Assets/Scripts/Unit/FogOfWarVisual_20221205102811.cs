using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisual : MonoBehaviour
{
    public static FogOfWarVisual Instance {get; private set;}
    

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
        // Hide every cell
        Debug.Log("HidiGg all grid cells");
        // Recorrer e
        
    }

    public void ShowGridPosition(List<Vector2Int> revealedGridPosition)
    {

    }
}

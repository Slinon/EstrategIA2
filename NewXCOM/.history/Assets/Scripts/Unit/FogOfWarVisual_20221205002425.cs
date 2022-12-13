using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisual : MonoBehaviour
{
    public static FogOfWarVisual Instance {get; private set;}
    public 
    

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
        
    }

    public void ShowGridPosition(List<Vector2Int> revealedGridPosition)
    {

    }
}

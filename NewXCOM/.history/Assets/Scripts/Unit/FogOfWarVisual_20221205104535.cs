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
        Debug.Log("Hiding all grid cells");
        // Para cada celda del grid level
        // Cambiar la textura
        // Esconder enemigos e interactuables
        
    }

    public void ShowGridPosition(List<Vector2Int> revealedGridPosition)
    {
        //para cada celda:
            //cambiar la textura
            //mostrar enemigos e interactuables 
    }
}

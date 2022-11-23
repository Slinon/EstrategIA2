using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{

    [SerializeField] private TextMeshPro gCostText;                         // Texto del coste de g
    [SerializeField] private TextMeshPro hCostText;                         // Texto del coste de h
    [SerializeField] private TextMeshPro fCostText;                         // Texto del coste de f
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;       // Texto del coste de f

    private PathNode pathNode;                                  // Nodo al que esta asignado el objeto

    // @IGM -------------------------
    // Setter del objeto de la malla. 
    // ------------------------------
    public override void SetGridObject(object gridObject)
    {

        // Asignamos el objeto asignado
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    protected override void Update()
    {

        // Establecemos los textos
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();

        // Comprobamos el estado del nodo
        if (pathNode.IsWalkable())
        {

            isWalkableSpriteRenderer.color = Color.green;

        }
        else
        {

            isWalkableSpriteRenderer.color = Color.red;

        }

    }

}

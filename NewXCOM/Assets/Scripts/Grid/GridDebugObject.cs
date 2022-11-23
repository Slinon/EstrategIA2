using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro textMeshPro;           // Objeto de texto

    private object gridObject;                                  // Objecto de la malla

    // @IGM -------------------------
    // Setter del objeto de la malla. 
    // ------------------------------
    public virtual void SetGridObject(object gridObject)
    {

        this.gridObject = gridObject;

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    protected virtual void Update()
    {

        // Actualizamos con el texto del objeto
        textMeshPro.text = gridObject.ToString();

    }

}

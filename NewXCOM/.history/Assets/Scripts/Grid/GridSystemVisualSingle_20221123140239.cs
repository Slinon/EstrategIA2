using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{

    [SerializeField] private MeshRenderer meshRenderer;         // Malla de renderizado
    [SerializeField] private LayerMask elevations;
    public float yValue; //DEBUG

    // @GRG ---------------------------
    // Cambiar la posición en Y
    // --------------------------------
    private void Start()
    {
        Physics.Raycast(transform.position, 10 * Vector3.up, out RaycastHit hit, 5f, elevations);
        yValue = hit.point.y;

        if (yValue != 0)
        {
            ChangeYValue();
        }

        
    }

    // @IGM ---------------------------
    // Metodo para mostrar el elemento.
    // --------------------------------
    public void Show(Material material)
    {

        meshRenderer.enabled = true;
        meshRenderer.material = material;

    }

    // @IGM ---------------------------
    // Metodo para ocultar el elemento.
    // --------------------------------
    public void Hide()
    {

        meshRenderer.enabled = false;

    }

    void ChangeYValue()
    {
        Debug.Log("LA CASILLA EN " + transform.position + " DEBE SUBIR " + yValue);
        transform.position += new Vector3(0, yValue, 0); ;
    }

}

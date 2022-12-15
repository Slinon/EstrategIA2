using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    private static MouseWorld Instance;                         // Instancia de la clase

    [SerializeField] private LayerMask mousePlaneLayer;         // Layer de la superficie del mapa

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()                                        
    {

        // Asignamos la instancia
        Instance = this;

    }

    // @IGM ----------------------------------------------------------------
    // Metodo para conseguir la posicion del mundo a la que apunta el raton.
    // ---------------------------------------------------------------------
    public static Vector3 GetPosition()
    {

        // Lanzamos el raycast
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.mousePlaneLayer);

        // Devolvemos el punto
        return raycastHit.point;

    }

}

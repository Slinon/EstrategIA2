using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    [SerializeField] private bool invert;                       // Booleano para indicar si la camara esta invertida

    private Transform cameraTransform;                          // Posicion de la camara

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos la posicion de la camara
        cameraTransform = Camera.main.transform;

    }

    private void LateUpdate()
    {

        // Comprobamos si la camara esta invertida
        if (invert)
        {

            // Buscamos el vector director de la camara y el objecto
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;

            // Rotamos el objeto a la direccion invertida de la camara
            transform.LookAt(transform.position + dirToCamera * -1);

        }
        else
        {

            // Rotamos el objeto hacia la camara
            transform.LookAt(cameraTransform);

        }

    }

}

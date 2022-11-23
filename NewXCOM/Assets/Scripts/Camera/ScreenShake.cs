using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    public static ScreenShake Instance { get; private set; }          // Instancia del singleton

    private CinemachineImpulseSource cinemachineImpulseSource;      // Impulso del cinemachine

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un ScreenShake!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

        // Establecemos impulso del cinemachine
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

    }

    // @IGM -------------------------------
    // Metodo para hacer temblar la camara.
    // ------------------------------------
    public void Shake(float intensity = 1f)
    {

        // Generamos un impulso en la camara
        cinemachineImpulseSource.GenerateImpulse(intensity);

    }

}

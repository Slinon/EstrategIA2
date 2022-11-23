#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }      // Instancia del singleton

    private PlayerInputActions playerInputActions;                 // Acciones del input

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un InputManager!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

        // Asignamos el input de acciones
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

    }

    // @IGM ------------------------------------------
    // Getter de la posicion del raton en la pantalla.
    // -----------------------------------------------
    public Vector2 GetMouseScreenPosition()
    {

#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif

    }

    // @IGM ---------------------------------------------
    // Funcion para saber si se ha hecho click izquierdo.
    // --------------------------------------------------
    public bool IsLeftMouseButtonDownThisFrame()
    {

#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif

    }

    // @IGM ----------------------------------------
    // Getter del vector de movimiento de la camara.
    // ---------------------------------------------
    public Vector2 GetCameraMoveVector()
    {

#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        // Creamos el vector de movimiento
        Vector2 inputMoveDir = new Vector2(0, 0);

        // Comrpobamos si hemos pulsado la W
        if (Input.GetKey(KeyCode.W))
        {

            // Modificamos el vector
            inputMoveDir.y = +1f;

        }

        // Comrpobamos si hemos pulsado la S
        if (Input.GetKey(KeyCode.S))
        {

            // Modificamos el vector
            inputMoveDir.y = -1f;

        }

        // Comrpobamos si hemos pulsado la A
        if (Input.GetKey(KeyCode.A))
        {

            // Modificamos el vector
            inputMoveDir.x = -1f;

        }

        // Comrpobamos si hemos pulsado la D
        if (Input.GetKey(KeyCode.D))
        {

            // Modificamos el vector
            inputMoveDir.x = +1f;

        }

        return inputMoveDir;

#endif

    }

    // @IGM -------------------------------------------------------------
    // Getter de la cantidad de rotacion que hay que aplicar a la camara.
    // ------------------------------------------------------------------
    public float GetCameraRotateAmount()
    {

#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        // Comrpobamos si hemos pulsado la Q
        if (Input.GetKey(KeyCode.Q))
        {

            // Modificamos la cantidad de rotacion
            rotateAmount = +1f;

        }

        // Comrpobamos si hemos pulsado la E
        if (Input.GetKey(KeyCode.E))
        {

            // Modificamos la cantidad de rotacion
            rotateAmount = -1f;

        }

        return rotateAmount;
#endif

    }

    // @IGM -------------------------------------------------------------
    // Getter de la cantidad de zoom que hay que aplicar a la camara.
    // ------------------------------------------------------------------
    public float GetCameraZoomAmount()
    {

#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0f;

        // Comprobamos si hemos hecho zoom in
        if (Input.mouseScrollDelta.y > 0)
        {

            // Disminuimos la cantidad de zoom
            zoomAmount = -1f;

        }

        // Comprobamos si hemos hecho zoom out
        if (Input.mouseScrollDelta.y < 0)
        {

            // Aumentamos la cantidad de zoom
            zoomAmount = +1f;

        }

        return zoomAmount;
#endif

    }

}

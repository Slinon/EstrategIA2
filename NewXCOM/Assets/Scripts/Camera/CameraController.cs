using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    private const float MIN_FOLLOW_Y_OFFSET = 2f;                                   // Constante zoom minimo
    private const float MAX_FOLLOW_Y_OFFSET = 20f;                                  // Constante zoom maximo

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;     // Objeto cinemachine

    private Vector3 targetFollowOffset;                                             // Offset de seguimiento
    private CinemachineTransposer cinemachineTransposer;                            // Transposer del cinemachine

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los parametros
        cinemachineTransposer = cinemachineVirtualCamera
            .GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {

        // Comprobamos el movimiento
        HandleMovement();

        // Comprobamos la rotacion
        HandleRotation();

        // Comprobamos el zoom
        HandleZoom();

    }

    // @IGM --------------------------------------------
    // Metodo para controlar el movimiento de la camara.
    // -------------------------------------------------
    private void HandleMovement()
    {

        // Creamos el vector de movimiento
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();


        float moveSpeed = 10f;

        // Movemos la camara
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

    }

    // @IGM ------------------------------------------
    // Metodo para controlar la rotacion de la camara.
    // -----------------------------------------------
    private void HandleRotation()
    {

        // Creamos el vector de rotacion
        Vector3 rotationVector = new Vector3(0, 0, 0);

        // Sumamos la rotacion aplicada
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 100f;

        // Rotamos la camara
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

    }

    // @IGM --------------------------------------
    // Metodo para controlar el zoom de la camara.
    // -------------------------------------------
    private void HandleZoom()
    {

        float zoomIncreaseAmount = 1f;

        // Sumamos el zoom aplicado
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        float zoomSpeed = 5f;

        // Actualizamos el zoom sin superar los limites
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET,
            MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset,
            targetFollowOffset, Time.deltaTime * zoomSpeed);

    }

}

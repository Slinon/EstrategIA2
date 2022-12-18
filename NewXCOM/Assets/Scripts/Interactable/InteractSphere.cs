using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{

    // @IGM -----------------------------------
    // Maquina de estado para saber el control.
    // ----------------------------------------
    public enum InControlState
    {

        Player,
        Enemy,
        Neutral

    }

    [Header("Materiales:")]
    [SerializeField] private Material playerMaterial;       // Material del jugador
    [SerializeField] private Material enemyMaterial;        // Material del enemigo
    [SerializeField] private Material neutralMaterial;      // Material neutral
    [Space(10)]

    [Header("Meshes:")]
    [SerializeField] private MeshRenderer meshRenderer;     // Malla de la esfera
    [Space(10)]

    [Header("Capture area size")]
    [SerializeField] private int maxCaptureDistanceWidth;   // Distancia de captura ancho
    [SerializeField] private int maxCaptureDistanceHeight;  // Distancia de captura alto

    private GridPosition gridPosition;                      // Posicion en la malla de la puerta
    private InControlState state;                           // Estado para saber de quien es el control 
    private Action onInteractionComplete;                   // Accion para identificar cuando ha terminado la accion
    private bool isActive;                                  // Booleano para saber si la animacion esta activa 
    private float timer;                                    // Tiempo que tarda en interactuar la puerta

    public event EventHandler OnSphereCaptured;

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {
        // Establecemos la puerta en la celda en la que se situe
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(this, gridPosition);

        // Establecemos el color neutral
        SetColorNeutral();
    }
    public GridPosition GetGridPosition()
    {

        return gridPosition;

    }
    public int GetMaxCaptureDistanceWidth()
    {
        return maxCaptureDistanceWidth;
    }

    public int GetMaxCaptureDistanceHeight()
    {
        return maxCaptureDistanceHeight;
    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {
        // Comprobamos si la accion se ha activado
        if (!isActive)
        {

            // No hacemos nada si la accion esta inactiva
            return;

        }

        // Restamos el tiempo
        timer -= Time.deltaTime;

        // Comprobamos si el timer ha llegado a 0
        if (timer <= 0f)
        {

            // Llamamos a la accion
            isActive = false;
            onInteractionComplete();
            OnSphereCaptured(this, EventArgs.Empty);

        }

    }

    // @IGM -------------------------------------
    // Metodo para cambiar el color de la esfera.
    // ------------------------------------------
    private void SetColorPlayer()
    {

        // Establecemos el control en el jugador
        state = InControlState.Player;
        meshRenderer.material = playerMaterial;

    }

    // @IGM -------------------------------------
    // Metodo para cambiar el color de la esfera.
    // ------------------------------------------
    private void SetColorEnemy()
    {

        // Establecemos el control en el enemigo
        state = InControlState.Enemy;
        meshRenderer.material = enemyMaterial;

    }

    // @IGM -------------------------------------
    // Metodo para cambiar el color de la esfera.
    // ------------------------------------------
    private void SetColorNeutral()
    {

        // No establecemos el control en nadie
        state = InControlState.Neutral;
        meshRenderer.material = neutralMaterial;

    }

    // @IGM -------------------------------------
    // Metodo para interactuar con los elementos.
    // ------------------------------------------
    public void Interact(Action onInteractionComplete, Unit unit)
    {

        // Asignamos las variables
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 1f;

        // Si el control es del jugador
        if (state == InControlState.Player)
        {

            // Cambiamos el control al enemigo
            SetColorEnemy();

        }
        else if (state == InControlState.Enemy)
        {

            // Control del enemigo
            // Cambiamos al control del jugador
            SetColorPlayer();

        }
        else if (state == InControlState.Neutral)
        {

            // Control neutral
            // Comprobamos si la unidad que lo ha activado es un enemigo
            if (unit.IsEnemy())
            {

                // Cambiamos el control al enemigo
                SetColorEnemy();

            }
            else
            {

                // Cambiamos al control del jugador
                SetColorPlayer();

            }
        }

        FogOfWar.Instance.UpdateAllFogOfWar();
    }

    // @IGM --------------------------
    // Getter del estado de la esfera.
    // -------------------------------
    public InControlState GetInControlState()
    {

        return state;

    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] private bool isOpen;               // Booleano para saber si la puerta esta abierta

    private GridPosition gridPosition;                  // Posicion en la malla de la puerta
    private Animator animator;                          // Animador de la puerta
    private Action onInteractionComplete;               // Accion para identificar cuando ha terminado la accion
    private bool isActive;                              // Booleano para saber si la animacion esta activa 
    private float timer;                                // Tiempo que tarda en interactuar la puerta

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos el animador
        animator = GetComponent<Animator>();

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Establecemos la puerta en la celda en la que se situe
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(this, gridPosition);

        // Comprobamos si la puerta esta abierta o cerrada
        if (isOpen)
        {

            // Abrimos la puerta
            OpenDoor();

        }
        else
        {

            // Cerramos la puerta
            CloseDoor();

        }

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

        }

    }

    // @IGM -------------------------------------
    // Metodo para interactuar con los elementos.
    // ------------------------------------------
    public void Interact(Action onInteractionComplete, Unit unit)
    {

        // Asignamos las variables
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer =  1f;

        // Comprobamos si la puerta esta abierta o cerrada
        if (!isOpen)
        {

            // Abrimos la puerta
            OpenDoor();

        }
        else
        {

            // Cerramos la puerta
            CloseDoor();

        }

    }

    // @IGM -----------------------
    // Metodo para abrir la puerta.
    // ----------------------------
    private void OpenDoor()
    {

        // Activamos el animator
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);

        // Marcamos como caminable la poscion de la malla
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);

    }

    // @IGM ------------------------
    // Metodo para cerrar la puerta.
    // -----------------------------
    private void CloseDoor()
    {

        // Activamos el animator
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);

        // Marcamos como no caminable la poscion de la malla
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);

    }

}

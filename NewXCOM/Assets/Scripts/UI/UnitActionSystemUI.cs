using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;              // Prefab del boton de accion
    [SerializeField] private Transform actionButtonContainerTransform;  // Posicion de los botones de accion

    private List<ActionButtonUI> actionButtonUIList;                    // Lista de botones

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Inicializamos la lista
        actionButtonUIList = new List<ActionButtonUI>();

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;

        // Creamos los botones de las acciones de la unidad.
        CreateUnitActionButtons();
        UpdateSelectedVisual();

    }

    // @IGM ------------------------------------------------------
    // Metodo para crear los botones de las acciones de la unidad.
    // -----------------------------------------------------------
    private void CreateUnitActionButtons()
    {

        // Recorremos la lista de hijos del contenedor de botones
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {

            // Destruimos los botones que hayan
            Destroy(buttonTransform.gameObject);

        }

        // Limpiamos la lista de botones
        actionButtonUIList.Clear();

        // Asignamos la unidad seleccionada
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        // Recorremos el array de acciones
        foreach (BaseAction baseAction in unit.GetBaseActionArray())
        {

            // Instanciamos el prefab
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, 
                actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();

            // Establecemos la accion del boton
            actionButtonUI.SetBaseAction(baseAction);

            // Añadimos el boton a la lista de botones
            actionButtonUIList.Add(actionButtonUI);

        }

    }

    // @IGM -----------------------------------------
    // Handler del evento cuando se cambia de unidad.
    // ----------------------------------------------
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {

        // Creamos los botones de las acciones de la unidad.
        CreateUnitActionButtons();
        UpdateSelectedVisual();

    }

    // @IGM -----------------------------------------
    // Handler del evento cuando se cambia de accion.
    // ----------------------------------------------
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs empty)
    {

        // Actualizamos la seleccion de accion
        UpdateSelectedVisual();

    }

    // @IGM -----------------------------------------
    // Metodo para actualizar la accion seleccionada.
    // ----------------------------------------------
    private void UpdateSelectedVisual()
    {

        // Recorremos la lista de botones
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {

            // Actualizamos la seleccion de botones
            actionButtonUI.UpdateSelectedVisual();

        }
    }
}

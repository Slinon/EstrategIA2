using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{

    [SerializeField] private Unit unit;                 // Unidad seleccionada

    private MeshRenderer meshRenderer;                  // Malla de renderizado

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos la malla de renderizado
        meshRenderer = GetComponent<MeshRenderer>();

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos el evento de cambio de unidad
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        // Actualizamos el visual
        UpdateVisual();

    }

    // @IGM -----------------------------------------
    // Handler del evento cuando se cambia de unidad.
    // ----------------------------------------------
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {

        // Actualizamos el visual
        UpdateVisual();

    }

    // @IGM -----------------------------------------
    // Metodo para actualizar el visual de la unidad.
    // ----------------------------------------------
    private void UpdateVisual()
    {

        // Comprobamos si es la unidad seleccionada
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {

            // Habilitamos la malla de renderizado
            meshRenderer.enabled = true;

            // 

        }
        else
        {

            // Deshabilitamos la malla de renderizado
            meshRenderer.enabled = false;

        }

    }

    // @IGM ----------------------------------------------------------------------------------
    // Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.
    // ---------------------------------------------------------------------------------------
    private void OnDestroy()
    {

        // Desasignamos el evento de cambio de unidad
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;

    }

}

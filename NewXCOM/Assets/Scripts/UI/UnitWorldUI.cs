using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI actionPointsText;      // Texto de los puntos de accion
    [SerializeField] private Unit unit;                             // Unidad que gestiona el UI
    [SerializeField] private Image healthBarImage;                  // Imagen de la barra de vida
    [SerializeField] private HealthSystem healthSystem;             // Sistema de vida de la unidad


    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {
        // Asignamos los eventos
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        
       

        // Actualizamos las variables
        UpdateActionPointsText();
        UpdateHelthBar();

    }

    // @IGM -----------------------------------------------------
    // Metodo para actualizar el texto de las acciones restantes.
    // ----------------------------------------------------------
    private void UpdateActionPointsText()
    {

        // Actualizamos el texto
        actionPointsText.text = unit.GetActionPoints().ToString();

    }

    // @IGM --------------------------------------------------
    // Handler del evento cuando cambian los puntos de accion.
    // -------------------------------------------------------
    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs empty)
    {

        // Actualizamos los puntos de accion
        UpdateActionPointsText();

    }

    // @IGM ---------------------------------------
    // Handler del evento cuando se da�a la unidad.
    // --------------------------------------------
    private void HealthSystem_OnDamaged(object sender, EventArgs empty)
    {

        // Actualizamos la barra de vida
        UpdateHelthBar();

    }

    // @IGM -----------------------------------
    // Metodo para actualizar la barra de vida.
    // ----------------------------------------
    private void UpdateHelthBar()
    {

        // Actualizamos la barra de vida
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();

    }




}

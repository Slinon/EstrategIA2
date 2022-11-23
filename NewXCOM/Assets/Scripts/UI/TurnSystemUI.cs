using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnButton;                  // Boton de finalizar el turno
    [SerializeField] private TextMeshProUGUI turnNumberText;        // Texto del numero del turno actual
    [SerializeField] private GameObject enemyTurnVisualGameObject;  // Objeto visual del turno del enemigo

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos el onClick con una funcion anonima
        endTurnButton.onClick.AddListener(() =>
        {

            // Pasamos al turno siguiente
            TurnSystem.Instance.NextTurn();

        });

        // Asignamos los eventos
        TurnSystem.Instance.OnTurnChanged += UnitActionSystem_OnTurnChanged;

        // Actualizamos los campos
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();

    }

    // @IGM ----------------------------------------
    // Handler del evento cuando cambiamos de turno.
    // ---------------------------------------------
    private void UnitActionSystem_OnTurnChanged(object sender, EventArgs empty)
    {

        // Actualizamos el turno
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();

    }

    // @IGM --------------------------------------------
    // Metodo para actualizar el texto del turno actual.
    // -------------------------------------------------
    private void UpdateTurnText()
    {

        // Actualizamos el texto con el numero del turno actual
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();

    }

    // @IGM -------------------------------------------------
    // Metodo para actualizar el campo del turno del enemigo.
    // ------------------------------------------------------
    private void UpdateEnemyTurnVisual()
    {

        // Actuvamos el campo en el turno del enemigo
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());

    }

    // @IGM --------------------------------------------------------------
    // Metodo para actualizar la visibilidad del boton de finalizar turno.
    // -------------------------------------------------------------------
    private void UpdateEndTurnButtonVisibility()
    {

        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());

    }

}

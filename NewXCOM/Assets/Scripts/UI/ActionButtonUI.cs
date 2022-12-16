using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshPro;       // Texto del boton
    [SerializeField] private Button button;                     // Boton de accion
    [SerializeField] private GameObject selectedGameObject;     // GameObject de seleccion

    private BaseAction baseAction;                              // Accion que contiene el boton

    private void Update()
    {

        if (baseAction.MoneyCost() > MoneySystem.Instance.player.money)
        {
            button.interactable = false;
        }

        // Comprobamos si hemos superado la capacidad de estructuras construidas
        if (baseAction is BuildStructureAction && 
            (baseAction as BuildStructureAction).GetStructureCount() 
            >= (baseAction as BuildStructureAction).GetMaxStructureCount())
        {

            button.interactable = false;

        }

    }


    // @IGM --------------------------------------
    // Metodo para establecer la accion del boton.
    // -------------------------------------------
    public void SetBaseAction(BaseAction baseAction)
    {

        // Establecemos la accion
        this.baseAction = baseAction;

        // Establecemos el nombre de la accion
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        // Asignamos el onClick con una funcion anonima
        button.onClick.AddListener(() =>
        {

            // Seleccionamos la accion del boton
            UnitActionSystem.Instance.SetSelectedAction(baseAction);

        });

    }

    // @IGM -----------------------------------------
    // Metodo para actualizar la accion seleccionada.
    // ----------------------------------------------
    public void UpdateSelectedVisual()
    {
        // Activamos el objeto si la accion del boton es la seleccionada
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }

}

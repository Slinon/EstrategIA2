using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos el evento de cambio de unidad y accion
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        // Ocultamos el panel
        Hide();

    }

    // @IGM ------------------------
    // Metodo para mostrar el panel.
    // -----------------------------
    private void Show()
    {

        // Activamos el objeto
        gameObject.SetActive(true);

    }

    // @IGM ------------------------
    // Metodo para ocultar el panel.
    // -----------------------------
    private void Hide()
    {

        // Desativamos el objeto
        gameObject.SetActive(false);

    }

    // @IGM ------------------------------------------------------
    // Handler del evento cuando se cambia el estado de la accion.
    // -----------------------------------------------------------
    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {

        // Comprobamos si la accion ha empezado
        if (isBusy)
        {

            // Mostramos el panel
            Show();

        }
        else
        {

            // Ocultamos el panel
            Hide();

        }

    }

}

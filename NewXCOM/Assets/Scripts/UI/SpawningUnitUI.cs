using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningUnitUI : MonoBehaviour
{
    /*
    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        SpawnUnitAction.onAnySpawnUnitChanched += SpawnUnitAction_onAnySpawnUnitChanched;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

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
    private void SpawnUnitAction_onAnySpawnUnitChanched(object sender, bool isWaiting)
    {

        // Comprobamos si la accion esta en espera
        if (isWaiting)
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

    // @IGM ------------------------------------------------------
    // Handler del evento cuando se cambia el estado de la accion.
    // -----------------------------------------------------------
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {

        // Comrpobamos si la unidad tiene la accion de spawn
        foreach (BaseAction baseAction in UnitActionSystem.Instance.GetSelectedUnit().GetBaseActionArray())
        {

            if (baseAction is SpawnUnitAction)
            {

                // Asignamos la accion
                SpawnUnitAction spawnUnitAction = baseAction as SpawnUnitAction;
                UnitActionSystem.Instance.SetSelectedAction(spawnUnitAction);

                // Comprobamos si la accion esta en progreso
                if (spawnUnitAction.IsWaiting())
                {

                    // Mostramos el panel
                    Show();
                    break;

                }
                else
                {

                    // Ocultamos el panel
                    Hide();
                    break;

                }
                

            }
            else
            {

                // Ocultamos el panel
                Hide();

            }

        }

    }
    */
}

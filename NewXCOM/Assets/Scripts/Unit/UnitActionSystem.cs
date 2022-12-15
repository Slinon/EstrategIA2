using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{

    public static UnitActionSystem Instance { get; private set; }       // Instancia del singleton

    public event EventHandler OnSelectedUnitChanged;                    // Evento cuando se cambia de unidad seleccionada
    public event EventHandler OnSelectedActionChanged;                  // Evento cuando se cambia de accion seleccionada
    public event EventHandler<bool> OnBusyChanged;                      // Evento cuando se cambia el estado de la accion seleccionada
    public event EventHandler OnActionStarted;                          // Evento cuando inicia una accion

    [SerializeField] private Unit selectedUnit;                         // Unidad seleccionada
    [SerializeField] private LayerMask unitLayerMask;                   // Capa de las unidades

    private BaseAction selectedAction;                                  // Accion seleccionada
    private bool isBusy;                                                // Booleano para indicar que hay una accion en curso

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si hay una instancia del objeto
        if (Instance != null)
        {

            // Lo eliminamos
            Debug.LogError("Mas de un UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;

        }

        // Asignamos el objeto a la instancia
        Instance = this;

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Establecemos la unidad seleccionada por defecto
        SetSelectedUnit(selectedUnit);

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {

        // Comprobamos si hay una accion en curso
        if (isBusy)
        {

            // No hacemos nada
            return;

        }

        // Comprobamos si es el turno del enemigo
        if (!TurnSystem.Instance.IsPlayerTurn())
        {

            // No hacemos nada
            return;

        }

        // Comprobamos si el raton esta sobre el canvas de los botones
        if (EventSystem.current.IsPointerOverGameObject())
        {

            // No hacemos nada
            return;

        }

        // Comprobamos si se acaba de seleccionar una unidad
        if (TryHandleUnitSelection())
        { 
            
            // No realizamos ninguna accion
            return;
        
        }

        // Gestionamos la accion seleccionada
        HandleSelectedAction();

    }

    // @IGM ----------------------------------------
    // Metodo para gestionar la accion seleccionada.
    // ---------------------------------------------
    private void HandleSelectedAction()
    {

        // Comprobamos si se ha hecho click izquierdo
        if (InputManager.Instance.IsLeftMouseButtonDownThisFrame())
        {

            // Calculamos la posicion del raton en la malla
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            // Comprobamos si no es una posicion valida
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {

                // No hacemos nada
                return;

            }

            // Intentamos si no se puede realizar la accion
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {

                // No hacemos nada
                return;

            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (!selectedUnit.IsEnemy()) //Si se trata del jugador
            {

                if (selectedAction.MoneyCost() <= MoneySystem.Instance.player.money)
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);                 

                }

                else
                {

                    Debug.Log("Esta acción NO se puede realizar, devolviendo el punto de acción");
                    selectedUnit.GiveActionPointBack();

                }

            }
         

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            // Comprobamos si hay alguna clase escuchando el evento
            if (OnActionStarted != null)
            {

                // Lanzamos el evento
                OnActionStarted(this, EventArgs.Empty);

            }

        }

    }

    // @IGM -------------------------------------------
    // Metodo para indicar que hay una accion en curso.
    // ------------------------------------------------
    private void SetBusy()
    {

        // Marcamos que la accion ha comenzado
        isBusy = true;

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnBusyChanged != null)
        {

            // Lanzamos el evento
            OnBusyChanged(this, isBusy);

        }

    }

    // @IGM --------------------------------------------------
    // Metodo para indicar que no hay ninguna accion en curso.
    // -------------------------------------------------------
    private void ClearBusy()
    {

        // La accion ha terminado
        isBusy = false;

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnBusyChanged != null)
        {

            // Lanzamos el evento
            OnBusyChanged(this, isBusy);

        }

    }

    // @IGM ----------------------------------------
    // Funcion para intentar seleccionar una unidad.
    // ---------------------------------------------
    private bool TryHandleUnitSelection()
    {

        // Comprobamos si se ha hecho click izquierdo
        if (InputManager.Instance.IsLeftMouseButtonDownThisFrame())
        {

            // Comprobamos si el rayo lanzado desde la posicion del raton ha colisionado con una unidad
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
           
                // Comprobamos que la unidad tenga el script
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                   
                    // Comprobamos si la unidad que se ha pulsado es la seleccionada
                    if (unit == selectedUnit)
                    {

                        // Queremos realizar una accion no volver a seleccionar la unidad
                        return false;

                    }

                    // Comrpobamos si la unidad es un enemigo
                    if (unit.IsEnemy())
                    {

                        // No seleccionamos la unidad
                        return false;

                    }

                    // Establecemos la unidad
                    SetSelectedUnit(unit);
                    return true;

                }

            }

        }

        // No se ha seleccionado ninguna unidad
        return false;

    }

    // @IGM ---------------
    // Setter de la unidad.
    // --------------------
    public void SetSelectedUnit(Unit unit)
    {

        // Asignamos la unidad y la accion de movimiento por defecto
        selectedUnit = unit;

        // Comprobamos que la unidad tiene accion de movimiento
        if (unit.GetComponent<MoveAction>())
        {

            // Establecemos la accion de movimiento por defecto
            SetSelectedAction(unit.GetAction<MoveAction>());

        }
        else
        {

            SetSelectedAction(unit.GetAction<PassAction>());

        }
        
        // Comprobamos si hay alguna clase escuchando el evento
        if(OnSelectedUnitChanged != null)
        {

            // Lanzamos el evento
            OnSelectedUnitChanged(this, EventArgs.Empty);

        }
          
    }

    // @IGM ----------------------------
    // Setter de la accion seleccionada.
    // ---------------------------------
    public void SetSelectedAction(BaseAction baseAction)
    {

        // Asignamos la accion
        selectedAction = baseAction;

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnSelectedActionChanged != null)
        {

            // Lanzamos el evento
            OnSelectedActionChanged(this, EventArgs.Empty);

        }

    }

    // @IGM ---------------
    // Getter de la unidad.
    // --------------------
    public Unit GetSelectedUnit()
    {

        return selectedUnit;

    }

    // @IGM ---------------
    // Getter de la accion.
    // --------------------
    public BaseAction GetSelectedAction()
    {

        return selectedAction;

    }

}

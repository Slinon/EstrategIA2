using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointsChanged;      // Evento cuando cambia el numero de acciones disponibles
    public static event EventHandler OnAnyUnitDied;                 // Evento cuando una unidad muere
    public static event EventHandler OnAnyUnitSpawned;              // Evento cuando aparece una tropa nueva
    public static event EventHandler OnAnyUnitMoved;
    public event EventHandler OnCoverChanged;                       //Evento para activar la animacion de Cobertura

    [SerializeField] private int maxActionPoints;                   // Puntos de accion maximos de la unidad
    [SerializeField] private bool isEnemy;                          // Booleano para saber si la unidad es enemiga
    [SerializeField] private CoverType coverType;                   // Tipo de covertura que tiene la unidad

    private GridPosition gridPosition;                              // Posicion de la malla donde esta la unidad
    private HealthSystem healthSystem;                              // Sistema de salud de la unidad
    private BaseAction[] baseActionArray;                           // Array de acciones de la unidad
    private int actionPoints;                                       // Puntos de accion de la unidad


    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Canvas UnitWorldUICanvas;
    [SerializeField] private bool isStructure = false;

    [SerializeField] LayerMask obstacleLayerMask;

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {
        // Asignamos las acciones al array
        baseActionArray = GetComponents<BaseAction>();

        // Asignamos el sistema de salud
        healthSystem = GetComponent<HealthSystem>();

        // Asignamos el numero de acciones disponibles
        actionPoints = maxActionPoints;

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        // @VJT Buscamos la acción de movimiento y asignamos el evento
        MoveAction moveAction = GetAction<MoveAction>();
        if(moveAction != null) 
        {
            moveAction.OnPositionChanged += MoveAction_OnPositionChanged;
        }

        // Asignamos la posicion de la malla donde esta la unidad
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        UpdateCoverType();

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyUnitSpawned != null)
        {

            // Lanzamos el evento
            OnAnyUnitSpawned(this, EventArgs.Empty);

        }

    }

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    void Update()
    {

        // Comprobamos si la posicion en la que se encuentra la unidad en la malla es diferente a la que teniamos
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {

            // Actualizamos la posicion
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);

        }
        UpdateCoverType();

        // Field of view
        //fieldOfView.SetOrigin(transform.position);
        //Debug.Log("setting new origin: " + transform.position);
        
    } 

    // @IGM ------------------------------------------
    // Getter de la posicion en la malla de la unidad.
    // -----------------------------------------------
    public GridPosition GetGridPosition()
    {

        return gridPosition;

    }

    // @IGM ------------------------------
    // Getter de la posicion de la unidad.
    // -----------------------------------
    public Vector3 GetWorldPosition()
    {

        return transform.position;

    }

    // @IGM ------------------------
    // Getter generico de la accion.
    // -----------------------------
    public T GetAction<T>() where T: BaseAction
    {

        // Recorremos los tipos de acciones
        foreach (BaseAction baseAction in baseActionArray)
        {

            // Comprobamos si la accion es del tipo que queremos
            if (baseAction is T)
            {

                // Devolvemos la accion
                return (T)baseAction;

            }

        }

        return null;

    }

    // @IGM ------------------------
    // Getter del array de acciones. 
    // -----------------------------
    public BaseAction[] GetBaseActionArray()
    {

        return baseActionArray;

    }

    // @IGM -----------------------------------------------------
    // Funcion para intentar gastar puntos de accion disponibles.
    // ----------------------------------------------------------
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {

        // Comprobamos si hay puntos para realizar la accion
        if (CanSpendActionPointsToTakeAction(baseAction))
        {

            // Gastamos los puntos
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;

        }
        else
        {

            // No tenemos puntos de accion suficientes
            return false;

        }

    }

    // @IGM ---------------------------------------------------------
    // Funcion para saber si aun quedan puntos de accion disponibles.
    // --------------------------------------------------------------
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {

        if (actionPoints >= baseAction.GetActionPointsCost())
        {

            return true;

        }
        else
        {

            return false;

        }

    }

    // @IGM -------------------------------
    // Metodo para gastar puntos de accion.
    // ------------------------------------
    public void SpendActionPoints(int amount)
    {

        // Restamos la cantidad de puntos que cuesta la accion
        actionPoints -= amount;

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyActionPointsChanged != null)
        {

            // Lanzamos el evento
            OnAnyActionPointsChanged(this, EventArgs.Empty);

        }

    }

    // @IGM --------------------------
    // Getter de los puntos de accion.
    // -------------------------------
    public int GetActionPoints()
    {

        return actionPoints;

    }

    // @IGM ----------------------------------------
    // Handler del evento cuando cambiamos de turno.
    // ---------------------------------------------
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {

        // Comprobamos si se ha cambiado de turno y de jugador
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || 
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {

            // Reseteamos los puntos de accion
            actionPoints = maxActionPoints;

            // Comprobamos si hay alguna clase escuchando el evento
            if (OnAnyActionPointsChanged != null)
            {

                // Lanzamos el evento
                OnAnyActionPointsChanged(this, EventArgs.Empty);

            }

        } 

    }

    // @IGM -----------------------------------------------
    // Funcion para saber si la unidad es aliada o enemiga.
    // ----------------------------------------------------
    public bool IsEnemy()
    {

        return isEnemy;

    }

    // @IGM ------------------------------
    // Metodo para hacer da�o a la unidad.
    // -----------------------------------
    public void Damage(Vector2 damageAmount)
    {

        healthSystem.Damage(damageAmount);

    }

    // @IGM -------------------------------------
    // Handler del evento cuando muere la unidad.
    // ------------------------------------------
    private void HealthSystem_OnDead(object sender, EventArgs empty)
    {

        // Eliminamos la unidad de la malla
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyUnitDied != null)
        {

            // Lanzamos el evento
            OnAnyUnitDied(this, EventArgs.Empty);

        }

    }

    // @IGM --------------------------------------------------
    // Funcion para saber la salud de la unidad en porcentaje.
    // -------------------------------------------------------
    public float GetHealthNormalized()
    {

        return healthSystem.GetHealthNormalized();

    }

    // @VJT --------------------------------------------------
    // Handler del evento cuando se mueve la unidad
    // -------------------------------------------------------
    private void MoveAction_OnPositionChanged(object sender, EventArgs empty)
    {
        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyUnitMoved != null)
        {

            // Lanzamos el evento
            OnAnyUnitMoved(this, EventArgs.Empty);

        }

    }

    public void SetIsVisible(bool isVisible)
    {
        if (isStructure) return; //Las estructuras como torretas y la base no son camufladas por el FoW, al ser entidades inmoviles.

        else
        {
            skinnedMeshRenderer.enabled = isVisible;
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.enabled = isVisible;
            }
            UnitWorldUICanvas.enabled = isVisible;
        }

    }

    //@GRG le devolvemos el puntito si el pobre no ha podido hacer la acci�n por estar pobre :(
    public void GiveActionPointBack()
    {
        actionPoints += 1;
        OnAnyActionPointsChanged(this, EventArgs.Empty);
    }


    private void UpdateCoverType()
    {
        coverType = LevelGrid.Instance.GetUnitCoverType(GetWorldPosition());
        
        if(coverType == CoverType.Covered)
        {
            OnCoverChanged?.Invoke(this, EventArgs.Empty);
        }

    }

    public CoverType GetCoverType()
    {
        return coverType;
    }

    public int GetDistanceBetweenUnits(Unit selected, Unit target)
    {
        return (Mathf.Abs(selected.GetGridPosition().x - target.GetGridPosition().x)) + Mathf.Abs(Mathf.Abs(selected.GetGridPosition().z - target.GetGridPosition().z));
    }


    //EMF, GRG ---------------------------------------------------
    //Checks if this unit is in LineOfSight of a given player unit
    //------------------------------------------------------------
    public bool ThisUnitIsInSight(Unit selectedUnit)
    {
        //Origen del rayo
        float unitShoulderHeight = 1.7f;

        //Obtiene la posición de la unidad pasada como parámetro
        Vector3 selectedUnitWorldPosition = LevelGrid.Instance.GetWorldPosition(selectedUnit.GetGridPosition());

        //Dirección del rayo
        Vector3 shootDirection = (selectedUnitWorldPosition + Vector3.down * 1f - this.GetWorldPosition()).normalized;

        if (selectedUnit.TryGetComponent(out ShootAction shootAction))
        {
            //Si golpea contra un obstaculo
            if (Physics.Raycast
                (this.GetWorldPosition() + Vector3.up * unitShoulderHeight, shootDirection, shootAction.GetMaxShootDistance(), obstacleLayerMask))
            {

                //falso, no está siendo visto.
                return false;
            }
            else
            {
                //está siendo visto
                return true;
            }
        }
        else return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAIManager : MonoBehaviour
{

    public static event EventHandler<Unit> OnAnyUnitThrowGrenade;   // Evento cuando una unidad decide lanzar una granada
    public static event EventHandler<Unit> OnAnyUnitBuildStructure; // Evento cuando una unidad decide construir una estructura
    public static event EventHandler<Unit> OnAnyUnitMoveAction;     // Evento cuando una unidad decide moverse

    [SerializeField] private int maxAIValueAction;                  // Valor maximo de una accion para la IA
    [SerializeField] private int minAIValueAction;                  // Valor minimode una accion para la IA

    private Unit unit;                                              // Unidad a la que afecta el arbol de decisiones

    // @GRG ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos la unidad
        unit = GetComponent<Unit>();

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        EnemyAI.OnAnyAIUnitIsSelected += EnemyAI_OnAnyAIUnitIsSelected;

    }

    // @IGM ------------------------------------------------------------
    // Handler del evento cuando se ha seleccionado una unidad de la IA.
    // -----------------------------------------------------------------
    private void EnemyAI_OnAnyAIUnitIsSelected(object sender, EventArgs empty)
    {

        // Asignamos los valores de las acciones
        CheckConditions();

    }

    // @GRG ------------------------------------------------
    // Metodo para comprobar que acción debe realizar la IA.
    // -----------------------------------------------------
    private void CheckConditions()
    {

        // Comprobamos si la unidad es una unidad del jugador
        if (unit == null || !unit.IsEnemy())
        {

            return;

        }

        // Reestablecemos los valores de las acciones
        RestartValues();
        
        // Si puedo interactuar
        if (unit.TryGetComponent(out InteractAction interactAction))
        {

            // Y tengo una esfera cerca
            if (Checkers.Instance.IsCloseToSphere(unit))
            {
           
                // Si la esfera esta al lado
                if (Checkers.Instance.IsSphereNearby(unit))
                {
               
                    // interactuo
                    SetValues(interactAction, maxAIValueAction, minAIValueAction);
                    return;

                }

                // Si no esta al lado, me muevo
                else if (unit.TryGetComponent(out MoveAction moveAction))
                {

                    // Comprobamos si hay alguna clase escuchando el evento
                    if (OnAnyUnitMoveAction != null)
                    {

                        // Lanzamos el evento
                        OnAnyUnitMoveAction(this, unit);

                    }

                    // me muevo (a la esfera)
                    SetValues(moveAction, maxAIValueAction, minAIValueAction);
                    return;

                }

            }

        }

        // Si tengo dos o mas puntos de accion
        if (Checkers.Instance.GetRemainingActionPoints(unit) > 1)
        {

            // puedo construir
            if (unit.TryGetComponent(out BuildStructureAction buildStructureAction))
            {

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnAnyUnitBuildStructure != null)
                {

                    // Lanzamos el evento
                    OnAnyUnitBuildStructure(this, unit);

                }

                // es factible construir
                if (Checkers.Instance.IsValidStructure(unit))
                {

                    // construyo
                    SetValues(buildStructureAction, maxAIValueAction, minAIValueAction);
                    return;

                }

            }

            // puedo moverme
            else if (unit.TryGetComponent(out MoveAction moveAction))
            {

                // no estoy en la mejor posicion
                if (!Checkers.Instance.UnitInBestPosition(unit))
                {

                    // Comprobamos si hay alguna clase escuchando el evento
                    if (OnAnyUnitMoveAction != null)
                    {

                        // Lanzamos el evento
                        OnAnyUnitMoveAction(this, unit);

                    }

                    // me muevo
                    SetValues(moveAction, maxAIValueAction, minAIValueAction);
                    return;

                }

            }

        }

        // Si no tengo mas de dos puntos y puedo disparar
        if (unit.TryGetComponent(out ShootAction shootAction))
        {

            // Si tengo enemigos cerca
            if (Checkers.Instance.AreEnemiesNearby(unit))
            {
  
                // Si puedo palmar
                if (Checkers.Instance.CouldBeKilled(unit))
                {
          
                    // Si me puedo mover
                    if (unit.TryGetComponent(out MoveAction moveAction))
                    {

                        // Comprobamos si hay alguna clase escuchando el evento
                        if (OnAnyUnitMoveAction != null)
                        {

                            // Lanzamos el evento
                            OnAnyUnitMoveAction(this, unit);

                        }

                        // me muevo
                        SetValues(moveAction, maxAIValueAction, minAIValueAction);
                        return;

                    }

                }

                // si no puedo morir...
                
                // si los tengo a mele
                else if (Checkers.Instance.IsEnemyPointBlank(unit))
                {
             
                    // Les meto espadazo si puedo
                    if (unit.TryGetComponent(out SwordAction swordAction))
                    {

                        SetValues(swordAction, maxAIValueAction, minAIValueAction);
                        return;

                    }

                }

                // Si estan lejos, y tengo granada
                else if (unit.TryGetComponent(out GrenadeAction grenadeAction))
                {
                 
                    // Si hay una posicion donde lanzar granada
                    if (Checkers.Instance.IsValidGrenade(unit))
                    {

                        // Comprobamos si hay alguna clase escuchando el evento
                        if (OnAnyUnitThrowGrenade != null)
                        {

                            // Lanzamos el evento
                            OnAnyUnitThrowGrenade(this, unit);

                        }

                        // Granadazo
                        SetValues(grenadeAction, maxAIValueAction, minAIValueAction);
                        return;

                    }

                }

                // Si no, les pego un tiro
                SetValues(shootAction, maxAIValueAction, minAIValueAction);
                return;

            }
      
            // si no se cumple ninguna, pero me puedo mover
            else if (unit.TryGetComponent(out MoveAction moveAction))
            {

                // Comprobamos si hay alguna clase escuchando el evento
                if (OnAnyUnitMoveAction != null)
                {

                    // Lanzamos el evento
                    OnAnyUnitMoveAction(this, unit);

                }

                // me muevo
                SetValues(moveAction, maxAIValueAction, minAIValueAction);
                return;

            }

        }

    }

    // @GRG ------------------------------------------------
    // Metodo para reestablecer los valores de las acciones.
    // -----------------------------------------------------
    private void RestartValues()
    {

        // Recorremos la lista de acciones validas
        foreach (BaseAction action in unit.GetBaseActionArray())
        {

            // Establecemos su valor de IA base a 0
            action.SetBaseAIValue(0);

        }

    }

    // @IGM ----------------------------------------------
    // Metodo para establecer los valores de las acciones.
    // ---------------------------------------------------
    private void SetValues(BaseAction chosenAction, int maxValue, int minValue)
    {
        
        // Recorremos la lista de acciones validas
        foreach (BaseAction action in unit.GetBaseActionArray())
        {
            
            if (action == chosenAction)
            {

                action.SetBaseAIValue(maxValue);

            }
            else
            {

                action.SetBaseAIValue(minValue);

            }

        }

    }

}

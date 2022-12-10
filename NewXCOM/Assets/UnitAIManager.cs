using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAIManager : MonoBehaviour
{
    Unit thisUnit;

    private void Awake()
    {
        thisUnit = GetComponent<Unit>();
    }

    public BaseAction[] GetUnitActions()
    {
        return thisUnit.GetBaseActionArray();
    }

    private void CheckConditions()
    {
        RestartValues();

        //Si puedo interactuar
        if (thisUnit.TryGetComponent(out InteractAction interactAction))
        {
            //Y tengo una esfera cerca
            if (Checkers.Instance.IsCloseToSphere(thisUnit))
            {
                //Si la esfera esta al lado
                if (Checkers.Instance.IsSphereNearby(thisUnit))
                {
                    //interactuo
                    interactAction.SetBaseValue(100);
                    return;

                }

                //Si no esta al lado, me muevo
                else if (thisUnit.TryGetComponent(out MoveAction moveAction))
                {
                    //me muevo (a la esfera)
                    moveAction.SetBaseValue(100);
                    return;

                }

            }

        }

        //Si tengo dos o mas puntos de accion
        if (Checkers.Instance.GetRemainingActionPoints(thisUnit) > 1)
        {
            //y puedo moverme
            if (thisUnit.TryGetComponent(out MoveAction moveAction))
            {
                //me muevo
                moveAction.SetBaseValue(100);
                return;
            }

        }


        //Si no tengo mas de dos puntos
        if (thisUnit.TryGetComponent(out ShootAction shootAction))
        {
            //Si tengo enemigos cerca
            if (Checkers.Instance.AreEnemiesNearby(thisUnit))
            {
                //Si puedo palmar
                if (Checkers.Instance.CouldBeKiled(thisUnit))
                {
                    //Si me puedo mover
                    if (thisUnit.TryGetComponent(out MoveAction moveAction))
                    {
                        //me muevo
                        moveAction.SetBaseValue(100);
                        return;
                    }
                }

                //si no puedo morir...

                //si los tengo a mele
                else if (Checkers.Instance.IsEnemyPointBlank(thisUnit))
                {
                    //Les meto espadazo si puedo
                    if (thisUnit.TryGetComponent(out SwordAction swordAction))
                    {

                        swordAction.SetBaseValue(100);
                        return;

                    }

                    //Si no le pego un tirito
                    else
                    {

                        shootAction.SetBaseValue(100);
                        return;

                    }
                }

                //Si estan lejos, y tengo granada
                else if (thisUnit.TryGetComponent(out GrenadeAction grenadeAction))
                {

                    //Granadazo
                    grenadeAction.SetBaseValue(100);
                    return;

                }

                else
                {

                    shootAction.SetBaseValue(100);
                    return;

                }

            }

            //si no se cumple ninguna, pero me puedo mover
            else if (thisUnit.TryGetComponent(out MoveAction moveAction))
            {
                //me muevo
                moveAction.SetBaseValue(100);
                return;
            }
        }
    }

    void RestartValues()
    {
        BaseAction[] unitActions = GetUnitActions();

        foreach (BaseAction action in unitActions)
        {
            action.SetBaseValue(0);
        }
    }
}

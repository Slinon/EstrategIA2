using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject;     // GameObject de la camara de accion

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        // Ocultamos la camara de accion
        HideActionCamera();

    }

    // @IGM -------------------------
    // Mostramos la camara de accion.
    // ------------------------------
    private void ShowActionCamera()
    {

        actionCameraGameObject.SetActive(true);

    }

    // @IGM -------------------------
    // Ocultamos la camara de accion.
    // ------------------------------
    private void HideActionCamera()
    {

        actionCameraGameObject.SetActive(false);

    }

    // @IGM ------------------------------------------
    // Handler del evento cuando se inicia una accion.
    // -----------------------------------------------
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs empty)
    {

        // Comprobamos que accion es la que ha llamado al evento
        switch (sender)
        {

            // Accion de disparar
            case ShootAction shootAction:

                // Asignamos las unidades
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                // Creamos el vector de la camara
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                // Calculamos la direccion de disparo
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - 
                    shooterUnit.GetWorldPosition()).normalized;

                // Asignamos el offset del hombro
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * 
                    shootDirection * shoulderOffsetAmount;

                // Calculamos la posicion de la camara de accion
                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + 
                    cameraCharacterHeight + shoulderOffset + (shootDirection * -1);

                // Cambiamos la posicion de la camara de accion
                actionCameraGameObject.transform.position = actionCameraPosition;

                // Rotamos la camara hacia el objetivo
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + 
                    cameraCharacterHeight);

                // Mostramos la camara
                ShowActionCamera();
                break;

            // Accion de dar un espadazo
            case SwordAction swordAction:

                // Asignamos las unidades
                Unit sworderUnit = swordAction.GetUnit();
                targetUnit = swordAction.GetTargetUnit();

                // Creamos el vector de la camara
                cameraCharacterHeight = Vector3.up * 1.7f;

                // Calculamos la direccion de disparo
                shootDirection = (targetUnit.GetWorldPosition() -
                    sworderUnit.GetWorldPosition()).normalized;

                // Asignamos el offset del hombro
                shoulderOffsetAmount = 3f;
                shoulderOffset = Quaternion.Euler(0, 90, 0) *
                    shootDirection * shoulderOffsetAmount;

                // Calculamos la posicion de la camara de accion
                actionCameraPosition = sworderUnit.GetWorldPosition() +
                    cameraCharacterHeight + shoulderOffset + (shootDirection * -3);

                // Cambiamos la posicion de la camara de accion
                actionCameraGameObject.transform.position = actionCameraPosition;

                // Rotamos la camara hacia el objetivo
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() +
                    cameraCharacterHeight);

                // Mostramos la camara
                ShowActionCamera();

                break;

        }

    }

    // @IGM ------------------------------------------
    // Handler del evento cuando se inicia una accion.
    // -----------------------------------------------
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs empty)
    {

        // Comprobamos que accion es la que ha llamado al evento
        switch (sender)
        {

            // Accion de disparar
            case ShootAction shootAction:
                // Mostramos la camara
                HideActionCamera();
                break;

            // Accion de dar un espadazo
            case SwordAction swordAction:
                // Mostramos la camara
                HideActionCamera();
                break;

        }

    }

}

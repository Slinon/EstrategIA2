using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator unitAnimator;             // Animator de la unidad
    [SerializeField] private Transform bulletProjectilePrefab;  // Prefab de la bala que dispara la unidad
    [SerializeField] private Transform shootPointTransform;     // Posicion de donde sale la bala
    [SerializeField] private Transform rifleTransform;          // Posicion del rifle
    [SerializeField] private Transform swordTransform;          // Posicion de la espada

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si la unidad tiene accion de moverse
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {

            // Asignamos los eventos
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;

        }

        // Comprobamos si la unidad tiene accion de disparar
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {

            shootAction.OnShoot += MoveAction_OnShoot;

        }

        // Comprobamos si la unidad tiene accion de atacar con espada
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {

            // Asignamos los eventos
            swordAction.OnSwordActionStarted += MoveAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += MoveAction_OnSwordActionCompleted;

        }

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Equipamos el rifle al inicio
        EquipeRifle();

    }

    // @IGM ------------------------------------------------------------------
    // Handler del evento cuando la unidad empieza el movimiento de la espada.
    // -----------------------------------------------------------------------
    private void MoveAction_OnSwordActionStarted(object sender, EventArgs empty)
    {

        // Activamos la animacion
        EquipeSword();
        unitAnimator.SetTrigger("SwordSlash");

    }

    // @IGM ------------------------------------------------------------------
    // Handler del evento cuando la unidad termina el movimiento de la espada.
    // -----------------------------------------------------------------------
    private void MoveAction_OnSwordActionCompleted(object sender, EventArgs empty)
    {

        // Equipamos el rifle de nuevo
        EquipeRifle();

    }

    // @IGM ----------------------------------------
    // Handler del evento cuando la unidad se mueve.
    // ---------------------------------------------
    private void MoveAction_OnStartMoving(object sender, EventArgs empty)
    {

        // Activamos la animacion
        unitAnimator.SetBool("IsWalking", true);
        unitAnimator.SetBool("Crouch", false);

    }

    // @IGM -------------------------------------------
    // Handler del evento cuando la unidad no se mueve.
    // ------------------------------------------------
    private void MoveAction_OnStopMoving(object sender, EventArgs empty)
    {

        // Activamos la animacion
        unitAnimator.SetBool("IsWalking", false);

    }

    // @IGM ---------------------------------------
    // Handler del evento cuando la unidad dispara.
    // --------------------------------------------
    private void MoveAction_OnShoot(object sender, Unit targetUnit)
    {

        // Activamos la animacion
        unitAnimator.SetTrigger("Shoot");

        // Instanciamos la bala
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, 
            shootPointTransform.position, Quaternion.identity);

        // Buscamos el componente de la bala
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        // Actualizamos la altura del disparo
        Vector3 targetUnitShootAtPosition = targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        // Establecemos los parametros de la bala
        bulletProjectile.Setup(targetUnitShootAtPosition);

    }

    // @IGM -------------------------
    // Metodo para equipar la espada.
    // ------------------------------
    private void EquipeSword()
    {

        // Activamos la espada
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);

    }

    // @IGM -------------------------
    // Metodo para equipara el rifle.
    // ------------------------------
    private void EquipeRifle()
    {

        // Activamos el rifle
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);

    }

}

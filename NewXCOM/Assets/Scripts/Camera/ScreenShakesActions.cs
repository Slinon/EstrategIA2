using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakesActions : MonoBehaviour
{

    [SerializeField] private float grenadeShake;            // Temblor al explotar una granada
    [SerializeField] private float swordShake;              // Temblor al pegar un espadazo

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Asignamos los eventos
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;

    }

    // @IGM -----------------------------------------
    // Handler del eventoi cuando una unidad dispara.
    // ----------------------------------------------
    private void ShootAction_OnAnyShoot(object sender, Unit targetUnit)
    {

        // Instancianciamos el movimiento de la camara
        ScreenShake.Instance.Shake();

    }

    // @IGM ------------------------------------------
    // Handler del eventoi cuando una granada explota.
    // -----------------------------------------------
    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs empty)
    {

        // Instancianciamos el movimiento de la camara
        ScreenShake.Instance.Shake(grenadeShake);

    }

    // @IGM ----------------------------------------
    // Handler del eventoi cuando se da un espadazo.
    // ---------------------------------------------
    private void SwordAction_OnAnySwordHit(object sender, EventArgs empty)
    {

        // Instancianciamos el movimiento de la camara
        ScreenShake.Instance.Shake(grenadeShake);

    }

}

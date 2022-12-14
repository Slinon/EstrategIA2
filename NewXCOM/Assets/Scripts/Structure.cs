using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{

    [SerializeField] private HealthSystem healthSystem;               // Sistema de salud de la unidad
    [SerializeField] private ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {

        healthSystem.OnDead += HealthSystem_OnDead;

    }

    // @IGM -------------------------------------
    // Handler del evento cuando muere la unidad.
    // ------------------------------------------
    private void HealthSystem_OnDead(object sender, EventArgs empty)
    {

        // Eliminamos la unidad de la malla
        Destroy(gameObject);

        explosion.Play();

    }

}

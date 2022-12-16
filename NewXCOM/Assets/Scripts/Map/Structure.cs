using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{

    [SerializeField] private HealthSystem healthSystem;               // Sistema de salud de la unidad
    [SerializeField] private ParticleSystem explosion;

    private Unit unit;                                                // Unidad que ha construido esta estructura

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

        // Restamos esta estructura del limite de estructuras de la unidad
        if (unit.TryGetComponent(out BuildStructureAction buildStructureAction))
        {

            buildStructureAction.DeleteStructureCount();

        }

        // Eliminamos la unidad de la malla
        Destroy(gameObject);

        explosion.Play();

    }

    // @IGM ---------------
    // Setter de la unidad.
    // --------------------
    public void SetUnit(Unit unit)
    {

        this.unit = unit;

    }
    
    // @IGM ---------------
    // Getter de la unidad.
    // --------------------
    public Unit GetUnit()
    {

        return unit;

    }


}

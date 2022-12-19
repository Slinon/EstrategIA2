using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseGameOver : MonoBehaviour
{
    HealthSystem healthSystem;
    public static event EventHandler<Unit> OnAnyBaseDestroyed;
    Unit unit;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    //GRG Evento al destruir la base
    public void HealthSystem_OnDead(object sender, EventArgs empty)
    {
        if (OnAnyBaseDestroyed != null)
        {
            OnAnyBaseDestroyed(this, unit);
        }
    }

}

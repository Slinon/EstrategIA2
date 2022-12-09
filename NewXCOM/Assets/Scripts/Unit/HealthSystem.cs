using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDead;                   // Evento cuando la unidad muere
    public event EventHandler OnDamaged;                // Evento cuando se daña a una unidad

    [SerializeField] private int healthMax;             // Salud maxima de la unidad

    private int health;                                 // Salud de la unidad

    private Unit unit;                                  //Referencia al component Unit asociado a este GameObject

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Establecemos la salud a la salud maxima
        health = healthMax;

    }

    //@GRG 
    //Modificaciones para que derrotar una unidad dé dinero:
    //Comprobar si el bool isEnemy = true en su Unit component
    //Llamar a GiveTakeMoney() de Money System
    private void Start()
    {

        unit = GetComponent<Unit>();

    }

    // @IGM -------------------------
    // Metodo para dañar a la unidad.
    // ------------------------------

    public void Damage(int damageAmount)
    {

        // Reducimos la vida
        health -= damageAmount;

        // Comprobamos si la vida es menor a 0
        if (health < 0)
        {

            // Igualamos la vida a 0
            health = 0;

        }

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnDamaged != null)
        {

            // Lanzamos el evento
            OnDamaged(this, EventArgs.Empty);

        }

        // Comprobamos si la unidad no tiene vida
        if (health == 0)
        {

            // Matamos la unidad
            Die();

        }

    }

    // @IGM -------------------------
    // Metodo para matar a la unidad.
    // ------------------------------
    private void Die()
    {

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnDead != null)
        {

            // Lanzamos el evento
            OnDead(this, EventArgs.Empty);

        }

        if (unit.IsEnemy())
        {
            //El jugador recibe dinero por el takedown
            MoneySystem.Instance.GiveTakeMoney(200, MoneySystem.Instance.player);

        }

        else
        {
            //La AI recibe dinero por el takedown
            MoneySystem.Instance.GiveTakeMoney(200, MoneySystem.Instance.enemyAI);

        }
        
    }

    // @IGM -----------------------------------------------
    // Metodo para calcular el porcentaje de vida restante.
    // ----------------------------------------------------
    public float GetHealthNormalized()
    {

        // Calculamos el porcentace de vida restante en decimales
        return (float)health / healthMax;

    }

}

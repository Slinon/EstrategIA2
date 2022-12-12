using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{

    [SerializeField] private Transform crateDestroyedPrefab;    // Prefab del objeto destruido
    public static event EventHandler OnAnyDestroyed;            // Evento cuando cualquier objeto es destruido

    private GridPosition gridPosition;                          // Posicion en la malla del objeto

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Guardamos la posicion en la malla del objeto
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

    }

    // @IGM ---------------------------------------
    // Geter de la posicion en la malla del objeto.
    // --------------------------------------------
    public GridPosition GetGridPosition()
    {

        return gridPosition;

    }

    // @IGM --------------------------
    // Metodo para destruir el objeto.
    // -------------------------------
    public void Damage()
    {

        // Instanciamos el objeto destruido
        Transform crateDestroyedTrasform = Instantiate(crateDestroyedPrefab, 
            transform.position, transform.rotation);

        // Aplicamos la fuerza de la explosion
        ApplyExplosionToChildren(crateDestroyedTrasform, 150f, transform.position, 10f);

        // Destruimos el objeto
        Destroy(gameObject);
        LevelGrid.Instance.SetCoverTypeAtGridPosition(CoverType.None, gridPosition);

        // Comprobamos si hay alguna clase escuchando el evento
        if (OnAnyDestroyed != null)
        {

            // Lanzamos el evento
            OnAnyDestroyed(this, EventArgs.Empty);

        }

    }
    // @IGM -------------------------------------------------
    // Metodo para aplicar una fuerza de explosion al mu�eco.
    // ------------------------------------------------------
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {

        // Recorremos todos los hijos de la raiz
        foreach (Transform child in root)
        {

            // Comprobamos si el hijo tiene rigidbody
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {

                // A�adimos la explosion
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);

            }

            // Llamamos recursivamente para seguir bajando por el arbol
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);

        }

    }

}

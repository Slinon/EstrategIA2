using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private TrailRenderer trailRenderer;   // Renderer de la bala
    [SerializeField] private Transform bulletHitVfxPrefab;  // Prefab del efecto de la bala al golpear a otra unidad

    private Vector3 targetPosition;                         // Posicion del destino de la bala

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {

        // Calculamos la direccion
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // Calculamos la distancia antes de mover la bala
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        // Movemos la bala
        float moveSpeed = 200f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Calculamos la distancia despues de movernos
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        // Comprobamos si la distancia de antes es menor que la de despues
        if (distanceBeforeMoving < distanceAfterMoving)
        {

            // Establacemos la bala en el objetivo
            transform.position = targetPosition;

            // Desvinculamos la bala del padre
            trailRenderer.transform.parent = null;

            // Destruimos el objeto
            Destroy(gameObject);

            // Intanciamos el efecto de golpear
            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);

        }

    }

    // @IGM --------------------------------------------
    // Metodo para establecer los parametros de la bala.
    // -------------------------------------------------
    public void Setup(Vector3 targetPosition)
    {

        // Asignamos el target
        this.targetPosition = targetPosition;

    }

}

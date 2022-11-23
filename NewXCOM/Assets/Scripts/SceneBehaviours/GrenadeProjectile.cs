using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{

    public static event EventHandler OnAnyGrenadeExploded;      // Evento cuando cualquier granada explota

    [SerializeField] private float damageRadius;                // Radio de daño de la granada
    [SerializeField] private int damageAmount;                  // Daño que hace la granada
    [SerializeField] private float maxGrenadeHeight;            // Máxima altura que puede alcanzar la granada
    [SerializeField] private Transform grenadeExplodeVfxPrefab; // Prefab del efecto visual de la explosion
    [SerializeField] private TrailRenderer trailRenderer;       // Render de la estela de la granada
    [SerializeField] private AnimationCurve arcYAnimationCurve; // Animacion de parabola de la granada

    private Vector3 targetPosition;                             // Posicion objetivo
    private Action onGrenadeBehaviourComplete;                  // Accion cuando la granada ha cumplido su funcion
    private float totalDistance;                                // Distancia total entre la unidad y el objetivo
    private Vector3 posicionXZ;                                 // Posicion en el plano de la granada

    // @IGM ------------------------
    // Update is called every frame.
    // -----------------------------
    private void Update()
    {

        // Calculamos la direccion
        Vector3 moveDir = (targetPosition - posicionXZ).normalized;

        // Movemos la granada
        float moveSpeed = 15;
        posicionXZ += moveDir * moveSpeed * Time.deltaTime;

        // Calculamos la distancia actual
        float distance = Vector3.Distance(posicionXZ, targetPosition);

        // Normalizamos e invertimos la distancia
        float distanceNormalized = 1 - distance / totalDistance;

        // Aplicamos la altura de la granada
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * (totalDistance / maxGrenadeHeight);

        // Establecemos la posicion de la bola
        transform.position = new Vector3(posicionXZ.x, positionY, posicionXZ.z);

        // Comprobamos si la granada ha llegado a su destino
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(posicionXZ, targetPosition) < reachedTargetDistance)
        {

            // Comrpobamos si ha dañado a alguna unidad
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            // Recorremos los objetos que ha golpeado la granada
            foreach (Collider collider in colliderArray)
            {

                // Comprobamos si los objetos son una unidad
                if (collider.TryGetComponent<Unit>(out Unit targetUnit)) 
                {

                    // Dañamos a la unidad
                    targetUnit.Damage(damageAmount);

                }

                // Comprobamos si los objetos son objetos destruibles
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {

                    // Dañamos a la unidad
                    destructibleCrate.Damage();

                }

            }

            // Comprobamos si hay alguna clase escuchando el evento
            if (OnAnyGrenadeExploded != null)
            {

                // Lanzamos el evento
                OnAnyGrenadeExploded(this, EventArgs.Empty);

            }

            // Anulamos el padre a la cola
            trailRenderer.transform.parent = null;

            // Instanciamos la explosion
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);

            // La destruimos
            Destroy(gameObject);

            // Llamamos a la accion
            onGrenadeBehaviourComplete();

        }
    }

    // @IGM -----------------------------------------------
    // Metodo para establecer los parametros de la granada.
    // ----------------------------------------------------
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {

        // Asignamos las variables
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;

        posicionXZ = transform.position;
        posicionXZ.y = 0;
        totalDistance = Vector3.Distance(posicionXZ, targetPosition);

    }

    // @IGM ----------------------------------
    // Getter del radio de daño de la granada.
    // ---------------------------------------
    public float GetDamageRadius()
    {

        return damageRadius;

    }

}

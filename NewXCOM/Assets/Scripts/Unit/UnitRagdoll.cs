using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{

    [SerializeField] private Transform ragdollRootBone;         // Posicion del origen del muñeco

    // @IGM --------------------------------------------------------------
    // Metodo para establecer la misma posicion de la unidad en el muñeco.
    // -------------------------------------------------------------------
    public void Setup(Transform originalRootBone)
    {

        // Igualamos los huesos del muñeco
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        // Aplicamos un componente de aletoriedad para la direccion
        Vector3 randomDir = new Vector3(Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f));

        // Aplicamos la explosion
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position + randomDir, 10f);

    }

    // @IGM --------------------------------------------------------
    // Metodo para igualar la posicion del muñeco a la de la unidad.
    // -------------------------------------------------------------
    private void MatchAllChildTransforms(Transform root, Transform clone)
    {

        // Recorremos todos los hijos de la raiz
        foreach (Transform child in root)
        {

            // Buscamos un hijo con el mismo nombre en los dos objetos
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {

                // Copiamos la posicion y rotacion del objeto
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                // Llamamos recursivamente para seguir bajando por el arbol
                MatchAllChildTransforms(child, cloneChild);

            }

        }

    }

    // @IGM -------------------------------------------------
    // Metodo para aplicar una fuerza de explosion al muñeco.
    // ------------------------------------------------------
    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {

        // Recorremos todos los hijos de la raiz
        foreach (Transform child in root)
        {

            // Comprobamos si el hijo tiene rigidbody
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {

                // Añadimos la explosion
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);

            }

            // Llamamos recursivamente para seguir bajando por el arbol
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);

        }

    }
}

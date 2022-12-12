using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    public GameObject floatingTextPrebab;

    [SerializeField] private Transform ragdollPrefab;           // Prefab del muñeco
    [SerializeField] private Transform originalRootBone;        // Posicion del hueso raiz de la unidad

    private HealthSystem healthSystem;                          // Sistema de vida de la unidad

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Asignamos el sistema de salud
        healthSystem = GetComponent<HealthSystem>();

        // Asignamos los eventos
        healthSystem.OnDead += HealthSystem_OnDead;

    }

    // @IGM -------------------------------------
    // Handler del evento cuando muere la unidad.
    // ------------------------------------------
    private void HealthSystem_OnDead(object sender, EventArgs empty)
    {
        ShowFloatingTextMoney(100);

        // Instanciamos el muñeco
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);

        // Asignamos la unidad del muñeco
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();

        // Establecemos la posicion del hueso raiz
        unitRagdoll.Setup(originalRootBone);
    }

    void ShowFloatingTextMoney(int money)
    {
        var go = Instantiate(floatingTextPrebab, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = "+" + money.ToString();
        go.GetComponent<TextMesh>().color = Color.yellow;
    }

}

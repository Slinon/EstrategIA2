using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGraphics : MonoBehaviour
{

    [SerializeField] private MeshRenderer factory;                  // Elemento para saber si hay una unidad en produccion
    [SerializeField] private Material darkMaterial;                 // Material off
    [SerializeField] private Material lightMaterial;                // Material on
    [SerializeField] private Transform spawnVfx;                    // Vfx del spawn de la unidad

    // @IGM ----------------------------------------------------
    // Awake is called when the script instance is being loaded.
    // ---------------------------------------------------------
    private void Awake()
    {

        // Comprobamos si la unidad tiene accion de spawnear unidad
        if (TryGetComponent<SpawnUnitAction>(out SpawnUnitAction spawnUnitAction))
        {

            spawnUnitAction.OnSpawnActionStarted += spawnUnitAction_OnSpawnActionStarted;
            spawnUnitAction.OnSpawnActionCompleted += spawnUnitAction_OnSpawnActionCompleted;
            SpawnUnitAction.OnAnyUnitSpawned += spawnUnitAction_OnAnyUnitSpawned;

        }

    }

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        // Mantenemos la fabrica apagada
        factory.material = darkMaterial;

    }

    // @IGM -------------------------------------------------------------
    // Handler del evento cuando la unidad empieza el spawn de la unidad.
    // ------------------------------------------------------------------
    private void spawnUnitAction_OnSpawnActionStarted(object sender, EventArgs empty)
    {

        // Activamos la fabrica
        factory.material = lightMaterial;

    }

    // @IGM -------------------------------------------------------------
    // Handler del evento cuando la unidad termina el spawn de la unidad.
    // ------------------------------------------------------------------
    private void spawnUnitAction_OnSpawnActionCompleted(object sender, EventArgs empty)
    {

        // Activamos la fabrica
        factory.material = darkMaterial;

    }

    // @IGM -------------------------------------------------
    // Handler del evento cuando la unidad spawnea la unidad.
    // ------------------------------------------------------
    private void spawnUnitAction_OnAnyUnitSpawned(object sender, Vector3 spawnPoint)
    {

        // Spawneamos el efecto del spawn
        Instantiate(spawnVfx, spawnPoint, Quaternion.identity);

    }

}

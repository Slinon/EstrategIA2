using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUptader : MonoBehaviour
{

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    void Start()
    {

        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;

    }

    // @IGM ---------------------------------------------
    // Handler del eventoi cuando un objeto es destruido.
    // --------------------------------------------------
    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs empty)
    {

        // Actualizamos el pathfinding
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);

    }

}

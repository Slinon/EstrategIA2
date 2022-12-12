using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    void Start()
    {
        //UnitManager.Instance.OnAnyUnitMovedGridPosition += UnitManager_OnAnyUnitMovedGridPosition;
    }

    private void UnitManager_OnAnyUnitMovedGridPosition(object sender, System.EventArgs e)
    {
        UpdateAllFogOfWar();
    }

    void UpdateAllFogOfWar()
    {
        
    }
}

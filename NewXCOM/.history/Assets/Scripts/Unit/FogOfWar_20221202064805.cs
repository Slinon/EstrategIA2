using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    // Start is called before the first frame update
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

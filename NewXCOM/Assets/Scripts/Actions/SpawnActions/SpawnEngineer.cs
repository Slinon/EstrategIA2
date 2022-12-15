using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEngineer: SpawnUnitAction
{

    public override string GetActionName()
    {
        return "Spawn Enginieer";
    }

    public override int MoneyCost()
    {
        return unitCost;
    }


}
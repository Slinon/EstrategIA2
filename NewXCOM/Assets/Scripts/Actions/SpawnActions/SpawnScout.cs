using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScout : SpawnUnitAction
{

    public override string GetActionName()
    {
        return "Spawn Scout";
    }

    public override int MoneyCost()
    {
        return unitCost;
    }


}
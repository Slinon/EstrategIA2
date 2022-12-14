using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBerserk : SpawnUnitAction
{

    public override string GetActionName()
    {
        return "Spawn Berserk";
    }

    public override int MoneyCost()
    {
        return unitCost;
    }


}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSoldier : SpawnUnitAction
{

    public override string GetActionName()
    {
        return "Spawn Soldier";
    }

    public override int MoneyCost()
    {
        return unitCost;
    }


}
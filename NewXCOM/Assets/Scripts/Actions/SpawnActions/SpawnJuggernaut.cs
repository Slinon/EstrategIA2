using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJuggernaut : SpawnUnitAction
{

    public override string GetActionName()
    {
        return "Spawn Juggernaut";
    }

    public override int MoneyCost()
    {
        return unitCost;
    }


}
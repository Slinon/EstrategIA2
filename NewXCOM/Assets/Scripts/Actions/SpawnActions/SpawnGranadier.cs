using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGranadier : SpawnUnitAction
{

    public override string GetActionName()
    {
        return "Spawn Granadier";
    }

    public override int MoneyCost()
    {
        return unitCost;
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarToggle : MonoBehaviour
{
    public void activeFogOfWar(bool active)
    {
        if(!active)
        {
            FogOfWar.Instance.unableFogOfWar();
        }
        else
        {
            
        }
        
    }
}

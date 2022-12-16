using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarToggle : MonoBehaviour
{
    public void setActiveFogOfWar(bool active)
    {
        if(!active)
        {
            FogOfWar.Instance.unableFogOfWar();
            Debug.Log("Desactivando fog of war...");
        }
        else
        {
            
            Debug.Log("Activando fog of war...");
        }
        
    }
}

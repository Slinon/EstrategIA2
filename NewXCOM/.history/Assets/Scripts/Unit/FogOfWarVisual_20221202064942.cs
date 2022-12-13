using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisual : MonoBehaviour
{
    public static FogOfWarVisual Instance {get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

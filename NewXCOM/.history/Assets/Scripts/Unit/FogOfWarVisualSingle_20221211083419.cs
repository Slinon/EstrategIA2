using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisualSingle : MonoBehaviour
{

    public void Show(Material material)
    {

        meshRenderer.enabled = true;
        meshRenderer.material = material;

    }
}

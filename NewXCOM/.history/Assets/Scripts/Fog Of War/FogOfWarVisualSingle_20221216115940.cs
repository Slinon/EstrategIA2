using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer; 

    public void Show(Material material)
    {
        meshRenderer.material = material;

    }

    public void hide()
    {
        meshRenderer.enabled = false;
    }
}

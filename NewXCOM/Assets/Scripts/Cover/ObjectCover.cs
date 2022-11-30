using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCover : MonoBehaviour
{
    [SerializeField] private CoverType coverType;

    public CoverType GetCoverType()
    {
        return coverType;
    }

}

public enum CoverType{None,Covered}
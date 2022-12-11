using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverObject : MonoBehaviour
{
     
    [SerializeField] private CoverType coverType;
        
        
    private GridPosition gridPosition;

    private void Start()
    {
        
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetCoverTypeAtGridPosition(coverType, gridPosition);
    }

    public CoverType GetCoverType()
    {
        return coverType;
    }
    
}

public enum CoverType {
    None,
    Covered
}
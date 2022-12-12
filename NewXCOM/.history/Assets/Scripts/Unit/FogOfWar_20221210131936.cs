using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @VJT 
public class FogOfWar : MonoBehaviour
{
    public static FogOfWar Instance;

    private void Awake() {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    void Start()
    {
        UnitManager.OnAnyUnitMovedGridPosition += UnitManager_OnAnyUnitMovedGridPosition;
        UpdateAllFogOfWar();
    }

    private void UnitManager_OnAnyUnitMovedGridPosition(object sender, System.EventArgs e)
    {
        // Updating 
        UpdateAllFogOfWar();
    }

    void UpdateAllFogOfWar()
    {
        Debug.Log("Fog of war visual: " + FogOfWarVisual.Instance != null);
        FogOfWarVisual.Instance.HideAllGridPositions();
        //FogOfWarVisual.idk();

        List<Vector2Int> revealedGridPositionList = new List<Vector2Int>();

        foreach(Unit unit in UnitManager.Instance.GetFriendlyUnitList())
        {
            Vector2Int unitGridPosition = unit.GetGridPosition();
            Vector3 unitWorldPosition = unit.GetWorldPosition();

            revealedGridPositionList.Add(unitGridPosition);

            Vector3 baseDir = new Vector3(1, 0, 0);
            float angleIncrease = 10;
            for(float angle = 0; angle < 360; angle += angleIncrease)
            {
                Vector3 dir = ApplyRotationToVectorXZ(baseDir, angle);
                //Debug.DrawLine(Vector3.zero, dir, Color.green, 100f);

                float viewDistanceMax = 5f;
                float viewDistanceIncrease = .1f;
                for(float viewDistance = 0f; viewDistance < viewDistanceMax; viewDistance += viewDistanceIncrease)
                {
                    Vector3 targetPosition = unitWorldPosition + dir * viewDistance;
                    Vector2Int targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
                    if(LevelGrid.Instance.IsValidGridPosition(targetGridPosition))
                    {
                        //CoverType coverType = LevelGrid.Instance.GetCoverTypeAtPosition(targetPosition);
                        //if(coverType == CoverType.None || coverType == coverType.Half)
                        //{
                            if(!revealedGridPositionList.Contains(targetGridPosition))
                            {
                                // Position not yet tested
                                revealedGridPositionList.Add(targetGridPosition);
                            }                            
                        //}
                    }
                }
            }
        }

        FogOfWarVisual.Instance.ShowGridPosition(revealedGridPositionList);
        //FogOfWarVisual.Instance.ShowEverythinglol(revealedGridPositionList);
    }

    // copied from Code Monkey Utilities
    public Vector3 ApplyRotationToVectorXZ(Vector3 vec, float angle) {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
}

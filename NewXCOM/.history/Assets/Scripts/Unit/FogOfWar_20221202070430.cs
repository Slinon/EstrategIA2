using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    void Start()
    {
        //UnitManager.Instance.OnAnyUnitMovedGridPosition += UnitManager_OnAnyUnitMovedGridPosition;
    }

    private void UnitManager_OnAnyUnitMovedGridPosition(object sender, System.EventArgs e)
    {
        UpdateAllFogOfWar();
    }

    void UpdateAllFogOfWar()
    {
        FogOfWarVisual.Instance.HideAllGridPositions();

        List<Vector2Int> revealedGridPositionList = new List<Vector2Int>();

        foreach(Unit unit in UnitManager.Instance.GetFriendlyUnitList())
        {
            Vector2Int unitGridPosition = unit.GetGridPosition();
            Vector3 unitWorldPosition = uint.GetPosition();

            revealedGridPositionList.Add(unitGridPosition);

            Vector3 baseDir = new Vector3(1, 0, 0);
            float angleIncrease = 10;
            for(float angle = 0; angle < 360; angle += angleIncrease)
            {
                Vector3 dir = ApplyRotationVectorXZ(baseDir, angle);
                //Debug.DrawLine(Vector3.zero, dir, Color.green, 100f);

                float viewDistanceMax = 5f;
                float viewDistanceIncrease = .1f;
                for(float viewDistance = 0f; viewDistance < viewDistanceMax; viewDistance += viewDistanceIncrease)
                {
                    Vector3 targetPosition = unitWorldPosition + dir * viewDistance;
                    Vector2Int targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);
                    if(LevelGrid.Instance.IsValidGridPosition(targetGridPosition))
                    {
                        LevelGrid.Instance.GetCoverTypeAtPosition(targetPosition);
                        if(!revealedGridPositionList.Contains(targetGridPosition))
                        {
                            revealedGridPositionList.Add(targetGridPosition)
                        }
                    }
                }
            }
        }

        FogOfWarVisual.Instance.ShowGridPosition(revealedGridPositionList);
    }

     private Vector3 ApplyRotationVectorXZ(Vector3 baseDir, float angle)
    {
        return Vector3.forward;
    }
}

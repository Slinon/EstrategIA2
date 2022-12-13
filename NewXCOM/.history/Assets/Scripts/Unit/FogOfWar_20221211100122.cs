using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @VJT 
public class FogOfWar : MonoBehaviour
{
    public static FogOfWar Instance;
    [Range (1, 5)] [SerializeField] private float viewDistanceMax = 1f;

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
        // Updating fog of war
        UpdateAllFogOfWar();
    }

    void UpdateAllFogOfWar()
    {

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

                // Detectar colisiones con paredes
            if(Physics.Raycast(origin, direction, out RaycastHit hit, viewDistance, layerMask) /*raycastHit2D.collider != null*/)
            {
                
                // Hit object
                vertex = transform.InverseTransformPoint(hit.point);
                //vertex = raycastHit2D.point;
            }
            else
            {
                // No hit
                vertex = direction * viewDistance;
            }

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

        FogOfWarVisual.Instance.ShowForOfWar(revealedGridPositionList);
    }

    // copied from Code Monkey Utilities
    public Vector3 ApplyRotationToVectorXZ(Vector3 vec, float angle) {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
}

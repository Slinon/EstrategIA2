using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @VJT 
public class FogOfWar : MonoBehaviour
{
    public static FogOfWar Instance { get; private set; }

    [Range (1, 5)] [SerializeField] private float viewDistanceMax = 5f;
    [SerializeField] private LayerMask layerMask;

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
        enableFogOfWar();
        UpdateAllFogOfWar();
    }

    public void enableFogOfWar()
    {
        UnitManager.OnAnyUnitMovedGridPosition += UnitManager_OnAnyUnitMovedGridPosition;
        Unit.OnAnyUnitDied += UnitManager_OnAnyUnitMovedGridPosition;
    }

    public void unableFogOfWar()
    {
        UnitManager.OnAnyUnitMovedGridPosition -= UnitManager_OnAnyUnitMovedGridPosition;
        Unit.OnAnyUnitDied -= UnitManager_OnAnyUnitMovedGridPosition; 
    }

    private void UnitManager_OnAnyUnitMovedGridPosition(object sender, System.EventArgs e)
    {
        // Updating fog of war
        UpdateAllFogOfWar();
    }

    public void UpdateAllFogOfWar()
    {

        List<Vector2Int> revealedGridPositionList = new List<Vector2Int>();

        foreach(Unit unit in UnitManager.Instance.GetFriendlyUnitList())
        {
            Vector2Int unitGridPosition = unit.GetGridPosition();
            Vector3 unitWorldPosition = unit.GetWorldPosition();

            revealedGridPositionList.Add(unitGridPosition);

            // Cambiamos distancia máxima según el rango de disparo
            float viewDistanceShootingRange = 0;
            if(unit.gameObject.TryGetComponent<ShootAction>(out ShootAction shootAction))
            {
                viewDistanceShootingRange = shootAction.GetMaxShootDistance() * LevelGrid.Instance.GetCellSize();
            }
            
            Vector3 baseDir = new Vector3(1, 0, 0);
            float angleIncrease = 10;
            for(float angle = 0; angle < 360; angle += angleIncrease)
            {
                Vector3 dir = ApplyRotationToVectorXZ(baseDir, angle);
                //Debug.DrawLine(Vector3.zero, dir, Color.green, 100f);

                // Si la unidad tiene rango de disparo, se usa eso como distancia. Si no, se usa la que está por defecto
                float viewDistanceCollision = viewDistanceShootingRange == 0? viewDistanceMax : viewDistanceShootingRange;

                // Detectamos colisiones con paredes
                //Debug.DrawRay(unitWorldPosition, dir * viewDistanceCollision, Color.green);
                if(Physics.Raycast(unitWorldPosition, dir, out RaycastHit hit, viewDistanceCollision, layerMask))
                {
                    // Hit object
                    viewDistanceCollision = Vector3.Distance(hit.point, unitWorldPosition);
                }

                float viewDistanceIncrease = .1f;
                for(float viewDistance = 0f; viewDistance < viewDistanceCollision; viewDistance += viewDistanceIncrease)
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

        // Asignamos el material correspondiente
        FogOfWarVisual.Instance.ShowForOfWar(revealedGridPositionList);

        // Dehabilitamos el meshRender de los enemigos fuera de rango
        UnitManager.Instance.hideOrShowEnemies(revealedGridPositionList);
    }

    // copied from Code Monkey Utilities
    public Vector3 ApplyRotationToVectorXZ(Vector3 vec, float angle) {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
}

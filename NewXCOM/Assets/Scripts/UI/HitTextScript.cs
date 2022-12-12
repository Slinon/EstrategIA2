using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitTextScript : MonoBehaviour
{
    [SerializeField] private Unit unit;

    [SerializeField] private TextMeshProUGUI hitProbabilityText;

    private UnitActionSystem unitActionSystem;
    private ProbabilitySystem ps;
    private Pathfinding pathFinding;

    private Unit unitSelected;
    private ShootAction unitSelectedShootAction;

    BaseAction selectedAction;

    private UnitManager unitManager;

    private List<Unit> enemyList;

    private LevelGrid levelGrid;


    void Start()
    {
        unitActionSystem = UnitActionSystem.Instance;
        ps = ProbabilitySystem.Instance;
        pathFinding = Pathfinding.Instance;
        unitManager = UnitManager.Instance;
        levelGrid = LevelGrid.Instance;

        unitSelected = unitActionSystem.GetSelectedUnit();
        unitSelectedShootAction = unitSelected.GetComponent<ShootAction>();
    }
    void Update()
    {
        enemyList = unitManager.GetEnemyUnitList();
        selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        unitSelected = unitActionSystem.GetSelectedUnit();
        unitSelectedShootAction = unitSelected.GetComponent<ShootAction>();


        switch (selectedAction)
        {
            // Accion de disparo
            case ShootAction shootAction:
                ShowAllProbabiltyTexts(enemyList, true);

                UpdateHitProbabilityText();
                break;

            default:
                ShowAllProbabiltyTexts(enemyList, false);
                break;
        }
        //Debug.Log(GetDistance((int)(unitSelected.GetWorldPosition().x / levelGrid.GetCellSize()), (int)(unitSelected.GetWorldPosition().z / levelGrid.GetCellSize())));
    }

    
    private void UpdateHitProbabilityText()
    {

        // Actualizamos el texto
        hitProbabilityText.text = GetProbabilityHit().ToString() + " %";

    }

    public int GetProbabilityHit()
    {
        return ps.GetProbabiltyByDistance(unitSelectedShootAction.GetShootHitProbability(), GetDistance((int)(unitSelected.GetWorldPosition().x / levelGrid.GetCellSize()), (int)(unitSelected.GetWorldPosition().z / levelGrid.GetCellSize())), unitSelectedShootAction.GetMaxShootDistance());
    }

    private void ShowAllProbabiltyTexts(List<Unit> enemyList, bool show)
    {

        foreach (Unit unit in enemyList)
        {
            unit.GetComponentInChildren<HitTextScript>().hitProbabilityText.gameObject.SetActive(show);
        }
    }
 
    private int GetDistance(int x, int z)
    {
        return Mathf.Abs(x) + Mathf.Abs(z);
    }
}

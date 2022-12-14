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
    }
    
    private void UpdateHitProbabilityText()
    {

        // Actualizamos el texto
        hitProbabilityText.text = GetProbabilityHit().ToString() + " %";

    }

    public int GetProbabilityHit()
    {
        int probability = 0;
        // Comprobar si esta cubierto para devolver 0
        if (this.unit.GetCoverType() == CoverType.None)
        {
            probability = ps.GetProbabiltyByDistance(unitSelectedShootAction.GetShootHitProbability(), unit.GetDistanceBetweenUnits(unitSelected, this.unit), unitSelectedShootAction.GetMaxShootDistance());
        }
        return probability;
    }

    private void ShowAllProbabiltyTexts(List<Unit> enemyList, bool show)
    {

        foreach (Unit unit in enemyList)
        {
            unit.GetComponentInChildren<HitTextScript>().hitProbabilityText.gameObject.SetActive(show);
        }
    }
    
}

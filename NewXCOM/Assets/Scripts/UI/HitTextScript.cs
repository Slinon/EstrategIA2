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

    private GameObject[] allTexts;


    // Start is called before the first frame update
    void Start()
    {
        
        unitActionSystem = UnitActionSystem.Instance;
        ps = ProbabilitySystem.Instance;
        pathFinding = Pathfinding.Instance;

        unitSelected = unitActionSystem.GetSelectedUnit();
        unitSelectedShootAction = unitSelected.GetComponent<ShootAction>();
        
        allTexts = GetAllProbabilityTexts();
    }

    // Update is called once per frame
    void Update()
    {
        selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        unitSelected = unitActionSystem.GetSelectedUnit();
        unitSelectedShootAction = unitSelected.GetComponent<ShootAction>();


        switch (selectedAction)
        {
            // Accion de disparo
            case ShootAction shootAction:
                ShowAllProbabiltyTexts(allTexts, true);

                //Debug.Log(ps.CalculateDistanceUnit(unitSelected, unit));
                UpdateHitProbabilityText();
                break;

            default:
                ShowAllProbabiltyTexts(allTexts, false);
                break;
        }

        if (unit.name == "UnitScoutEnemy")
        {
            Debug.Log("Enemy  " + unit.name + " at distance: " + pathFinding.CalculateDistance(unitSelected.GetGridPosition(), unit.GetGridPosition()) / 10 );
        }
        
    }

    private void UpdateHitProbabilityText()
    {

        // Actualizamos el texto
        hitProbabilityText.text = GetProbabilityHit().ToString() + " %";

    }

    public int GetProbabilityHit()
    {
        return ps.GetProbabiltyByDistance(unitSelectedShootAction.GetShootHitProbability(), pathFinding.CalculateDistance(unitSelected.GetGridPosition(), unit.GetGridPosition()) / 10, unitSelectedShootAction.GetMaxShootDistance());
    }

    private void ShowAllProbabiltyTexts(GameObject[] allTexts, bool show)
    {
        foreach (GameObject text in allTexts)
        {
            text.SetActive(show);
        }
    }

    private GameObject[] GetAllProbabilityTexts()
    {
        return GameObject.FindGameObjectsWithTag("ProbabilityText");
    }
}

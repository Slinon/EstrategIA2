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

    private Unit unitSelected;
    private ShootAction unitSelectedShootAction;

    BaseAction selectedAction;

    private GameObject[] allTexts;


    // Start is called before the first frame update
    void Start()
    {

        if (unit.IsEnemy())
        {
            //UnitActionSystem.Instance.OnSelectedActionChanged += HitTextScript_OnSelectedActionChanged;
        }
        
        unitActionSystem = UnitActionSystem.Instance;
        ps = ProbabilitySystem.Instance;

        unitSelected = unitActionSystem.GetSelectedUnit();
        unitSelectedShootAction = unitSelected.GetComponent<ShootAction>();

       allTexts = GetAllProbabilityTexts();
    }

    // Update is called once per frame
    void Update()
    {
        selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        switch (selectedAction)
        {
            // Accion de disparo
            case ShootAction shootAction:
                ShowAllProbabiltyTexts(allTexts, true);

                unitSelected = unitActionSystem.GetSelectedUnit();
                unitSelectedShootAction = unitSelected.GetComponent<ShootAction>();

                //Debug.Log(ps.CalculateDistanceUnit(unitSelected, unit));
                UpdateHitProbabilityText();
                break;

            default:
                ShowAllProbabiltyTexts(allTexts, false);
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
        return ps.GetProbabiltyByDistance(unitSelectedShootAction.GetShootHitProbability(), ps.CalculateDistanceUnit(unitSelected, unit), unitSelectedShootAction.GetMaxShootDistance());
    }

    private void HitTextScript_OnSelectedActionChanged(object sender, EventArgs empty)
    {

        // Actualizamos la vista de la malla
        UpdateHitProbabilityText();

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

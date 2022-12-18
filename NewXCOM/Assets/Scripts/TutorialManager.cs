using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TutorialManager : MonoBehaviour
{
    [Header("Window")]
    [SerializeField] private GameObject tutorialWindow;
    [Space(10)]

    [Header("Arrow")]
    [SerializeField] private GameObject tutorialArrow;
    [Space(10)]

    [Header("Tasks")]
    [SerializeField] private GameObject tutorialTaskPanel;
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] [TextArea] private string[] taskContent;
    private int taskIndex =-1;

    InteractSphere sphere;

    bool alreadyDone;


    void Start()
    {
        sphere = FindObjectOfType<InteractSphere>();

        sphere.OnSphereCaptured += Sphere_OnSphereCaptured;

        tutorialWindow.SetActive(true);
    }


    private void Update()
    {
        if (TurnSystem.Instance.GetTurnNumber() == 3 && !alreadyDone)
        {
            //Evitamos que entre más veces (paso de hacer un evento en el turn system)
            alreadyDone = true;

            MoveArrow(0, 0, 16);
            SetActiveTaskPanel(true);
        }

        else return;
    }

    public void ActiveNextModalWindow()
    {
        //Acceder al indice de la ventana y sumar uno
        tutorialWindow.GetComponent<TutorialWindowSystem>().windowIndex += 1;

        //Actualizar contenido de la ventana
        tutorialWindow.GetComponent<TutorialWindowSystem>().UpdateWindow();

        //Activar ventana
        tutorialWindow.SetActive(true);
    }

    public void MoveArrow(int x, int y, int z)
    {
        Debug.Log("Moviendo flecha" + x + z);
        tutorialArrow.transform.position += new Vector3(x, y, z);
    }

    public void SetActiveTaskPanel(bool b)
    {
        if (b) taskIndex += 1;

        taskText.text = taskContent[taskIndex];
        tutorialTaskPanel.SetActive(b);
    }

    public void Sphere_OnSphereCaptured(object sender, EventArgs empty)
    {
        SetActiveTaskPanel(false);
        ActiveNextModalWindow();
        MoveArrow(0, 3, -8);
    }

}

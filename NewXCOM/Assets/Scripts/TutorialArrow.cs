using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag is "TutorialUnit")
        {
            Invoke("ActiveWindow", 0.7f);
            
        }
    }

    void ActiveWindow()
    {
        //Activamos la ventana
        tutorialManager.ActiveNextModalWindow();

        //Desplazamos la flecha a una nueva posici?n
        tutorialManager.MoveArrow(-2, 0, 2);

        //Apagamos el panel de tasks
        tutorialManager.SetActiveTaskPanel(false);
    }
}

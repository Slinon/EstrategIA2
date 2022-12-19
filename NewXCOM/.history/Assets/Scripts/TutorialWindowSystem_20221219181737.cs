using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialWindowSystem : MonoBehaviour
{
    [Header("WindowComponent")]
    [SerializeField] TextMeshProUGUI windowTitle;
    [SerializeField] TextMeshProUGUI windowFlavorText;
    [SerializeField] Image windowImage;
    [Space(10)]

    [Header("WindowButtons")]
    [SerializeField] Button backButton;
    [SerializeField] TextMeshProUGUI continueButtonText;
    [Space(10)]

    [Header("WindowContent")]
    [SerializeField] private ScriptableWindow[] modalWindows;

    public int windowIndex;
    private int pageIndex;
    TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();

        windowIndex = 0;
        pageIndex = 0;

        UpdateWindow();
    }


    //@GRG Actualizar datos de la ventana modal
    public void UpdateWindow()
    {
        //Usamos stringbuilder para evitar garbage collection.
        windowTitle.text = modalWindows[windowIndex].
            page[pageIndex].title.ToUpper().ToString();

        windowFlavorText.text = modalWindows[windowIndex].
            page[pageIndex].flavorText.ToString();

        windowImage.sprite = modalWindows[windowIndex].
            page[pageIndex].image;

        //Apagar back button en primera p�gina
        if (pageIndex == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        else
        {
            backButton.gameObject.SetActive(true);
        }

        //Cambiar continue a close en la ultima p�gina
        if (pageIndex == modalWindows[windowIndex].page.Length - 1)
        {
            continueButtonText.text = "CLOSE".ToString();
        }

        else
        {
            continueButtonText.text = "CONTINUE".ToString();
        }

    }

    //@GRG m�todo para BACK button, cargar la ventana anterior.
    public void LoadLastPage()
    {
        if (pageIndex - 1 < 0)
        {
            return;
        }

        else
        {
            pageIndex -= 1;
            UpdateWindow();
        }
    }

    //@GRG m�todo para CONTINUE button, cargar la siguiente ventana.
    public void LoadNextPage()
    {
        //Si la no hay m�s p�ginas
        if (pageIndex + 1 == modalWindows[windowIndex].page.Length)
        {
            pageIndex = 0;

            //Apagamos la ventana
            gameObject.SetActive(false);

            //Activamos la task
            tutorialManager.SetActiveTaskPanel(true);

        }

        else
        {
            pageIndex+= 1;
            UpdateWindow();
        }
    }
}

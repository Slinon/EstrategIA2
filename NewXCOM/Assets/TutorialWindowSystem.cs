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

    private int windowIndex;
    private int pageIndex;

    private void Start()
    {
        windowIndex = 0;
        pageIndex = 0;

        UpdateWindow();
    }


    //@GRG Actualizar datos de la ventana modal
    private void UpdateWindow()
    {
        //Usamos stringbuilder para evitar garbage collection.
        windowTitle.text = modalWindows[windowIndex].
            page[pageIndex].title.ToUpper().ToString();

        windowFlavorText.text = modalWindows[windowIndex].
            page[pageIndex].flavorText.ToString();

        windowImage = modalWindows[windowIndex].
            page[pageIndex].image;

        //Apagar back button en primera página
        if (pageIndex == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        else
        {
            backButton.gameObject.SetActive(true);
        }

        //Cambiar continue a close en la ultima página
        if (pageIndex == modalWindows[windowIndex].page.Length - 1)
        {
            continueButtonText.text = "CLOSE".ToString();
        }

        else
        {
            continueButtonText.text = "CONTINUE".ToString();
        }

    }

    //@GRG método para BACK button, cargar la ventana anterior.
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

    //@GRG método para CONTINUE button, cargar la siguiente ventana.
    public void LoadNextPage()
    {
        if (pageIndex + 1 == modalWindows[windowIndex].page.Length)
        {
            Debug.Log("Se cierra la ventana xd");

            windowIndex += 1;
            pageIndex = 0;

            if (windowIndex >= modalWindows.Length)
            {
                Destroy(gameObject);
            }

            else
            {
                UpdateWindow();

            }
        }

        else
        {
            pageIndex+= 1;
            UpdateWindow();
        }
    }
}

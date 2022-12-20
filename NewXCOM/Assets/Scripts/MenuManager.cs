using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static string seed;
    [SerializeField] private TMP_InputField inputField;

    //Quit application
    public void QuitGame()
    {
        Application.Quit();
    }

    //load scene by name
    public void LoadScene(string scene)
    {
        if (scene == "Tutorial")
        {
            seed = "tutorial";
        }

        SceneManager.LoadScene(scene);
    }

    //Get input field value and set it as seed
    public void SetSeed()
    {
        seed = inputField.text;
    }
}

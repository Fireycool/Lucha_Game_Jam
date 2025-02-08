using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Cambiar el número de la escena a la que se quiere ir
        SceneManager.LoadSceneAsync("Tests");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

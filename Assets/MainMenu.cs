using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Cambiar el n�mero de la escena a la que se quiere ir
        SceneManager.LoadSceneAsync("Infinite");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

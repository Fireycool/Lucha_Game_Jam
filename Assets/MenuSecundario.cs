using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSecundario : MonoBehaviour
{
    private bool Pausa = false;

    public void PausarJuego()
    {
        if (!Pausa)
        {
            Time.timeScale = 0f;
            Pausa = true;
        }
    }

    public void ReanudarJuego()
    {
        if (Pausa)
        {
            Time.timeScale = 1f;
            Pausa = false;
        }
    }
    public void salirJuego()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}

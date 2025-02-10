using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSecundario : MonoBehaviour
{
    // pausa tambien se activa al hacer clic en esc

    private bool Pausa = false;
    public GameObject MenuSecundarioUI;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Pausa)
        {
            Time.timeScale = 0f;
            Pausa = true;
            MenuSecundarioUI.SetActive(true);
        }

        else if(Input.GetKeyDown(KeyCode.Escape) && Pausa)
        {
            Time.timeScale = 1f;
            Pausa = false;
            MenuSecundarioUI.SetActive(false);
        }
    }

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
        Time.timeScale = 1f;
        Pausa = false;
        SceneManager.LoadSceneAsync("0");
    }
}

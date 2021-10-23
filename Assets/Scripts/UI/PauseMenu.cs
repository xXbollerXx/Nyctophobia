using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;

    [SerializeField] private bool isPaused;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused; 
        }

        if (isPaused)
        {
            ActivatePause();
        } else 
        {
            DeactivatePause();
        }
    }

    void ActivatePause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        PauseMenuUI.SetActive(true);
    }

    void DeactivatePause()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        PauseMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

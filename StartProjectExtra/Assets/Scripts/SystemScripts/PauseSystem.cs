using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseSystem : MonoBehaviour
{
    public static PauseSystem instance;

    [SerializeField] GameObject pauseMenu;

    private void Awake()
    {
        instance = this;
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        TimeManager.instance.PauseGame();
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        TimeManager.instance.ResumeGame();
        pauseMenu.SetActive(false);
    }

    public void TriggerPauseButton()
    {
        if (TimeManager.instance.IsPaused())
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneSystem.instance.ReloadSceneWithTransition();
    }

    public void LoadMainMenu()
    {
        SceneSystem.instance.LoadSceneByIndexWithTransition(0);
    }

}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI winnerText;


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Activate(string winnerTitle = "better ")
    {
        TimeManager.instance.PauseGame();
        gameObject.SetActive(true);
        winnerText.text = $"{winnerTitle} won!";
    }

    public void OnRestartButtonPressed()
    {
        SceneSystem.instance.ReloadSceneWithTransition();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

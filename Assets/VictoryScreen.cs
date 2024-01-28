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

    public void Activate(string winnerTitle, Color winnerColor)
    {
        TimeManager.instance.PauseGame();
        gameObject.SetActive(true);
        winnerText.color = winnerColor;
        winnerText.text = $"Player {winnerTitle} won!";
    }

    public void OnRestartButtonPressed()
    {
        SceneSystem.instance.ReloadSceneWithTransition();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

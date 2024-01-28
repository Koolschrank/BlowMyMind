using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private SkinnedMeshRenderer winnerModelRenderer;
    [SerializeField] private GameObject winnerModelObject;

    private bool isTransition = false;
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Activate(string winnerTitle, Color winnerColor, SkinnedMeshRenderer playerMeshRenderer)
    {
        TimeManager.instance.PauseGame();
        gameObject.SetActive(true);
        winnerText.color = winnerColor;
        winnerText.text = $"Player {winnerTitle} won!";
        var winnerMaterials = playerMeshRenderer.materials;
        winnerModelRenderer.materials = winnerMaterials;
        winnerModelObject.SetActive(true);
    }
    
    public void OnRestartButtonPressed()
    {
        if (isTransition)
            return;
        SceneSystem.instance.ReloadSceneWithTransition();
        isTransition = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

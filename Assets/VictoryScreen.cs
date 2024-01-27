using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    
    public void Show(string winnerTitle = "better ")
    {
        winnerText.text = $"The {winnerTitle} Player won!";
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

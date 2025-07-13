using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPopup; // You still use this

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // Reset in case of retry
    }

    public void TriggerWin()
    {
        Debug.Log("🎉 Level Complete!");

        // Use the proper WinUI script to show the win canvas
        WinUI winUI = FindObjectOfType<WinUI>();
        if (winUI != null)
        {
            winUI.ShowWin();
        }
        else
        {
            Debug.LogWarning("GameManager: No WinUI script found in scene!");
        }
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        if (gameOverPopup != null) gameOverPopup.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Update this to your actual menu scene name
    }
}

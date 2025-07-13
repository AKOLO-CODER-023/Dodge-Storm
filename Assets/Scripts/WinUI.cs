using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class WinUI : MonoBehaviour
{
    [Header("Win Panel & Audio")]
    public GameObject winPanel;
    public AudioSource winAudioSource;
    public AudioSource buttonAudioSource;
    public AudioClip winSound;
    public AudioClip buttonClickSound;

    private bool hasShown = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
            canvasGroup = winPanel.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                Debug.Log("WinUI: CanvasGroup found and reset.");
            }
            else
            {
                Debug.LogWarning("WinUI: CanvasGroup not found on winPanel.");
            }

            if (EventSystem.current != null)
            {
                EventSystem.current.sendNavigationEvents = true;
            }

            Debug.Log("WinUI: Win panel hidden at start.");
        }
        else
        {
            Debug.LogWarning("WinUI: winPanel is not assigned in the Inspector!");
        }
    }

    public void ShowWin()
    {
        if (hasShown)
        {
            Debug.LogWarning("WinUI: ShowWin called but panel was already shown.");
            return;
        }

        hasShown = true;
        Debug.Log("WinUI: ShowWin() triggered.");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("WinUI: winPanel.SetActive(true) called.");
        }
        else
        {
            Debug.LogError("WinUI: winPanel is null!");
            return;
        }

        StopBackgroundMusic();

        if (winAudioSource != null && winSound != null)
        {
            winAudioSource.PlayOneShot(winSound);
            Debug.Log("WinUI: Win sound played.");
        }
        else
        {
            Debug.LogWarning("WinUI: Missing winAudioSource or winSound.");
        }

        StartCoroutine(FadeInWinPanel());
    }

    private IEnumerator FadeInWinPanel()
    {
        Debug.Log("WinUI: Starting FadeInWinPanel coroutine...");

        if (canvasGroup == null)
        {
            canvasGroup = winPanel.GetComponent<CanvasGroup>();
        }

        if (canvasGroup == null)
        {
            Debug.LogError("WinUI: CanvasGroup missing on winPanel.");
            yield break;
        }

        float duration = 1.0f;
        float t = 0f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        Debug.Log("WinUI: Fade-in complete.");

        yield return new WaitForSecondsRealtime(0.5f); // wait a bit to ensure UI is ready

        if (EventSystem.current != null)
        {
            EventSystem.current.sendNavigationEvents = true;
        }

        Time.timeScale = 0f; // now safe to pause
        Debug.Log("WinUI: Game paused (Time.timeScale = 0).");
    }

    public void RetryLevel()
    {
        Debug.Log("WinUI: RetryLevel() button clicked.");
        PlayClickSound();
        ResumeAndLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Debug.Log("WinUI: NextLevel() button clicked.");
        PlayClickSound();

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            ResumeAndLoad(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("WinUI: No next scene in build settings.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("WinUI: QuitGame() button clicked.");
        PlayClickSound();
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ResumeAndLoad(int sceneIndex)
    {
        Debug.Log("WinUI: Loading scene index: " + sceneIndex);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    private void PlayClickSound()
    {
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
            Debug.Log("WinUI: Button click sound played.");
        }
        else
        {
            Debug.LogWarning("WinUI: Missing buttonAudioSource or buttonClickSound.");
        }
    }

    private void StopBackgroundMusic()
    {
        Debug.Log("WinUI: Trying to stop background music...");
        MenuMusicPlayer music = FindObjectOfType<MenuMusicPlayer>();
        if (music != null)
        {
            AudioSource bgSource = music.GetComponent<AudioSource>();
            if (bgSource != null && bgSource.isPlaying)
            {
                bgSource.Stop();
                Debug.Log("WinUI: Background music stopped.");
            }
        }
    }
} 
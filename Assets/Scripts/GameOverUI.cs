using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public AudioSource clickSound;       // 🔊 Button click
    public AudioSource gameOverMusic;    // 💀 Music when Game Over

    private AudioSource sceneMusic;      // 🎵 Main game background music
    private CanvasGroup canvasGroup;
    private float originalSceneVolume = 1f;

    void Start()
    {
        // Find the scene background music
        GameObject soundObj = GameObject.Find("Game sound");
        if (soundObj != null)
        {
            sceneMusic = soundObj.GetComponent<AudioSource>();
            if (sceneMusic != null)
                originalSceneVolume = sceneMusic.volume;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            StartCoroutine(FadeInGameOver());
        }

        // 🔁 Crossfade background music to Game Over music
        StartCoroutine(CrossfadeToGameOverMusic());

        // Let player fall, THEN freeze time
        StartCoroutine(DelayTimeFreeze());
    }

    IEnumerator CrossfadeToGameOverMusic()
    {
        float duration = 2f;
        float time = 0f;

        if (gameOverMusic != null)
        {
            gameOverMusic.volume = 0f;
            gameOverMusic.Play();
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            if (sceneMusic != null)
                sceneMusic.volume = Mathf.Lerp(originalSceneVolume, 0f, t);

            if (gameOverMusic != null)
                gameOverMusic.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (sceneMusic != null)
        {
            sceneMusic.Pause(); // ✅ Pause instead of Stop
            sceneMusic.volume = originalSceneVolume; // Reset volume for resume
        }
    }

    IEnumerator DelayTimeFreeze()
    {
        yield return new WaitForSeconds(2.5f); // Let death animation play
        Time.timeScale = 0f;
    }

    IEnumerator FadeInGameOver()
    {
        if (canvasGroup == null)
            canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            yield break;

        float duration = 1.5f;
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
    }

    public void RetryGame()
    {
        if (clickSound != null) clickSound.Play();

        Time.timeScale = 1f;

        if (gameOverMusic != null)
        {
            gameOverMusic.Stop();
        }

        // ✅ Resume background music from where it paused
        if (sceneMusic != null)
        {
            sceneMusic.UnPause();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        if (clickSound != null) clickSound.Play();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        Debug.Log("Quit Game (Editor only in build)");
    }
}

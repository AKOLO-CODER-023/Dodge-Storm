using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load when Play is clicked.")]
    public string gameSceneName = "Game 1";

    [Header("Audio Settings")]
    [Tooltip("Audio clip to play on button click.")]
    public AudioClip buttonClickSound;

    [Range(0f, 2f)]
    [Tooltip("Delay before loading scene or quitting game (to allow sound to play).")]
    public float buttonActionDelay = 1f;

    [Range(0f, 1f)]
    [Tooltip("Volume of the button click sound.")]
    public float buttonVolume = 0.7f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = buttonVolume;
    }

    public void OnPlayButtonPressed()
    {
        PlayClickSound();
        Invoke(nameof(LoadGameScene), buttonActionDelay);
    }

    public void OnQuitButtonPressed()
    {
        PlayClickSound();
        Invoke(nameof(QuitGame), buttonActionDelay);
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.volume = buttonVolume;
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    private void LoadGameScene()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ Game scene name not set in MainMenu script.");
        }
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in editor
#endif
    }
}

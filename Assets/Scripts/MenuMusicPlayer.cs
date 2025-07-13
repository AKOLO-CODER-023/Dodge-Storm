using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuMusicPlayer : MonoBehaviour
{
    public AudioClip menuMusic;   // Drag your music clip here in Inspector
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f; // Adjust volume (0.0 to 1.0)
    }

    void Start()
    {
        audioSource.Play();
    }
}


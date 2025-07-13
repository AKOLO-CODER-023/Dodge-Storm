using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    void Start()
    {
        // Load saved values or default to 1
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        ApplyVolumes();

        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });
    }

    void OnMusicVolumeChange()
    {
        float vol = musicSlider.value;
        musicSource.volume = vol;
        PlayerPrefs.SetFloat("MusicVolume", vol);
        PlayerPrefs.Save(); // 🆕 Save instantly
    }

    void OnSFXVolumeChange()
    {
        float vol = sfxSlider.value;
        foreach (AudioSource src in sfxSources)
        {
            src.volume = vol;
        }
        PlayerPrefs.SetFloat("SFXVolume", vol);
        PlayerPrefs.Save(); // 🆕 Save instantly
    }

    void ApplyVolumes()
    {
        musicSource.volume = musicSlider.value;
        foreach (AudioSource src in sfxSources)
        {
            src.volume = sfxSlider.value;
        }
    }
}

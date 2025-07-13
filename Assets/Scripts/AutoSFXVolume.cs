using UnityEngine;

public class AutoSFXVolume : MonoBehaviour
{
    private AudioSource sfx;

    void Start()
    {
        // Try to get AudioSource on this GameObject
        sfx = GetComponent<AudioSource>();

        // If not found, try on child GameObject
        if (sfx == null)
        {
            sfx = GetComponentInChildren<AudioSource>();
        }

        // If AudioSource found, set volume from PlayerPrefs
        if (sfx != null)
        {
            sfx.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
        else
        {
            Debug.LogWarning("AutoSFXVolume: No AudioSource found on " + gameObject.name);
        }
    }
}

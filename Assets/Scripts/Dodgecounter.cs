using TMPro;
using UnityEngine;
using System.Collections;

public class DodgeCounter : MonoBehaviour
{
    public int dodgeGoal = 20;
    public int dodgedCount = 0;

    public TextMeshProUGUI dodgeText;

    [Header("🎧 Milestone Sound")]
    public AudioSource audioSource;
    public AudioClip dodgeMilestoneClip;

    [Header("⚠️ Lost in Computation")]
    public PlayerMovement playerMovement;
    public GameObject reverseWarningText;
    public float reverseDuration = 4f;
    private bool hasTriggeredReverse = false;

    private bool hasWon = false;

    void Start()
    {
        UpdateUI();

        // Hide the warning text at start
        if (reverseWarningText != null)
        {
            reverseWarningText.SetActive(false);
        }
    }

    public void AddDodge()
    {
        if (hasWon) return;

        dodgedCount++;
        UpdateUI();

        // 🎯 Play milestone sound every 5 dodges
        if (dodgedCount % 5 == 0 && audioSource != null && dodgeMilestoneClip != null)
        {
            audioSource.PlayOneShot(dodgeMilestoneClip);
        }

        // 🧠 Trigger confusion effect at 10 dodges
        if (dodgedCount == 10 && !hasTriggeredReverse)
        {
            hasTriggeredReverse = true;
            StartCoroutine(ReverseControlsTemporarily());
        }

        if (dodgedCount >= dodgeGoal)
        {
            TriggerWin();
        }
    }

    void UpdateUI()
    {
        dodgeText.text = $"Coconuts Dodged: {dodgedCount} / {dodgeGoal}";
    }

    void TriggerWin()
    {
        hasWon = true;
        Debug.Log("✅ Win condition reached!");

        WinUI winUI = FindObjectOfType<WinUI>();
        if (winUI != null)
        {
            winUI.ShowWin();
        }
        else
        {
            Debug.LogWarning("⚠️ WinUI script not found in the scene.");
        }
    }

    private IEnumerator ReverseControlsTemporarily()
    {
        if (playerMovement != null)
        {
            // Show warning
            if (reverseWarningText != null)
                reverseWarningText.SetActive(true);

            // Reverse controls and slow time
            playerMovement.isReversed = true;
            Time.timeScale = 0.4f;

            Debug.Log("🔁 Controls reversed + time slowed");

            yield return new WaitForSecondsRealtime(reverseDuration); // not affected by slow time

            // Restore normal state
            Time.timeScale = 1f;
            playerMovement.isReversed = false;

            if (reverseWarningText != null)
                reverseWarningText.SetActive(false);

            Debug.Log("✅ Controls and time reset");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] heartImages; // Assign 3 heart UI images in the Inspector
    public AudioSource hitSound;
    public AudioSource deathSound;

    private int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        Debug.Log("⚠️ TakeDamage called. Damage: " + amount + ", Current Health: " + currentHealth);

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (hitSound != null) hitSound.Play();
            if (anim != null) anim.SetTrigger("Hit");
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < currentHealth;
        }
    }

    void Die()
    {
        isDead = true;

        if (deathSound != null) deathSound.Play();
        if (anim != null) anim.SetTrigger("Die");

        // ✅ Show Game Over panel through GameOverUI script
        GameOverUI ui = FindObjectOfType<GameOverUI>();
        if (ui != null)
        {
            ui.ShowGameOver();
        }

        // Optional: Add ragdoll or physics effect
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(Vector3.back * 3f + Vector3.up * 4f, ForceMode.Impulse);
        }

        Debug.Log("💀 Player has died.");
    }
}

using UnityEngine;

public class CoconutFade : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Material material;
    private Color originalColor;
    private bool isFading = false;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Make a unique material for this instance
            material = renderer.material;
            originalColor = material.color;
        }

        // Auto-fade after 10 seconds if nothing else fades it
        Invoke(nameof(TriggerFade), 10f);
    }

    public void TriggerFade()
    {
        if (!isFading && material != null)
        {
            isFading = true;
            StartCoroutine(FadeOut());
        }
    }

    System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

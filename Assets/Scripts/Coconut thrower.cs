using UnityEngine;

public class CoconutThrower : MonoBehaviour
{
    public GameObject coconutPrefab;
    public Transform throwPoint;
    public Transform coconutcontainer;
    public Transform playerTransform;
    public float arcHeight = 1.5f;

    public AudioClip[] monkeySounds; // 🎵 Add multiple taunt/throw sounds
    private AudioSource audioSource;

    private float minDelay = 2f;
    private float maxDelay = 4f;
    private float timer = 0f;
    private float nextThrowTime;

    void Start()
    {
        SetNextThrowTime();

        // 🎵 Get AudioSource component (make sure one is added in the Inspector or via script)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // For 3D sound (optional)
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextThrowTime)
        {
            ThrowCoconut();
            SetNextThrowTime();
            timer = 0f;
        }

        if (minDelay > 0.5f)
            minDelay -= Time.deltaTime * 0.01f;

        if (maxDelay > 1f)
            maxDelay -= Time.deltaTime * 0.01f;
    }

    void SetNextThrowTime()
    {
        nextThrowTime = Random.Range(minDelay, maxDelay);
    }

    void ThrowCoconut()
    {
        if (coconutPrefab == null || throwPoint == null || playerTransform == null) return;

        // 🎵 Play a random monkey sound
        if (monkeySounds != null && monkeySounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = monkeySounds[Random.Range(0, monkeySounds.Length)];
            audioSource.PlayOneShot(clip);
        }

        GameObject coconut = Instantiate(coconutPrefab, throwPoint.position, Quaternion.identity, coconutcontainer);
        Destroy(coconut, 10f); // 🧹 Auto-destroy after 10 seconds ✅

        Rigidbody rb = coconut.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 targetPos = playerTransform.position + new Vector3(0, 1.2f, 0); // chest height
            Vector3 velocity = CalculateArcVelocity(targetPos, throwPoint.position, arcHeight);
            rb.linearVelocity = velocity; // ✅ Correct property
        }
    }

    Vector3 CalculateArcVelocity(Vector3 target, Vector3 origin, float arcHeight)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        Vector3 direction = target - origin;
        Vector3 horizontal = new Vector3(direction.x, 0, direction.z);

        float distance = horizontal.magnitude;
        float yOffset = direction.y;

        float initialYVelocity = Mathf.Sqrt(2 * gravity * arcHeight);
        float timeToApex = initialYVelocity / gravity;
        float totalTime = timeToApex + Mathf.Sqrt(2 * (arcHeight - yOffset) / gravity);

        Vector3 horizontalVelocity = horizontal / totalTime;
        return horizontalVelocity + Vector3.up * initialYVelocity;
    }
}

using UnityEngine;

[RequireComponent(typeof(CoconutStatus))]
public class CoconutCollisionHandler : MonoBehaviour
{
    private CoconutStatus status;
    private Rigidbody rb;

    private bool canDamage = true; // Controls whether the coconut can damage

    void Start()
    {
        status = GetComponent<CoconutStatus>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // If coconut is already processed, don't process again
        if (status.isProcessed) return;

        // If the coconut hits the ground, disable its ability to damage
        if (collision.gameObject.CompareTag("Ground"))
        {
            canDamage = false;
            status.isProcessed = true;
            return;
        }

        // If it hits the player and damage is allowed, deal damage
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            status.hitPlayer = true;
            status.isProcessed = true;

            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(1); // Damage the player
            }

            canDamage = false; // Prevent further damage by this coconut
        }
    }
}

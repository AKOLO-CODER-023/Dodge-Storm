using UnityEngine;

public class CoconutCatcher : MonoBehaviour
{
    public DodgeCounter dodgeCounter;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the catcher trigger: " + other.name);

        if (other.CompareTag("coconut"))
        {
            Debug.Log("Coconut entered the catcher trigger.");

            CoconutStatus status = other.GetComponent<CoconutStatus>();

            if (status == null)
            {
                Debug.LogWarning("No CoconutStatus script found on: " + other.name);
            }
            else if (status.isProcessed)
            {
                Debug.Log("Coconut already processed.");
            }
            else
            {
                status.isProcessed = true;

                if (!status.hitPlayer)
                {
                    Debug.Log("✅ Coconut was dodged!");
                    if (dodgeCounter != null)
                        dodgeCounter.AddDodge();
                    else
                        Debug.LogError("DodgeCounter is not assigned!");
                }
                else
                {
                    Debug.Log("❌ Coconut hit the player.");
                }
            }

            // 🟢 Use fade instead of Destroy
            CoconutFade fade = other.GetComponent<CoconutFade>();
            if (fade != null)
            {
                fade.TriggerFade();
            }
            else
            {
                Destroy(other.gameObject, 0.2f);
            }
        }
    }
}

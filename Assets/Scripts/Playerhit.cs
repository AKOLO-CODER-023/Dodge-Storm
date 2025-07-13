using UnityEngine;

public class Playerhit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coconut"))
        {
            // ✅ Only fade visual — no more damage here!
            StartCoconutFade(other.gameObject);
        }
    }

    void StartCoconutFade(GameObject coconut)
    {
        CoconutFade fade = coconut.GetComponent<CoconutFade>();
        if (fade != null)
        {
            fade.TriggerFade();
        }
        else
        {
            Destroy(coconut, 0.2f); // fallback
        }
    }
}

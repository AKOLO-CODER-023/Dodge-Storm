using UnityEngine;

public class CoconutStatus : MonoBehaviour
{
    public bool wasCaught = false;
    public bool hitPlayer = false;
    public bool isProcessed = false; // NEW: to ensure it's only handled once
}

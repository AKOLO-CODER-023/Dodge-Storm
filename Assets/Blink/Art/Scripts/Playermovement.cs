using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;

    [HideInInspector] public bool isReversed = false; // ✅ Add this flag

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        // ✅ Reverse horizontal input if needed
        if (isReversed)
        {
            horizontal *= -1;
        }

        // Clamp tiny values to zero
        if (Mathf.Abs(horizontal) < 0.01f)
            horizontal = 0f;

        // Move the player
        Vector3 move = new Vector3(horizontal, 0, 0);
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = -1f;
        }

        // Set animation parameter
        if (animator != null)
        {
            animator.SetFloat("MoveX", horizontal);
        }
    }
}

using UnityEngine;          // Core Unity library (MonoBehaviour, Vector2, etc.)
using UnityEngine.InputSystem; // New Input System library (Mouse, Touchscreen, etc.)
using UnityEngine.SceneManagement;


public class NewMonoBehaviourScript : MonoBehaviour
{
    // movespeed controls how fast the object moves
    // 'public' makes it visible and editable in the Unity Inspector(i can change directly by unity sofware)
    public float movespeed = 5f;

    // Reference to the Rigidbody2D component attached to this GameObject
    // Used to apply physics-based movement 
    Rigidbody2D rb;

    // Start() is called ONCE when the game starts
    void Start()
    {
        // Find and store the Rigidbody2D component on this same GameObject
        // So we don't have to search for it every frame (better performance)
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate() is called at a FIXED time interval (default: 50 times/sec)
    // Always use FixedUpdate for physics — never Update() — for consistency
    void FixedUpdate()
    {
        // Flag to track whether the player is currently touching/clicking
        // Starts as false each frame, set to true if input is detected
        bool isTouching = false;

        // Stores the screen position of the touch or mouse click
        // Vector2.zero = (0, 0) as default until real input is detected
        Vector2 inputPosition = Vector2.zero;

        // ── MOBILE INPUT
        // Check if a touchscreen exists on this device AND a finger is pressing
        // 'primaryTouch' = the first finger touching the screen
        // '.press.isPressed' = returns true while the finger is held down
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Input detected — set flag to true
            isTouching = true;

            // Read the (x, y) screen position of the finger
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        // ── PC / EDITOR INPUT ─────────────────────────────────────────
        // If no touch detected, check if a mouse exists AND left button is held
        // 'else if' means this only runs if the touch block above was false
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            // Input detected — set flag to true
            isTouching = true;

            // Read the (x, y) screen position of the mouse cursor
            inputPosition = Mouse.current.position.ReadValue();
        }

        // ── MOVEMENT LOGIC ────────────────────────────────────────────
        // Only move if we detected any input (touch or mouse)
        if (isTouching)
        {
            // Convert raw screen position (e.g. 0–1920) to viewport position (0.0–1.0)
            // Viewport space: (0,0) = bottom-left, (1,1) = top-right, (0.5, 0.5) = center
            Vector3 touchPos = Camera.main.ScreenToViewportPoint(inputPosition);

            // If the input is on the LEFT half of the screen (x < 0.5)
            if (touchPos.x < 0.5f)
                // Apply force to the LEFT
                // Vector2.left = (-1, 0), multiplied by movespeed for strength
                rb.AddForce(Vector2.left * movespeed);
            else
                // Input is on the RIGHT half of the screen (x >= 0.5)
                // Apply force to the RIGHT
                // Vector2.right = (1, 0), multiplied by movespeed for strength
                rb.AddForce(Vector2.right * movespeed);
        }

        // ── DECELERATION
        else
        {
            // No input this frame — gradually slow the object down
            // Lerp smoothly interpolates between current velocity and zero
            // 0.2f = 20% closer to zero each FixedUpdate (smooth stop)
            // Without this, the object would slide forever due to physics
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Block")
        {
            SceneManager.LoadScene(0);
        }
    }
}

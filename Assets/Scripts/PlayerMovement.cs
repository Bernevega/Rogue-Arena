// Ann Bernevega - edited 27.1.2025

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement, can be adjusted in the inspector

    private Rigidbody2D rb; // Rigidbody2D component used for movement
    private Vector2 movement; // Stores the player's input for movement

    void Start()
    {
        // Get the Rigidbody2D component attached to the player object at the start
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from the player for horizontal and vertical movement
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow to move horizontally
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrow to move vertically

        // Normalize movement to maintain consistent speed even when moving diagonally (equal speed in all directions)
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the player by setting the Rigidbody2D's linear velocity to the desired movement direction and speed
        rb.linearVelocity = movement * moveSpeed; 
    }

    public void IncreaseMovementSpeed()
    {
        moveSpeed = moveSpeed + 1; // Increases the movement speed by 1 each time this method is called
    }
}

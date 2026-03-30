// Ann Bernevega - edited 28.2.2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator myAnim; // Reference to the Animator component for controlling animations
    private Rigidbody2D myRB; // Reference to the Rigidbody2D component for player movement physics
    Vector2 movement; // Variable to store player movement direction

    void Start()
    {
        myAnim = GetComponent<Animator>(); // Get the Animator component attached to the player
        myRB = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
    }

    void Update()
    {
        // Get movement inputs from player for horizontal and vertical axes
        movement.x = Input.GetAxisRaw("Horizontal"); // Horizontal movement input (A/D or arrow keys)
        movement.y = Input.GetAxisRaw("Vertical"); // Vertical movement input (W/S or arrow keys)

        // Set movement parameters in the animator for walking animations
        myAnim.SetFloat("moveX", movement.x); // Set the X direction of movement for animation
        myAnim.SetFloat("moveY", movement.y); // Set the Y direction of movement for animation

        // Update the last movement direction when the player is moving
        if (movement != Vector2.zero)
        {
            // Set the last known movement direction for idle animations
            myAnim.SetFloat("lastMoveX", movement.x); 
            myAnim.SetFloat("lastMoveY", movement.y);
        }

        // Trigger attack animation when the player presses the attack button (Fire1)
        if (Input.GetButtonDown("Fire1"))
        {
            myAnim.SetTrigger("isAttacking"); // Activate the "isAttacking" trigger in the animator
        }
    }
}

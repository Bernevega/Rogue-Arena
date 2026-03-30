// Ann Bernevega - edited 2.3.2025

using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    private EndBoss bossMovement; // Reference to the boss movement script
    private Animator animator; // Animator for the boss
    private Animator camAnim; // Animator for the camera
    private Vector2 lastDirection = Vector2.down; // Default direction facing downward

    void Start()
    {
        animator = GetComponent<Animator>();
        bossMovement = GetComponent<EndBoss>(); // Get reference to the movement script
        camAnim = GameObject.FindWithTag("MainCamera").GetComponent<Animator>(); // Find the main camera and get its animator
    }

    void Update()
    {
        if (bossMovement != null)
        {
            // Calculate movement direction based on the player's position
            Vector2 direction = (bossMovement.player.position - transform.position).normalized;

            // If the movement is significant, update the last direction
            if (direction.sqrMagnitude > 0.01f)
            {
                lastDirection = direction; 
            }

            // Update animator parameters
            animator.SetBool("isStanding", false);
            animator.SetFloat("moveX", lastDirection.x);
            animator.SetFloat("moveY", lastDirection.y);
        }

        // If the boss is walking to the center, ensure it's not standing still
        if (bossMovement.isWalkingToCenter == true)
        {
            animator.SetBool("isStanding", false);
        }

        // If the boss is paused, set the standing animation
        if (bossMovement.isPaused == true)
        {
            animator.SetBool("isStanding", true);
        }

        // If the boss is charging, trigger the shake animation and log the event
        if (bossMovement.isCharging == true)
        {
            animator.SetBool("isStanding", false); 
            camAnim.Play("boss_shake"); // Play camera shake animation
            Debug.Log("Camera shakes");
        }

        // If the boss is shooting basketballs, set the animation accordingly
        if (bossMovement.isShootingBasketballs == true)
        {
            animator.SetBool("isStanding", false);
        }

        // If the boss is shooting frisbees, update the animation state
        if (bossMovement.isShootingFrisbees == true)
        {
            animator.SetBool("isStanding", false);
        }
    }
}

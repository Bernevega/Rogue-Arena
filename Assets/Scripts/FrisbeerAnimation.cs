// Ann Bernevega - edited 27.2.2025

using UnityEngine;

public class FrisbeerAnimation : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component to control animations
    private Vector2 lastDirection = Vector2.down; // Default direction facing downward
    private FrisbeeEnemy enemyMovement; // Reference to the enemy movement script

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
        enemyMovement = GetComponent<FrisbeeEnemy>(); // Get the FrisbeeEnemy script attached to the same GameObject
    }

    void Update()
    {
        // Check if the enemy movement script exists and has a valid target point
        if (enemyMovement != null && enemyMovement.targetPoint != null)
        {
            // Calculate the movement direction towards the target point
            Vector2 direction = (enemyMovement.targetPoint.position - transform.position).normalized;

            // Only update the last direction if there's actual movement
            if (direction.sqrMagnitude > 0.01f) 
            {
                lastDirection = direction; // Store last movement direction for animation purposes
            }

            // Set movement animation parameters
            animator.SetBool("isIdle", false); // Enemy is moving, so it's not idle
            animator.SetFloat("moveX", lastDirection.x); // Update horizontal movement direction
            animator.SetFloat("moveY", lastDirection.y); // Update vertical movement direction
        }

        // If the enemy is in a standing state, set the idle animation
        if (enemyMovement.isStanding == true)
        {
            animator.SetBool("isIdle", true); // Enemy is idle, so play the idle animation
        }
    }
}

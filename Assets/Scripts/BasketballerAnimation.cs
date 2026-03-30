// Ann Bernevega - edited 4.2.2025

using UnityEngine;

public class BasketballerAnimation : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private Vector2 lastDirection = Vector2.down; // Default direction facing downward
    private BasketballEnemy enemyMovement; // Reference to the enemy movement script

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
        enemyMovement = GetComponent<BasketballEnemy>(); // Get reference to the enemy movement script
    }

    void Update()
    {
        // Ensure the enemy has a target player before updating animations
        if (enemyMovement != null && enemyMovement.player != null)
        {
            // Calculate the direction towards the player
            Vector2 direction = (enemyMovement.player.position - transform.position).normalized;

            // Update animation direction only if movement is noticeable
            if (direction.sqrMagnitude > 0.01f)
            {
                lastDirection = direction; // Store the last movement direction
            }

            // Update animation parameters to reflect movement direction
            animator.SetFloat("moveX", lastDirection.x);
            animator.SetFloat("moveY", lastDirection.y);
        }
    }
}

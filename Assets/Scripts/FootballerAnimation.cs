// Ann Bernevega - edited 4.2.2025

using UnityEngine;

public class FootballerAnimation : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private Vector2 lastDirection = Vector2.down; // Default direction (facing downward)
    private FootballEnemy enemyMovement; // Reference to the enemy movement script

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator attached to the GameObject
        enemyMovement = GetComponent<FootballEnemy>(); // Get reference to movement script
    }

    void Update()
    {
        // Ensure the enemy movement script and player reference exist
        if (enemyMovement != null && enemyMovement.player != null)
        {
            // Calculate movement direction towards the player
            Vector2 direction = (enemyMovement.player.position - transform.position).normalized;

            // Only update direction if movement is significant (avoids flickering animations)
            if (direction.sqrMagnitude > 0.01f)
            {
                lastDirection = direction; // Store last movement direction
            }

            // Update animator parameters to reflect movement direction
            animator.SetFloat("moveX", lastDirection.x);
            animator.SetFloat("moveY", lastDirection.y);
        }
    }
}

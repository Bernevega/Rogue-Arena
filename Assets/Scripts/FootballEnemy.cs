// Ann Bernevega - edited 27.1.2025

using UnityEngine;

public class FootballEnemy : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    public float speed = 2f; // Default speed of the enemy

    void Start()
    {
        // Check if the player reference is not set
        if (player == null)
        {
            // Find the player by its tag and assign it to the player variable
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Only move if there is a player assigned
        if (player != null)
        {
            // Move the enemy towards the player each frame
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to the player by subtracting enemy position from player position
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the enemy in that direction with a certain speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        // If the enemy collides with a bullet or the player, destroy itself
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Player"))
        {
            // Add points to the score for defeating the enemy
            ScoreManager.instance.AddScore(10);
            // Destroy the enemy object
            Destroy(gameObject);
        }
    }
}

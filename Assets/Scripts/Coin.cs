// Ann Bernevega - edited 18.2.2025

using UnityEngine;

public class Coin : MonoBehaviour
{   
    // Detects collision with other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the colliding object has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            // Increase the player's score by 25 points
            ScoreManager.instance.AddScore(25);

            // Destroy the coin object after collection
            Destroy(gameObject);
        }
    }
}

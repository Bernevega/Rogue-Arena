// Ann Bernevega - edited 1.2.2025

using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float rotationSpeed = 360f; // Degrees per second for bullet's rotation speed

    void Start()
    {
        // Destroy the bullet after 2 seconds to prevent it from staying in the scene indefinitely
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        // Continuously rotate the bullet. The speed is adjustable by changing rotationSpeed
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the bullet if it collides with anything that's not an enemy
        if (!collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

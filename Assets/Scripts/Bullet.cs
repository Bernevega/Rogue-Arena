// Ann Bernevega - edited 2.3.2025

using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotationSpeed = 360f; // Degrees per second for bullet rotation
    [SerializeField] private AudioClip hitSound; // Sound to play on collision, assign in Inspector

    private AudioSource audioSource; // Handles audio playback
    private SpriteRenderer spriteRenderer; // Controls the bullet's visual representation
    private Collider2D bulletCollider; // Manages collision detection
    private bool isDestroying = false; // Prevents multiple destruction triggers

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Get the sprite renderer and collider components
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Rotate the bullet continuously
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Log the name of the object the bullet collides with (for debugging)
        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        // If the bullet hits an enemy and hasn't already started being destroyed
        if (collision.gameObject.CompareTag("Enemy") && !isDestroying)
        {
            isDestroying = true; // Prevents multiple triggers
            HideAndPlaySound(); // Handle visual/audio effects before destruction
        }
        else
        {
            Destroy(gameObject); // Destroy immediately if it collides with something else
        }
    }

    private void HideAndPlaySound()
    {
        // Hide bullet sprite and disable collider
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (bulletCollider != null) bulletCollider.enabled = false;

        // Play collision sound if available, then destroy bullet
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
            Destroy(gameObject, hitSound.length); // Destroy after the sound finishes
        }
        else
        {
            Destroy(gameObject); // Destroy instantly if no sound is set
        }
    }
}

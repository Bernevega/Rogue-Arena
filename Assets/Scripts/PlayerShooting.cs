// Ann Bernevega - edited 27.1.2025

using UnityEngine;
using System.Collections;
using UnityEngine.Audio; // Required for working with AudioMixers

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet to be fired
    public Transform firePoint;     // The position and direction where the bullet will spawn
    public float bulletSpeed = 10f; // Speed of the bullet
    public float fireRate = 0.5f;   // Time between shots
    private float shootDelay = 0.2f; // Delay before the bullet is fired (e.g., when the animation reaches the right frame)

    private float nextFireTime = 0f; // Time when the next shot can be fired
    private Animator animator;       // Reference to the player's Animator

    public AudioClip shootSound;     // Sound effect for shooting
    public AudioSource audioSource; // Audio source for playing sounds
    public AudioMixerGroup soundMixerGroup; // Assign in the Inspector

    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>(); 

        // Get the AudioSource component attached to the player, or add it if it doesn't exist
        audioSource = GetComponent<AudioSource>(); 

        // If the AudioSource is missing, add one dynamically
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); 
        }

        // Assign the AudioSource to the correct Mixer Group
        if (soundMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = soundMixerGroup;
        }
    }

    void Update()
    {
        // Check if the player is pressing the fire button (left mouse click) and if enough time has passed since the last shot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // Update the time for the next shot
            StartCoroutine(ShootWithDelay()); // Start a coroutine to shoot with a delay
        }

        // Rotate the firePoint to always face the mouse direction, allowing for directional shooting
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get mouse position in world space
        Vector2 direction = (mousePosition - firePoint.position).normalized; // Calculate direction from player to mouse
        firePoint.up = direction; // Set the firePoint's rotation to face the direction of the mouse
    }

    private IEnumerator ShootWithDelay()
    {
        // Play the shooting animation when the player shoots
        if (animator != null)
        {
            animator.SetTrigger("isAttacking"); // Trigger the shooting animation
        }

        // Wait for the specified delay before shooting
        yield return new WaitForSeconds(shootDelay);

        // Play the shooting sound if it's assigned
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // Play the shooting sound effect
        }

        // Instantiate the bullet at the firePoint's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); 

        // Ignore collisions between the bullet and the player to prevent self-damage
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (bulletCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, playerCollider); // Prevent bullet-player collision
        }

        // Get the Rigidbody2D component of the bullet and apply velocity to move it
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.up * bulletSpeed; // Set the bullet's velocity in the direction of firePoint's "up"
        }
    }

    public void IncreaseAttackSpeed()
    {
        // Increase the speed of the animation
        animator.speed = 1f / fireRate;

        // Decrease the cooldown between shots, ensuring it doesn't go below 0.1 seconds
        fireRate = Mathf.Max(0.1f, fireRate - 0.10f); 
        Debug.Log("Shooting speed increased. Current shooting speed: " + fireRate); // Log the current shooting speed
    }
}

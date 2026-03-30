// Ann Bernevega - edited 27.1.2025
// Juho Karjalainen - edited 17.2.2025

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 3; // Player's current health
    public int maxHealth = 7;      // Maximum number of hearts that can be unlocked
    public Image[] fullHearts;     // Array for full heart images (only enabled if player has that health)
    public Image[] emptyHearts;    // Array for empty heart images (unlocked hearts – once enabled, they never disable)
    public Sprite fullHeartSprite; // Sprite for a full heart
    public Sprite emptyHeartSprite;// Sprite for an empty heart

    private Animator camAnim;           // Animator for camera shake when hit
    private SpriteRenderer spriteRenderer;// For toggling player sprite during invincibility
    public AudioSource audioSource;       // For playing damage sound

    public bool isInvincible = false; // Whether the player is currently invincible
    public float invincibilityDuration = 1.0f; // How long the player stays invincible after being hit
    public float flashInterval = 0.2f;         // How fast the player flashes during invincibility

    public AudioClip damageSound; // Sound effect for taking damage

    // Tracks the highest number of hearts unlocked (empty hearts that remain visible)
    private int unlockedHearts;

    void Start()
    {
        // Initially, the unlocked hearts match the starting health.
        unlockedHearts = (int)playerHealth;
        InitializeHearts();  // Set up the hearts arrays so that only the unlocked hearts are enabled initially.
        UpdateHealthUI();    // Update the heart display.
        ScoreManager.instance.ResetScore(); // Reset score when game starts.
        camAnim = GameObject.FindWithTag("MainCamera").GetComponent<Animator>(); // Get camera animator.
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get player's sprite renderer.
        audioSource = GetComponent<AudioSource>(); // Get AudioSource for damage sound.
    }

    void Update()
    {
        // If the player's health drops to 0 or below, load the lose menu.
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("Lose Menu");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If collided with an enemy and not invincible, reduce health.
        if (collision.collider.CompareTag("Enemy") && !isInvincible)
        {
            playerHealth -= 1;
            Debug.Log("Player hit by Enemy. Current health: " + playerHealth);

            // Subtract score when the player loses a heart.
            ScoreManager.instance.SubtractScore(100);
            UpdateHealthUI(); // Update the UI to reflect new health.

            camAnim.Play("Shake"); // Play camera shake animation.

            // Play the damage sound effect.
            if (audioSource != null && damageSound != null)
            {
                audioSource.clip = damageSound;
                audioSource.volume = 0.5f;
                audioSource.pitch = 1.0f;
                audioSource.Play();
            }

            // Start the invincibility period with flashing effect.
            StartCoroutine(InvincibilityTimer());
        }
    }

    // Coroutine for temporary invincibility with flashing effect.
    IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    // Increase player's health by a specified amount.
    public void IncreaseHealth(float amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Min(playerHealth, maxHealth);
        Debug.Log("Health increased by " + amount + ". Current health: " + playerHealth);

        // If the new health exceeds the previously unlocked hearts, update unlockedHearts.
        if (playerHealth > unlockedHearts)
        {
            unlockedHearts = (int)playerHealth;
        }
        UpdateHealthUI();
    }

    // Initialize the hearts: enable only the hearts that match the starting health.
    void InitializeHearts()
    {
        // First, disable all hearts.
        foreach (Image heart in fullHearts)
        {
            heart.enabled = false;
        }
        foreach (Image heart in emptyHearts)
        {
            heart.enabled = false;
        }

        // Enable hearts only up to the initial unlocked amount.
        for (int i = 0; i < unlockedHearts && i < maxHealth; i++)
        {
            if (i < fullHearts.Length)
            {
                fullHearts[i].enabled = true;
                fullHearts[i].sprite = fullHeartSprite;
            }
            if (i < emptyHearts.Length)
            {
                emptyHearts[i].enabled = true;
                emptyHearts[i].sprite = emptyHeartSprite;
            }
        }
    }

    // Update the heart UI display.
    void UpdateHealthUI()
    {
        // Loop from 0 to maxHealth (or the length of the arrays if shorter).
        for (int i = 0; i < maxHealth; i++)
        {
            // Update empty hearts:
            // If this heart index is less than the number of unlocked hearts, ensure it's enabled.
            if (i < unlockedHearts && i < emptyHearts.Length)
            {
                emptyHearts[i].enabled = true;
                emptyHearts[i].sprite = emptyHeartSprite;
            }
            else if (i < emptyHearts.Length)
            {
                emptyHearts[i].enabled = false;
            }

            // Update full hearts:
            // Only enable a full heart if the index is less than current player health.
            if (i < playerHealth && i < fullHearts.Length)
            {
                fullHearts[i].enabled = true;
                fullHearts[i].sprite = fullHeartSprite;
            }
            else if (i < fullHearts.Length)
            {
                fullHearts[i].enabled = false;
            }
        }
    }
}

// Ann Bernevega - edited 27.1.2025
// Juho Karjalainen - edited 25.2.2025

using UnityEngine;

public class BonusSystem : MonoBehaviour
{
    // References to player-related scripts
    private PlayerHealth playerHealthScript;
    private PlayerShooting playerShootingScript;
    private PlayerMovement playerMovementScript;
    private EnemySpawner enemySpawnerScript; // Reference to the enemy spawner

    private GameObject currentBonus; // Stores the currently available bonus item

    void Start()
    {
        // Get references to the necessary player scripts
        playerHealthScript = GetComponent<PlayerHealth>();
        playerShootingScript = GetComponent<PlayerShooting>();
        playerMovementScript = GetComponent<PlayerMovement>();

        // Find the EnemySpawner in the scene
        enemySpawnerScript = FindFirstObjectByType<EnemySpawner>(); 
    }

    void Update()
    {
        // If there's a bonus nearby and the player presses 'E', pick it up
        if (currentBonus != null && Input.GetKeyDown(KeyCode.E))
        {
            HandlePickup(currentBonus);
            currentBonus = null; // Reset current bonus after pickup
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger zone of a bonus item
        if (other.CompareTag("HealthBonus") || other.CompareTag("AttackSpeed") || other.CompareTag("MovementSpeed"))
        {
            currentBonus = other.gameObject; // Store reference to the detected bonus
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the player leaves the bonus item’s trigger zone, remove reference
        if (other.gameObject == currentBonus)
        {
            currentBonus = null; 
        }
    }

    void HandlePickup(GameObject bonus)
    {
        // Apply the effect based on the bonus type
        if (bonus.CompareTag("HealthBonus"))
        {
            playerHealthScript.IncreaseHealth(1); // Increase player's health
        }
        else if (bonus.CompareTag("AttackSpeed"))
        {
            playerShootingScript.IncreaseAttackSpeed();  // Increase player's attack speed
        }
        else if (bonus.CompareTag("MovementSpeed"))
        {
            playerMovementScript.IncreaseMovementSpeed(); // Increase player's movement speed
        }
        
        // Notify the enemy spawner to trigger a new wave after picking a bonus
        TriggerBonusSelection();
    }

    void TriggerBonusSelection()
    {
        enemySpawnerScript.OnBonusSelected(); // Start a new wave of enemies
    }
}

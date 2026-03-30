// Ann Bernevega - edited 2.2.2025

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndBoss : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f; // How fast the boss moves when walking
    [SerializeField] private float moveSpeed = 2f; // Movement speed while in circle
    [SerializeField] private float pauseDuration = 1f; // Time to pause before starting attacks

    [Header("Attacks")]
    [SerializeField] private float basketballSpeed = 4f; // Speed of basketball projectiles
    [SerializeField] private float frisbeeSpeed = 4f; // Speed of frisbee projectiles
    [SerializeField] private float shootInterval = 3f; // How often the boss shoots
    [SerializeField] private float chargeCooldown = 6f; // Cooldown between charge attacks
    [SerializeField] private float chargeSpeed = 6f; // Speed of charging attack
    [SerializeField] private float chargeDuration = 1f; // How long the charge lasts

    [Header("Health")]
    [SerializeField] public int health = 100; // Boss's health

    private GameObject basketballPrefab; // Prefab for basketballs
    private GameObject frisbeePrefab; // Prefab for frisbees
    private Transform spawnPoint; // Where the boss spawns
    private Transform centerPoint; // Point the boss will orbit around
    private EnemySpawner enemySpawner; // Reference to the enemy spawner

    public Transform player; // Reference to the player
    private Animator anim; // Animator for the boss
    private float shootTimer; // Timer to control shooting intervals
    private float chargeTimer; // Timer to control charge attack cooldown
    private float radius; // How far the boss moves in the circle
    private float angle; // Current angle for circular movement
    private float direction; // Direction of movement (clockwise or counterclockwise)
    public bool isWalkingToCenter = true; // Is the boss walking to the center?
    public bool isPaused = false; // Is the boss paused between actions?
    public bool isCharging = false; // Is the boss charging at the player?
    public bool isShootingBasketballs = false; // Is the boss currently shooting basketballs?
    public bool isShootingFrisbees = false; // Is the boss currently shooting frisbees?

    private SpriteRenderer spriteRenderer; // For handling the boss's sprite color changes

    void Start()
    {
        enemySpawner = FindFirstObjectByType<EnemySpawner>(); // Get reference to the spawner

        health = enemySpawner.GetEndBossHealth(this); // Get health from spawner (could vary)
        player = GameObject.FindWithTag("Player")?.transform; // Find the player object

        // Load projectiles from Resources folder
        basketballPrefab = Resources.Load<GameObject>("Prefabs/Basketball");
        frisbeePrefab = Resources.Load<GameObject>("Prefabs/Frisbee");

        spawnPoint = GameObject.Find("SpawnPoint")?.transform; // Get spawn point
        centerPoint = GameObject.Find("CenterPoint")?.transform; // Get center point

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        if (spriteRenderer == null) 
        {
            Debug.LogError("SpriteRenderer not found on EndBoss!"); // Just in case the component is missing
        }

        // Make sure the prefabs are correctly loaded
        if (basketballPrefab == null) Debug.LogError("Basketball prefab not found!");
        if (frisbeePrefab == null) Debug.LogError("Frisbee prefab not found!");

        // Set initial position and other values
        transform.position = spawnPoint.position;
        shootTimer = shootInterval;
        chargeTimer = chargeCooldown;
        direction = Random.Range(0f, 1f) > 0.5f ? 1f : -1f; // Randomize movement direction
        radius = Random.Range(3f, 3.5f); // Set movement radius
    }

    void Update()
    {
        if (player == null) return; // Don't do anything if player is missing

        if (isWalkingToCenter)
        {
            MoveToCenter(); // Walk to the center point
        }
        else if (!isCharging && !isPaused)
        {
            MoveInCircle(); // Once at center, move in a circle
        }

        HandleShooting(); // Handle shooting projectiles
        HandleCharging(); // Handle charge attack logic

        // Keep the boss inside a specific area to prevent it from going off-screen
        ClampPosition();
    }

    private void ClampPosition()
    {
        // Define boundaries based on the center point
        float minX = centerPoint.position.x - 3.5f;
        float maxX = centerPoint.position.x + 3.5f;
        float minY = centerPoint.position.y - 3f;
        float maxY = centerPoint.position.y + 3f;

        // Clamp the position to these boundaries
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY)
        );

        transform.position = clampedPosition; // Apply the clamped position
    }

    private void MoveToCenter()
    {
        // Move towards the center
        transform.position = Vector2.MoveTowards(transform.position, centerPoint.position, walkSpeed * Time.deltaTime);

        // Once we reach the center, start the attack phase
        if (Vector2.Distance(transform.position, centerPoint.position) < 0.1f)
        {
            isWalkingToCenter = false;
            StartCoroutine(PauseBeforeStartingAttacks());
        }
    }

    private void MoveInCircle()
    {
        // Calculate target position for circular movement
        float targetX = centerPoint.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float targetY = centerPoint.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        Vector2 targetPosition = new Vector2(targetX, targetY);

        // Move towards the target position
        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update the angle for circular movement
        angle += direction * moveSpeed * 10 * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;
        if (angle < 0f) angle += 360f;
    }

    private void HandleShooting()
    {
        if (!isWalkingToCenter)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                if (isShootingBasketballs)
                    ShootMultipleBasketballs(); // Shoot basketballs if flag is set
                else
                    ShootMultipleFrisbees(); // Otherwise, shoot frisbees

                // Toggle shooting mode for the next interval
                isShootingBasketballs = !isShootingBasketballs;
                shootTimer = shootInterval; // Reset shoot timer
            }
        }
    }

    private void ShootMultipleBasketballs()
    {
        int numberOfProjectiles = 24; // Number of basketballs to shoot
        float angleSpread = 15f; // Spread angle for basketballs

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float spreadAngle = (i - (numberOfProjectiles - 1) / 2f) * angleSpread;
            ShootBasketball(spreadAngle);
        }
    }

    private void ShootBasketball(float spreadAngle)
    {
        // Create a random offset to make the shooting more unpredictable
        Vector2 spawnOffset = Random.insideUnitCircle * 0.5f;
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;

        GameObject basketball = Instantiate(basketballPrefab, spawnPosition, Quaternion.identity); // Instantiate basketball
        Rigidbody2D rb = basketball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized; // Direction to player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + spreadAngle;
            Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            rb.AddForce(shootDirection * basketballSpeed, ForceMode2D.Impulse); // Shoot with applied force
        }
    }

    private void ShootMultipleFrisbees()
    {
        int numberOfProjectiles = 16; // Number of frisbees to shoot
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            ShootFrisbee();
        }
    }

    private void ShootFrisbee()
    {
        // Create a random offset for frisbee spawn
        Vector2 spawnOffset = Random.insideUnitCircle * 1.5f;
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
        GameObject frisbee = Instantiate(frisbeePrefab, spawnPosition, Quaternion.identity); // Instantiate frisbee
        Rigidbody2D rb = frisbee.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random direction for frisbee
            rb.AddForce(randomDirection * frisbeeSpeed, ForceMode2D.Impulse); // Apply force to frisbee
        }
    }

    private void HandleCharging()
    {
        chargeTimer -= Time.deltaTime;
        if (chargeTimer <= 0f)
        {
            StartCoroutine(ChargeAtPlayer()); // Start charging at the player
            chargeTimer = chargeCooldown; // Reset charge cooldown
        }
    }

    private IEnumerator ChargeAtPlayer()
    {
        isCharging = true;
        Vector2 chargeDirection = (player.position - transform.position).normalized; // Direction to charge
        float elapsedTime = 0f;

        // Charge towards player for a set duration
        while (elapsedTime < chargeDuration)
        {
            transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // Wait after charge
        StartCoroutine(SmoothReturnToCircle()); // Smoothly return to circle movement
        isCharging = false;
    }

    private IEnumerator SmoothReturnToCircle()
    {
        isPaused = true; // Pause before resuming movement
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false; // Resume movement
    }

    private IEnumerator PauseBeforeStartingAttacks()
    {
        isPaused = true; // Pause before attacking
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false; // Resume actions
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet")) // If hit by a bullet
        {
            TakeDamage(1); // Take damage
            Destroy(other.gameObject); // Destroy the bullet
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage; // Reduce health
        Debug.Log("Boss Health: " + health);

        StartCoroutine(FlashRed()); // Flash red when hit

        if (health <= 0) // If health is zero, boss dies
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0.4f, 0.4f, 1f); // Change color to red
        yield return new WaitForSeconds(0.1f); // Keep red color for a short time
        spriteRenderer.color = Color.white; // Reset to normal color
    }

    private void Die()
    {
        Debug.Log("Boss Defeated!"); // Log when the boss dies
        Destroy(gameObject); // Destroy the boss object
        // You could add a scene transition or a victory screen here.
    }
}
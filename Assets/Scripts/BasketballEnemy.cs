// Ann Bernevega - edited 11.2.2025

using UnityEngine;

public class BasketballEnemy : MonoBehaviour
{
    [SerializeField] private GameObject basketballPrefab; // The basketball the enemy shoots
    [SerializeField] private float shootInterval = 1f; // Time between shots
    [SerializeField] private float basketballSpeed = 1f; // Speed of the basketball when shot
    [SerializeField] private float approachSpeed = 1f; // Speed at which the enemy moves to the target point
    [SerializeField] private float moveSpeed = 10f; // Speed of circular movement around the center

    public Transform player; // Reference to the player
    private float shootTimer; // Timer to keep track of when to shoot

    private Transform center; // The center point for circular movement
    private float radius; // The movement radius
    private Vector2 targetPoint; // The random point enemy moves to before starting circular movement
    private float angle; // Angle for circular motion
    private float direction; // Determines clockwise or counterclockwise movement

    private bool hasReachedTargetPoint = false; // Checks if enemy reached the initial target point

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindWithTag("Player")?.transform;

        shootTimer = 0f; // Start shooting immediately

        // Find the center point for movement
        center = GameObject.Find("CenterPoint")?.transform;

        // Randomly decide movement direction (clockwise or counterclockwise)
        direction = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;

        // Set a random movement radius within a small range
        radius = Random.Range(3f, 3.3f);

        // Choose a random angle and calculate the target position
        float randomAngle = Random.Range(0f, 360f);
        float targetX = center.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * radius;
        float targetY = center.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * radius;
        targetPoint = new Vector2(targetX, targetY);

        // Initialize angle for circular movement
        angle = randomAngle;
    }

    void Update()
    {
        if (player != null)
        {
            if (!hasReachedTargetPoint)
            {
                MoveToTargetPoint(); // Move to the starting position first
            }
            else
            {
                MoveInCircle(); // Move in a circular pattern around the center
            }

            ShootAtPlayer(); // Keep shooting at the player regardless of movement
        }
    }

    private void MoveToTargetPoint()
    {
        // Get direction and distance to the target point
        Vector2 directionToTarget = (targetPoint - (Vector2)transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, targetPoint);

        // Debugging output to check enemy movement
        Debug.Log("Distance to Target: " + distanceToTarget);
        Debug.Log("Direction: " + directionToTarget);

        if (distanceToTarget > 0.1f) // Move closer if not close enough
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, approachSpeed * Time.deltaTime);
        }
        else
        {
            hasReachedTargetPoint = true; // Once close enough, start circular movement
        }
    }

    private void MoveInCircle()
    {
        // Update angle for circular movement
        angle += direction * moveSpeed * Time.deltaTime;

        // Keep angle within valid range (0 to 360 degrees)
        if (angle >= 360f) angle -= 360f;
        if (angle < 0f) angle += 360f;

        // Calculate new position along the circle
        float x = center.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = center.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        transform.position = new Vector2(x, y);
    }

    private void ShootAtPlayer()
    {
        if (player == null) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            // Create a new basketball and launch it towards the player
            GameObject basketball = Instantiate(basketballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = basketball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 directionToPlayer = (player.position - transform.position).normalized;
                rb.AddForce(directionToPlayer * basketballSpeed, ForceMode2D.Impulse);
            }
            shootTimer = shootInterval; // Reset the shoot timer
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the enemy and give points if hit by a bullet or the player
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(25);
            Destroy(gameObject);
        }
    }
}

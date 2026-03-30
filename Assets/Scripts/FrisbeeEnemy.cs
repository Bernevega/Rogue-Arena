// Ann Bernevega - edited 2.2.2025

using UnityEngine;
using System.Collections;
using System.Linq;

public class FrisbeeEnemy : MonoBehaviour
{
    [SerializeField] private GameObject frisbeePrefab; // The frisbee projectile this enemy will shoot
    [SerializeField] private float moveSpeed = 3f; // Speed at which the enemy moves
    [SerializeField] private float pauseDuration = 1f; // Time the enemy pauses at each point
    [SerializeField] private float shootInterval = 3f; // How often the enemy shoots a frisbee

    private Transform[] spawnPoints; // List of possible spawn locations
    private Transform[] walkingPoints; // List of points the enemy moves between
    public Transform targetPoint; // The current destination the enemy is moving toward
    private bool isPaused = false; // Whether the enemy is currently pausing between movements
    public bool isStanding = false; // Whether the enemy is standing still (e.g., when shooting)
    private float shootTimer = 0f; // Timer to track when the enemy should shoot next

    void Start()
    {
        // Find all spawn and walking points in the scene using their tags
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(obj => obj.transform).ToArray();
        walkingPoints = GameObject.FindGameObjectsWithTag("WalkingPoint").Select(obj => obj.transform).ToArray();

        // If there are no spawn or walking points, log an error and destroy the enemy to avoid issues
        if (spawnPoints.Length == 0 || walkingPoints.Length == 0)
        {
            Debug.LogError("Spawn or walking points missing! Make sure they have the correct tags.");
            Destroy(gameObject);
            return;
        }

        // Debugging: Log the walking point names
        foreach (var point in walkingPoints)
        {
            Debug.Log($"Walking Point: {point.name}");
        }

        // Spawn the enemy at a random spawn point
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        Debug.Log($"Enemy spawned at {transform.position}");

        // Find the nearest walking point and start moving toward it
        targetPoint = FindNearestWalkingPoint();
        if (targetPoint != null)
        {
            Debug.Log($"Enemy is moving towards {targetPoint.name}");
        }
        else
        {
            Debug.LogError("No valid walking points found for the enemy to move to.");
        }
    }

    void Update()
    {
        // If the enemy isn't paused, continue moving toward the target point
        if (!isPaused && targetPoint != null)
        {
            MoveToTarget();
        }

        // Countdown the shoot timer and shoot when it reaches zero
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootFrisbee();
            shootTimer = shootInterval; // Reset the timer for the next shot
        }
    }

    private void MoveToTarget()
    {
        isStanding = false; // The enemy is moving, so it’s not standing still

        if (targetPoint == null) return;

        // Move the enemy toward the target position at a fixed speed
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // If the enemy reaches the target point, pause before moving to the next one
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            Debug.Log($"Enemy reached {targetPoint.name}");
            StartCoroutine(PauseBeforeSwitching()); // Start the pause timer before switching targets
            isStanding = true; // The enemy is now standing still
        }
    }

    private IEnumerator PauseBeforeSwitching()
    {
        isStanding = false; // The enemy is not actively standing during this transition
        isPaused = true; // Mark the enemy as paused
        Debug.Log("Enemy paused for a moment");
        yield return new WaitForSeconds(pauseDuration); // Wait for the pause duration

        // Ensure the targetPoint is still valid before switching to a new one
        if (targetPoint != null)
        {
            targetPoint = FindAlternateWalkingPoint(targetPoint.name); // Find the next point to walk to
            if (targetPoint != null)
            {
                Debug.Log($"Enemy is now moving towards {targetPoint.name}");
            }
            else
            {
                Debug.LogError("No alternate walking point found.");
            }
        }
        else
        {
            Debug.LogError("Target point is null, unable to switch.");
        }

        isPaused = false; // Resume movement
    }

    private Transform FindNearestWalkingPoint()
    {
        // Find the closest walking point to the enemy’s current position
        return walkingPoints.OrderBy(p => Vector2.Distance(transform.position, p.position)).FirstOrDefault();
    }

    public Transform FindAlternateWalkingPoint(string currentPointName)
    {
        Debug.Log($"Finding alternate point for {currentPointName}...");

        // Based on the current walking point, find the next destination
        switch (currentPointName)
        {
            case "WalkingPointUpLeft":
                return walkingPoints.FirstOrDefault(p => p.name == "WalkingPointDownLeft");
            case "WalkingPointUp":
                return walkingPoints.FirstOrDefault(p => p.name == "WalkingPointDown");
            case "WalkingPointUpRight":
                return walkingPoints.FirstOrDefault(p => p.name == "WalkingPointDownRight");
            case "WalkingPointDownRight":
                return walkingPoints.FirstOrDefault(p => p.name == "WalkingPointUpRight");
            case "WalkingPointDown":
                return walkingPoints.FirstOrDefault(p => p.name == "WalkingPointUp");
            case "WalkingPointDownLeft":
                return walkingPoints.FirstOrDefault(p => p.name == "WalkingPointUpLeft");
            default:
                Debug.LogError("Unknown walking point name.");
                return null;
        }
    }

    private void ShootFrisbee()
    {
        isStanding = true; // Enemy stands still while shooting

        // Spawn a new frisbee at the enemy's position
        GameObject frisbeeObj = Instantiate(frisbeePrefab, transform.position, Quaternion.identity);

        // Get the Frisbee script component from the spawned frisbee
        Frisbee frisbee = frisbeeObj.GetComponent<Frisbee>();

        // Ensure the frisbee script is found before modifying its properties
        if (frisbee != null)
        {
            // Set some frisbee properties for speed and arc height
            frisbee.followSpeed = 5f; // Adjust speed if necessary
            frisbee.arcHeight = 2f;
        }
        else
        {
            Debug.LogError("Frisbee component not found!");
        }
    }

    private Vector3 GetPlayerPosition()
    {
        // Find the player in the scene and return their position (default to (0,0,0) if not found)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy is hit by a bullet or collides with the player, it gets destroyed
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Player"))
        {
            Debug.Log("Enemy hit by bullet or player, destroyed.");
            ScoreManager.instance.AddScore(50); // Increase the score when enemy is destroyed
            Destroy(gameObject);
        }
    }
}

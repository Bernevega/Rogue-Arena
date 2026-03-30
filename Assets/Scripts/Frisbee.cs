// Ann Bernevega - edited 2.2.2025

using UnityEngine;

public class Frisbee : MonoBehaviour
{
    public float followSpeed = 1f; // How fast the frisbee moves towards the player
    public float arcHeight = 2f; // Maximum height the frisbee will reach in its arc
    public float rotateSpeed = 10f; // How fast the frisbee spins

    private Vector3 startPosition; // Where the frisbee starts its movement
    private Vector3 targetPosition; // The position where the frisbee is aiming to land
    private float journeyTime = 2f; // How long it takes to reach the target
    private float startTime; // The moment when the frisbee starts moving

    void Start()
    {
        // Find the player in the scene using the "Player" tag
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // If no player is found, log an error and destroy the frisbee to avoid unwanted behavior
        if (player == null)
        {
            Debug.LogError("Player not found! Frisbee will not move correctly.");
            Destroy(gameObject);
            return;
        }

        // Store the starting position (where the frisbee was launched from)
        startPosition = transform.position;

        // Store the player's position at the moment of launch (frisbee won't track live movement)
        targetPosition = player.position;

        // Store the time when movement started
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate how much time has passed since the frisbee started moving
        float elapsedTime = Time.time - startTime;

        // Normalize the progress from 0 (start) to 1 (end)
        float t = elapsedTime / journeyTime;

        // If the frisbee has reached (or exceeded) the journey time, destroy it
        if (t >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        // Move in a straight line from start to target while adding an arc effect
        Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

        // Add vertical movement in an arc shape using a sine wave
        newPosition.y += arcHeight * Mathf.Sin(Mathf.PI * t);

        // Apply the calculated position to the frisbee
        transform.position = newPosition;

        // Adjust rotation speed dynamically: slow at the start and end, fast in the middle
        float dynamicRotationSpeed = Mathf.Lerp(500f, 2000f, Mathf.Sin(Mathf.PI * t));

        // Rotate the frisbee around the Z-axis to create a spinning effect
        transform.Rotate(0, 0, dynamicRotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the frisbee collides with anything that is NOT an enemy, destroy it
        if (!collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

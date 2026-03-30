// Ann Bernevega - edited 4.3.2025

using System.Collections;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] coin; // Array of coin prefabs to spawn

    [SerializeField] float spawnRate = 10f; // Time interval between coin spawns

    public float spawnRange = 4f; // Defines how far coins can spawn from the center
    public float spawnDuration = 50f; // Total duration for which coins will spawn
    public Vector2 centerPoint; // The central position for spawning coins

    private Coroutine spawnRoutine; // Reference to the coroutine handling spawning

    public void StartSpawning()
    {
        StopSpawning(); // Make sure no duplicate coroutines are running
        spawnRoutine = StartCoroutine(SpawnCoinsForTime()); // Start the coin spawning process
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine); // Stop the coroutine if it's running
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnCoinsForTime()
    {
        float elapsedTime = 0f; // Tracks how long we've been spawning coins

        while (elapsedTime < spawnDuration)
        {
            yield return StartCoroutine(SpawnCoins()); // Spawn a coin
            elapsedTime += spawnRate; // Update elapsed time
        }
    }

    private IEnumerator SpawnCoins()
    {
        if (coin.Length == 0) yield break; // If no coin prefabs are assigned, stop

        Debug.Log("Coin spawned"); // Log when a coin spawns

        // Generate a random position within the spawn range
        float x = Random.Range(centerPoint.x - spawnRange, centerPoint.x + spawnRange);
        float y = Random.Range(centerPoint.y - spawnRange, centerPoint.y + spawnRange);
        Vector2 spawnPosition = new Vector2(x, y);

        // Pick a random coin from the array and spawn it
        GameObject spawnedCoin = Instantiate(coin[Random.Range(0, coin.Length)], spawnPosition, Quaternion.identity);

        // Start flickering effect after 5 seconds
        StartCoroutine(FlickerCoin(spawnedCoin));

        // Ensure the coin is visible before flickering starts
        spawnedCoin.SetActive(true);

        Destroy(spawnedCoin, 8f); // Destroy the coin after 8 seconds

        yield return new WaitForSeconds(spawnRate); // Wait before spawning the next coin
    }

    private IEnumerator FlickerCoin(GameObject coinObject)
    {
        float elapsedTime = 0f;
        bool isFlickeringFaster = false; // Controls flicker speed

        // Wait 4 seconds before starting the flickering effect
        yield return new WaitForSeconds(4f);

        while (elapsedTime < 8f)
        {
            // If the coin was destroyed earlier, stop flickering
            if (coinObject == null) yield break;

            // Increase flickering speed after 3 seconds
            if (elapsedTime >= 3f && !isFlickeringFaster)
            {
                isFlickeringFaster = true;
            }

            // Set flicker speed based on elapsed time
            float flickerSpeed = isFlickeringFaster ? 0.3f : 0.6f;

            // Toggle the coin's visibility to create the flickering effect
            coinObject.SetActive(elapsedTime % flickerSpeed < flickerSpeed / 2);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }
    }
}

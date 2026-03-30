// Ann Bernevega - edited 5.2.2025

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;  // Prefab of the enemy to spawn in this wave
        public float spawnInterval = 3f;  // Time between each enemy spawn
        public float waveDuration = 60f;  // Total duration of the wave
    }

    public enum Difficulty { Easy, Medium, Hard }  // Enum to set the difficulty levels
    public Difficulty difficulty = Difficulty.Medium;  // Default difficulty is Medium

    public Wave[] waves;  // Array of waves to spawn
    public Transform[] spawnPoints;  // Array of spawn points for enemies
    public GameObject bonuses;  // GameObject for bonus items
    public TMP_Text timerText;  // UI text to show the timer
    public GameObject bonusTexts;  // UI for bonus selection texts
    public Sprite[] roundBg;  // Background sprites for each wave
    public SpriteRenderer backgroundRenderer;  // Background renderer to change the background
    public static EnemySpawner instance;  // Singleton instance of EnemySpawner
    public int health = 100;  // Health of the end boss
    
    public string enemyTag = "Enemy";  // Tag to identify enemy objects

    private int currentWaveIndex = 0;  // Current wave index
    private float waveTimer;  // Timer for wave duration
    private float spawnTimer;  // Timer for enemy spawn interval
    private bool isSpawning = true;  // Whether enemies should be spawning
    private bool waveEnded = false;  // Whether the current wave has ended
    private bool bonusesActivated = false;  // Whether bonuses are activated

    private CoinSpawn CoinSpawnScript;  // Reference to the CoinSpawn script

    // Audio system for background music and sound effects
    public AudioSource soundtrackAudioSource;
    public AudioSource soundeffectAudioSource;
    public AudioClip[] waveMusic;  // Music for each wave
    public AudioClip bonusSound;  // Sound played when a bonus is selected

    void Start()
    {
        // Set the difficulty based on the selected option from the menu
        difficulty = DifficultyMenu.selectedDifficulty;
        Debug.Log("Selected Difficulty: " + difficulty);

        CoinSpawnScript = FindFirstObjectByType<CoinSpawn>();  // Find the CoinSpawn script in the scene

        bonusTexts.SetActive(false);  // Hide bonus texts at the start
        bonuses.SetActive(false);  // Hide bonuses at the start

        // If there are waves, initialize the timer and start the first wave
        if (waves.Length > 0)
        {
            UpdateTimerText();  // Update the timer text at the start
            timerText.gameObject.SetActive(true);  // Show the timer text
            StartWave(0);  // Start the first wave
        }
    }

    void Update()
    {
        // If all waves are completed, do nothing
        if (currentWaveIndex >= waves.Length) return;

        waveTimer -= Time.deltaTime;  // Update the wave timer
        spawnTimer -= Time.deltaTime;  // Update the spawn timer

        UpdateTimerText();  // Update the displayed timer

        // Spawn enemies at the specified intervals
        if (spawnTimer <= 0f && isSpawning)
        {
            SpawnEnemy();  // Spawn an enemy
            float baseInterval = waves[currentWaveIndex].spawnInterval;  // Base spawn interval

            // Adjust the minimum spawn interval for faster spawns
            float minSpawnInterval = baseInterval / 3.2f;

            float difficultyMultiplier = 1f;  // Multiplier based on difficulty
            switch (difficulty)
            {
                case Difficulty.Easy:
                    difficultyMultiplier = 2f;
                    break;
                case Difficulty.Medium:
                    difficultyMultiplier = 1f;
                    break;
                case Difficulty.Hard:
                    difficultyMultiplier = 0.1f;
                    break;
            }

            // Adjust spawn interval based on wave time and difficulty
            spawnTimer = (waveTimer > 30f) ? Mathf.Lerp(baseInterval * difficultyMultiplier, minSpawnInterval, (waves[currentWaveIndex].waveDuration - waveTimer) / (waves[currentWaveIndex].waveDuration - 30f)) : minSpawnInterval;
        }

        // End the current wave when the timer reaches 0
        if (waveTimer <= 0f && !waveEnded)
        {
            EndCurrentWave();  // End the wave and stop spawning enemies
        }

        CheckIfAllEnemiesDead();  // Check if all enemies are dead to activate bonuses
    }

    void StartWave(int waveIndex)
    {
        // Hide bonus texts and show timer at the start of the wave
        timerText.gameObject.SetActive(true);
        bonusTexts.SetActive(false);

        // If the wave index is out of bounds, stop
        if (waveIndex >= waves.Length)
        {
            Debug.LogWarning("No more waves to start.");
            return;
        }

        // Set the wave timer and spawn timer based on the current wave
        waveTimer = waves[waveIndex].waveDuration;
        spawnTimer = waves[waveIndex].spawnInterval;
        isSpawning = true;  // Enable enemy spawning
        waveEnded = false;  // Mark the wave as not ended
        bonusesActivated = false;  // Bonuses are not activated yet

        ChangeBackground(waveIndex);  // Change the background based on the wave
        PlayWaveMusic(waveIndex);  // Play the music for the current wave

        CoinSpawnScript.StartSpawning();  // Start spawning coins
        AddDifficultyPoints();  // Add points based on the difficulty

        Debug.Log($"Starting Wave {waveIndex + 1}");
    }

    void EndCurrentWave()
    {
        // Stop spawning enemies and mark the wave as ended
        isSpawning = false;
        waveEnded = true;
        timerText.gameObject.SetActive(false);  // Hide the timer
        currentWaveIndex++;  // Move to the next wave

        CoinSpawnScript.StopSpawning();  // Stop coin spawning

        // If there are more waves, wait for the next one, otherwise finish the game
        if (currentWaveIndex < waves.Length)
        {
            Debug.Log("Wave ended. Waiting for next wave.");
        }
        else
        {
            Debug.Log("All waves completed.");
        }
    }

    public void OnBonusSelected()
    {
        // Play the sound when a bonus is selected
        PlayBonusSound();

        bonuses.SetActive(false);  // Hide the bonus items

        // If there are no more waves, do nothing
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("No more waves left to start.");
            return;
        }

        // Start the next wave after selecting a bonus
        StartWave(currentWaveIndex);
    }

    void SpawnEnemy()
    {
        // Randomly choose a spawn point and spawn an enemy at that location
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(waves[currentWaveIndex].enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    void CheckIfAllEnemiesDead()
    {
        // Check if there are no more enemies, and if bonuses haven't been activated yet
        if (GameObject.FindGameObjectsWithTag(enemyTag).Length == 0 && !bonusesActivated && waveEnded)
        {
            bonusTexts.SetActive(true);  // Show the bonus texts
            bonusesActivated = true;  // Mark bonuses as activated
            bonuses.SetActive(true);  // Show the bonuses
            Debug.Log("Activating bonuses.");
        }
    }

    void UpdateTimerText()
    {
        // Update the displayed timer text (minutes:seconds)
        int minutes = Mathf.FloorToInt(waveTimer / 60);
        int seconds = Mathf.FloorToInt(waveTimer % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    void ChangeBackground(int waveIndex)
    {
        // Change the background based on the current wave
        if (backgroundRenderer != null && roundBg.Length > 0)
        {
            backgroundRenderer.sprite = roundBg[waveIndex % roundBg.Length];
        }
    }

    void AddDifficultyPoints()
    {
        // Add points based on the selected difficulty level
        int points = 0;
        switch (difficulty)
        {
            case Difficulty.Easy:
                points = 100;
                break;
            case Difficulty.Medium:
                points = 200;
                break;
            case Difficulty.Hard:
                points = 300;
                break;
        }
        ScoreManager.instance.AddScore(points);  // Add the points to the score manager
    }

    public int GetEndBossHealth(EndBoss endBoss)
    {
        // Set the health of the end boss based on the difficulty level
        switch (difficulty)
        {
            case Difficulty.Easy:
                return health = 50;
            case Difficulty.Medium:
                return health = 100;
            case Difficulty.Hard:
                return health = 200;
            default:
                return health;
        }
    }

    void PlayWaveMusic(int waveIndex)
    {
        // Play the appropriate music for the current wave
        if (soundtrackAudioSource != null && waveMusic.Length > 0)
        {
            int musicIndex = waveIndex % waveMusic.Length;  // Ensure index is within bounds
            if (waveMusic[musicIndex] != null)
            {
                soundtrackAudioSource.clip = waveMusic[musicIndex];
                soundtrackAudioSource.Play();  // Play the selected music
                Debug.Log($"Playing music for Wave {waveIndex + 1}");
            }
            else
            {
                Debug.LogWarning($"No music assigned for Wave {waveIndex + 1}");
            }
        }
    }

    void PlayBonusSound()
    {
        // Play the sound when a bonus is selected
        if (bonusSound != null && soundeffectAudioSource != null)
        {
            soundeffectAudioSource.PlayOneShot(bonusSound);  // Play the bonus selection sound
            Debug.Log("Bonus sound played.");
        }
        else
        {
            Debug.LogWarning("Bonus sound not assigned.");
        }
    }
}
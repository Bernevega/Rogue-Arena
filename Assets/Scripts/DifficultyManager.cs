// Ann Bernevega - edited 2.3.2025

using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Enum to represent the different difficulty levels
    public enum DifficultyLevel { Easy, Normal, Hard }
    public static DifficultyManager instance; // Singleton instance of DifficultyManager

    // Current selected difficulty (default is Normal)
    public DifficultyLevel currentDifficulty = DifficultyLevel.Normal;
    
    // Multipliers for spawn rates based on difficulty levels
    public float easySpawnMultiplier = 1.5f;
    public float normalSpawnMultiplier = 1f;
    public float hardSpawnMultiplier = 0.7f;

    // Score bonuses for each difficulty level
    public int easyScoreBonus = 10;
    public int normalScoreBonus = 20;
    public int hardScoreBonus = 30;

    private void Awake()
    {
        // Ensure only one instance of DifficultyManager exists (Singleton pattern)
        if (instance == null)
        {
            instance = this; // Assign the instance
            DontDestroyOnLoad(gameObject); // Keep the object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Returns the spawn multiplier based on the current difficulty level
    public float GetSpawnMultiplier()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                return easySpawnMultiplier; // Return multiplier for Easy difficulty
            case DifficultyLevel.Normal:
                return normalSpawnMultiplier; // Return multiplier for Normal difficulty
            case DifficultyLevel.Hard:
                return hardSpawnMultiplier; // Return multiplier for Hard difficulty
            default:
                return 1f; // Default multiplier if something goes wrong
        }
    }

    // Returns the score bonus based on the current difficulty level
    public int GetScoreBonus()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                return easyScoreBonus; // Return score bonus for Easy difficulty
            case DifficultyLevel.Normal:
                return normalScoreBonus; // Return score bonus for Normal difficulty
            case DifficultyLevel.Hard:
                return hardScoreBonus; // Return score bonus for Hard difficulty
            default:
                return 0; // Default score bonus if something goes wrong
        }
    }

    // Set the difficulty level (can be called to change the difficulty)
    public void SetDifficulty(DifficultyLevel difficulty)
    {
        currentDifficulty = difficulty; // Set the new difficulty level
    }
}
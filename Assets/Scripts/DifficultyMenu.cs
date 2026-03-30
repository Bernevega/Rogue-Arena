// Ann Bernevega - edited 2.3.2025

using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    // Store selected difficulty globally to be accessed by other parts of the game
    public static EnemySpawner.Difficulty selectedDifficulty = EnemySpawner.Difficulty.Medium;

    // Called when the "Easy" difficulty button is pressed
    public void EasyDifficulty()
    {
        selectedDifficulty = EnemySpawner.Difficulty.Easy; // Set the difficulty to Easy
        StartGame(); // Start the game with the selected difficulty
    }

    // Called when the "Normal" difficulty button is pressed
    public void NormalDifficulty()
    {
        selectedDifficulty = EnemySpawner.Difficulty.Medium; // Set the difficulty to Medium (Normal)
        StartGame(); // Start the game with the selected difficulty
    }

    // Called when the "Hard" difficulty button is pressed
    public void HardDifficulty()
    {
        selectedDifficulty = EnemySpawner.Difficulty.Hard; // Set the difficulty to Hard
        StartGame(); // Start the game with the selected difficulty
    }

    // Loads the "Wave" scene where the EnemySpawner is located
    private void StartGame()
    {
        SceneManager.LoadScene("Wave"); // Load the next scene where enemies will spawn based on the difficulty
    }
}

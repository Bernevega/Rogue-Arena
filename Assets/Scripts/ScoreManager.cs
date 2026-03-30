// Juho Karjalainen - edited 17.2.2025
// Ann Bernevega - edited 19.2.2025

using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance of the ScoreManager

    public int score = 0; // Current score that will be updated during gameplay
    public TMP_Text scoreText; // UI Text component to display the score

    private HighscoreManager highscoreManager; // Reference to HighscoreManager for handling high scores

    void Awake()
    {
        // Ensure only one instance of ScoreManager exists using Singleton pattern
        if (instance == null)
        {
            instance = this; // Set the instance to the current object
            DontDestroyOnLoad(gameObject); // Keep this object between scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy any extra instances to maintain only one
        }
    }

    void Start()
    {
        // Find the HighscoreManager object in the scene (updated method)
        highscoreManager = FindFirstObjectByType<HighscoreManager>(); 
        UpdateScoreText(); // Initial update of the score UI
    }

    // Method to add points to the current score
    public void AddScore(int amount)
    {
        score += amount; // Increase the score by the specified amount
        UpdateScoreText(); // Update the score display in the UI

        // If the HighscoreManager is available, update the high score
        if (highscoreManager != null)
        {
            highscoreManager.UpdateHighscore(score); // Set the new high score
        }
    }

    // Method to subtract points from the score (ensuring it doesn't go below zero)
    public void SubtractScore(int amount)
    {
        score = Mathf.Max(0, score - amount); // Prevent the score from going negative
        UpdateScoreText(); // Update the UI to reflect the new score
    }

    // Method to reset the score to zero
    public void ResetScore()
    {
        score = 0; // Set the score to zero
        UpdateScoreText(); // Update the displayed score to show zero
    }

    // Method to update the score displayed on the screen
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString(); // Convert score to string and update the UI text
    }
}

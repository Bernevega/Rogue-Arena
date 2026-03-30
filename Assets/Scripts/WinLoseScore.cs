// Juho Karjalainen - edited 3.2.2025
// Ann Bernevega - edited 19.2.2025

using UnityEngine;
using TMPro;
using System.Collections;

public class WinLoseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI element displaying the score
    [SerializeField] private TextMeshProUGUI highscoreText; // Reference to the UI element displaying the highscore
    public GameObject menuCanvas; // Reference to the menu canvas that will display the win/lose menu

    void Start()
    {
        menuCanvas.SetActive(false); // Hide the menu canvas at the start
        StartCoroutine(ShowMenuWithDelay()); // Start coroutine to show the menu after a short delay
    }

    // Coroutine to show the win/lose menu after a delay
    IEnumerator ShowMenuWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before showing the menu
        DisplayScores(); // Display the current score and highscore
        menuCanvas.SetActive(true); // Enable the menu canvas to show the menu
    }

    // Method to update the UI with the current score and highscore
    void DisplayScores()
    {
        // Check if the ScoreManager instance is available
        if (ScoreManager.instance != null)
        {
            scoreText.text = "Your Score: " + ScoreManager.instance.score.ToString(); // Display current score
        }

        // Find the HighscoreManager instance and display the highscore
        HighscoreManager highscoreManager = FindFirstObjectByType<HighscoreManager>(); // Updated method
        if (highscoreManager != null)
        {
            highscoreText.text = "Highscore: " + highscoreManager.GetHighscore().ToString(); // Display highscore
        }
    }
}

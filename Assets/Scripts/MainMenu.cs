// Juho Karjalainen - edited 3.2.2025
// Ann Bernevega - edited 2.3.2025

using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // References to UI elements for different menu screens
    public GameObject difficultyCanvas; // The canvas for selecting difficulty
    public GameObject mainMenuCanvas; // The main menu canvas

    // Function to start the game by opening the difficulty selection menu
    public void Playgame()
    {
        difficultyCanvas.SetActive(true); // Show the difficulty selection screen
        mainMenuCanvas.SetActive(false); // Hide the main menu
    }

    // Function to quit the game
    public void Quitgame()
    {
        Debug.Log("QuitGame"); // Logs quit action for debugging

        Application.Quit(); // Exits the game (only works in a built application, not in the Unity Editor)
    }
}

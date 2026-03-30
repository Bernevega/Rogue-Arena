// Juho Karjalainen - edited 3.2.2025

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    // Function to restart the game
    public void Playgame()
    {       
        SceneManager.LoadScene("Wave"); // Loads the scene named "Wave"
    }

    // Function to return to the main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Loads the scene named "Main Menu"
    }

    // Function to quit the game
    public void Quitgame()
    {
        Application.Quit(); // Quits the game
    }
}

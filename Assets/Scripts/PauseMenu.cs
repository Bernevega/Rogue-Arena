// Juho Karjalainen - edited 28.1.2025

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false; // Keeps track of whether the game is paused
    public GameObject pauseMenuUI; // UI panel for the pause menu
    public GameObject optionsMenuUI; // UI panel for the options menu

    void Start()
    {
        // Ensure the game starts in an unpaused state
        Resume();
    }

    void Update()
    {
        // Listen for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenuUI.activeSelf)
            {
                // If the options menu is open, close it
                CloseOptionsMenu();

                // If the game was paused, resume it after closing the options menu
                if (isPaused)
                {
                    Resume();
                }
            }
            else if (isPaused)
            {
                // If the game is already paused and no other menus are open, resume it
                Resume();
            }
            else
            {
                // Otherwise, pause the game
                Pause();
            }
        }
    }

    public void Pause()
    {
        // Show the pause menu and hide the options menu
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 0.0f; // Stop in-game time
        isPaused = true; // Mark the game as paused
    }

    public void Resume()
    {
        // Hide all menus and resume the game
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1.0f; // Resume in-game time
        isPaused = false; // Mark the game as unpaused
    }

    public void ReturnToMainMenu()
    {
        // Load the main menu scene (assumed to be scene index 0)
        Time.timeScale = 1.0f; // Ensure time is running before changing scenes
        SceneManager.LoadScene(0);
    }

    public void OpenOptionsMenu()
    {
        // Show the options menu and hide the pause menu
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        // Show the pause menu again and hide the options menu
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}

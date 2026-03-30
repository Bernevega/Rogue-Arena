// Ann Bernevega - edited 19.2.2025

using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad instance; // Singleton instance to ensure only one instance of this script
    public GameObject scoreCanvas; // Reference to the scoreCanvas GameObject

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this; // Assign this instance to the singleton
            DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed when loading a new scene
            
            // Find and reference the scoreCanvas in the scene
            scoreCanvas = GameObject.Find("ScoreCanvas");
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event to update canvas state when a scene is loaded
        }
        else
        {
            Destroy(gameObject); // Destroy this GameObject if an instance already exists
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene loaded event when this object is destroyed
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the state of the scoreCanvas whenever a new scene is loaded
        UpdateScoreCanvasActiveState();
    }

    void UpdateScoreCanvasActiveState()
    {
        if (scoreCanvas != null) // Check if the scoreCanvas reference is not null
        {
            string currentSceneName = SceneManager.GetActiveScene().name; // Get the name of the current scene
            Debug.Log($"Current Scene: {currentSceneName}"); // Log the name of the current scene

            if (currentSceneName == "Wave")
            {
                scoreCanvas.GetComponent<Canvas>().enabled = true; // Enable the scoreCanvas if we're in the "Wave" scene
                Debug.Log("scoreCanvas activated."); // Log that the scoreCanvas was activated
            }
            else
            {
                scoreCanvas.GetComponent<Canvas>().enabled = false; // Disable the scoreCanvas if we're not in the "Wave" scene
                Debug.Log("scoreCanvas deactivated."); // Log that the scoreCanvas was deactivated
            }
        }
        else
        {
            Debug.LogWarning("scoreCanvas reference is null."); // Log a warning if scoreCanvas is not found
        }
    }
}

// Juho Karjalainen - edited 4.2.2025

using UnityEngine;
using TMPro;

public class ScreenOptions : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Reference to the TMP_Dropdown for screen mode selection in the UI

    private void Start()
    {     
        // Add a listener to the dropdown to call OnDropdownValueChanged when a new value is selected
        resolutionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Load the previously saved screen mode from PlayerPrefs (default to fullscreen if not found)
        int savedMode = PlayerPrefs.GetInt("ScreenMode", 0); // Default to fullscreen (mode 0)
        resolutionDropdown.value = savedMode; // Set the dropdown value to the saved mode
    }

    // Method to change the screen mode based on the selected mode
    public void SetScreenMode(int mode)
    {
        Debug.Log("Screen mode changed to: " + mode); // Log the screen mode change for debugging

        // Check the mode and apply the corresponding screen resolution and fullscreen setting
        switch (mode)
        {
            case 0: // Fullscreen
                // Set the screen to the current resolution in fullscreen mode
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
                break;
            case 1: // Borderless
                // Set the screen to a maximized window (borderless)
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.MaximizedWindow);
                break;
            case 2: // Windowed
                // Set the screen to windowed mode with a resolution of 1280x720
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
        }

        // Save the chosen screen mode to PlayerPrefs so it can be remembered next time
        PlayerPrefs.SetInt("ScreenMode", mode);
        PlayerPrefs.Save(); // Ensure the setting is saved immediately
    }

    // This method is called whenever the dropdown value changes
    private void OnDropdownValueChanged(int mode)
    {
        SetScreenMode(mode); // Call SetScreenMode with the selected mode
    }
}

// Juho Karjalainen - edited 11.2.2025

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer; // Reference to the audio mixer
    public Slider musicSlider; // Public slider for music volume, assigned in the Inspector
    public Slider soundSlider; // Public slider for sound effects volume, assigned in the Inspector

    void Start()
    {
        // Initialize sliders and load saved volume settings
        if (musicSlider != null && soundSlider != null)
        {
            // Check if saved player preferences exist for volume
            if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("soundsVolume"))
            {
                LoadVolume(); // Load saved volume preferences
            }
            else
            {
                // Set default volume values if no preferences are saved
                SetMusicVolume();
                SetSoundVolume();
            }

            // Add listeners to sliders to update volume when changed
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            soundSlider.onValueChanged.AddListener(delegate { SetSoundVolume(); });
        }
        else
        {
            // Log a warning if the sliders have not been assigned in the Inspector
            Debug.LogWarning("Sliders are not assigned in the Inspector!");
        }
    }

    // Method to update the music volume based on slider value
    public void SetMusicVolume()
    {
        if (musicSlider == null) return; // Ensure the slider is assigned
        float volume = musicSlider.value;

        // Convert slider value to decibels, avoiding log of zero
        float volumeInDecibels = volume == 0f ? -80f : Mathf.Log10(volume) * 20;

        myMixer.SetFloat("Music", volumeInDecibels); // Set the audio mixer volume
        PlayerPrefs.SetFloat("musicVolume", volume); // Save the music volume setting
    }

    // Method to update the sound effects volume based on slider value
    public void SetSoundVolume()
    {
        if (soundSlider == null) return; // Ensure the slider is assigned
        float volume = soundSlider.value;

        // Convert slider value to decibels, avoiding log of zero
        float volumeInDecibels = volume == 0f ? -80f : Mathf.Log10(volume) * 20;

        myMixer.SetFloat("Sounds", volumeInDecibels); // Set the audio mixer volume
        PlayerPrefs.SetFloat("soundsVolume", volume); // Save the sound effects volume setting
    }

    // Method to load saved volume settings from PlayerPrefs
    private void LoadVolume()
    {
        if (musicSlider != null && soundSlider != null)
        {
            // Retrieve and apply saved volume values to sliders
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            soundSlider.value = PlayerPrefs.GetFloat("soundsVolume");

            // Update audio mixer based on loaded values
            SetMusicVolume();
            SetSoundVolume();
        }
    }
}

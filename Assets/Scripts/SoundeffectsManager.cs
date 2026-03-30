// Ann Bernevega - edited 5.3.2025

using UnityEngine;

public class SoundeffectsManager : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component that will play sounds
    public AudioClip coinPickupSound; // Sound to play when the player picks up a coin

    // This method is called when another collider collides with this GameObject's collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "Coins"
        if (collision.gameObject.CompareTag("Coins"))
        {
            // If it's a coin, play the coin pickup sound
            PlayCoinPickupSound();
        }
    }

    // Method to play the coin pickup sound
    void PlayCoinPickupSound()
    {
        // Ensure that both the sound clip and audio source are set before playing the sound
        if (coinPickupSound != null && audioSource != null)
        {
            // Play the coin pickup sound once using the audio source
            audioSource.PlayOneShot(coinPickupSound); 
        }
    }
}

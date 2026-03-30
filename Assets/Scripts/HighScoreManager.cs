// Juho Karjalainen - edited 17.2.2025
// Ann Bernevega - edited 19.2.2025

using System;
using System.IO;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;

public class HighscoreManager : MonoBehaviour
{
    public TMP_Text highscoreText; // UI text element to display the high score
    private int highscore = 0; // Variable to store the highest score
    private string filePath; // Path to save the high score data

    private void Start()
    {
        // Set the file path where the high score data will be stored
        filePath = Application.persistentDataPath + "/highscore.dat";
        
        LoadHighscore(); // Load the saved high score when the game starts
        DisplayHighscore(); // Update the UI to reflect the current high score
    }

    public void UpdateHighscore(int currentScore)
    {
        // Only update the high score if the new score is higher than the previous one
        if (currentScore > highscore)
        {
            highscore = currentScore; // Set the new high score
            SaveHighscore(highscore); // Save the new high score to file
            DisplayHighscore(); // Update the displayed high score
        }
    }

    // Save the high score to a file
    public void SaveHighscore(int score)
    {
        BinaryFormatter formatter = new BinaryFormatter(); // Create a binary formatter for serialization
        FileStream file = File.Create(filePath); // Create a file at the specified path
        HighscoreData data = new HighscoreData(score); // Create an object to store the high score
        formatter.Serialize(file, data); // Serialize and save the data to the file
        file.Close(); // Close the file to prevent data corruption
        Debug.Log("Highscore saved: " + score); // Log the saved high score
    }

    // Load the high score from a file
    private void LoadHighscore()
    {
        // Check if the high score file exists
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter(); // Create a binary formatter for deserialization
            FileStream file = File.Open(filePath, FileMode.Open); // Open the existing file
            HighscoreData data = (HighscoreData)formatter.Deserialize(file); // Read and deserialize the data
            highscore = data.highscore; // Assign the loaded high score
            file.Close(); // Close the file after reading
            Debug.Log("Highscore loaded: " + highscore); // Log the loaded high score
        }
        else
        {
            highscore = 0; // Default to 0 if no saved high score is found
            Debug.Log("No saved highscore found, starting at 0."); // Log that no high score was found
        }
    }

    // Display the high score in the UI
    public void DisplayHighscore()
    {
        if (highscoreText != null)
        {
            highscoreText.text = highscore.ToString(); // Update the UI text with the high score
        }
    }

    // Class to store high score data for serialization
    [Serializable]
    public class HighscoreData
    {
        public int highscore; // Variable to hold the saved high score

        public HighscoreData(int score)
        {
            highscore = score; // Assign the score value when creating the object
        }
    }

    // Return the current high score
    public int GetHighscore()
    {
        return highscore;
    }
}

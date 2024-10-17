using UnityEngine;
using TMPro; // For TextMeshPro

public class WordGame : MonoBehaviour
{
    public TMP_Text wordDisplay; // UI text element to show the word
    private string targetWord = ""; // The word the player has to type
    private string playerInput = ""; // Player's input

    private bool isGameActive = false; // Tracks if the word game is active


    void Start()
    {
        // Other initialization code...

        // Automatically find the WordGame component
        var wordGame = FindObjectOfType<WordGame>();
        if (wordGame == null)
        {
            Debug.LogError("WordGame is not assigned! Make sure there's a WordGame component in the scene.");
        }
    }


    // Call this method to start the word mini-game
    public void StartWordGame(string word)
    {
        targetWord = word;
        playerInput = "";
        wordDisplay.text = word;
        isGameActive = true; // Mark the game as active
    }

    // Call this method to end the word mini-game
    public void EndWordGame()
    {
        wordDisplay.text = ""; // Clear the word from the screen
        isGameActive = false; // Mark the game as inactive
    }

    void Update()
    {
        if (isGameActive)
        {
            foreach (char c in Input.inputString)
            {
                playerInput += c; // Add each character the player types to their input

                // If the player's input matches the target word, the game is complete
                if (playerInput == targetWord)
                {
                    Debug.Log("Correct Word! Mini-game passed.");
                    EndWordGame(); // End the word game
                }
                // If the player's input exceeds the word, reset
                else if (playerInput.Length > targetWord.Length)
                {
                    Debug.Log("Wrong Word! Try Again.");
                    playerInput = ""; // Reset player input
                }
            }
        }
    }
}

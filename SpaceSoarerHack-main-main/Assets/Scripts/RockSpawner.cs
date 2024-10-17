using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockThemes; // Array to store different rock/asteroid themes
    public int numberOfRocks = 5;   // Number of rocks to spawn
    public float spacing = 2f;      // Space between each rock
    public Transform spawnPoint;    // Assign the starting spawn point in the Inspector

    private int currentThemeIndex = 0; // Index to track the current rock theme

    public void SpawnRocks()
    {
        if (rockThemes == null || rockThemes.Length == 0 || spawnPoint == null)
        {
            Debug.LogError("Rock Themes or Spawn Point not assigned.");
            return;
        }

        // Ensure valid theme index
        currentThemeIndex = Mathf.Clamp(currentThemeIndex, 0, rockThemes.Length - 1);

        GameObject selectedRockPrefab = rockThemes[currentThemeIndex]; // Choose the current rock theme

        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector3 spawnPosition = spawnPoint.position + new Vector3(i * spacing, 0, 0);
            Instantiate(selectedRockPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Method to change the current rock theme
    public void ChangeRockTheme(int newThemeIndex)
    {
        if (newThemeIndex >= 0 && newThemeIndex < rockThemes.Length)
        {
            currentThemeIndex = newThemeIndex;  // Update to the new theme index
        }
        else
        {
            Debug.LogWarning("Invalid rock theme index.");
        }
    }
}

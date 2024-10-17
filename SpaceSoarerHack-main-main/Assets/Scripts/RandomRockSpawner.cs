using System.Collections.Generic;
using UnityEngine;

public class RandomRockSpawner : MonoBehaviour
{
    public GameObject[] rockThemes; // Array to store different rock/asteroid themes
    public float spawnInterval = 1f; // Time interval between spawns
    public float rockSpeed = 2.0f; // Speed at which rocks move from right to left
    public int maxRocks = 20; // Maximum number of rocks on screen at one time
    public Transform spawnPoint;
    public int startFrameCount = 300; // Number of frames to wait before starting to spawn rocks

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private float timer = 0f;
    private int currentThemeIndex = 0; // Track the current rock theme

    void Start()
    {
        // You can initialize here if necessary.
    }

    void Update()
    {
        if (startFrameCount > 0)
        {
            startFrameCount--;
            return;
        }

        // Timer to control spawn interval
        timer += Time.deltaTime;
        if (timer >= spawnInterval && spawnedRocks.Count < maxRocks)
        {
            SpawnRock();
            timer = 0f;
        }

        // Move the spawned rocks from right to left
        foreach (GameObject rock in spawnedRocks)
        {
            if (rock != null)
            {
                rock.transform.Translate(Vector3.left * rockSpeed * Time.deltaTime);
            }
        }

        // Destroy rocks when they go off-screen
        float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).x;
        for (int i = spawnedRocks.Count - 1; i >= 0; i--)
        {
            if (spawnedRocks[i].transform.position.x < screenLeft)
            {
                Destroy(spawnedRocks[i]);
                spawnedRocks.RemoveAt(i);
            }
        }
    }

    void SpawnRock()
    {
        if (rockThemes == null || rockThemes.Length == 0)
        {
            Debug.LogError("Rock Themes not assigned.");
            return;
        }

        // Determine a random spawn position off-screen to the right
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y;
        float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y;
        float spawnY = Random.Range(screenBottom, screenTop);
        Vector3 spawnPosition = spawnPoint.position;
        spawnPosition.y = spawnY;

        // Select the current rock theme
        GameObject selectedRockPrefab = rockThemes[currentThemeIndex];

        GameObject spawnedRock = Instantiate(selectedRockPrefab, spawnPosition, Quaternion.identity);
        spawnedRock.SetActive(true);

        // Add the spawned rock to the list
        spawnedRocks.Add(spawnedRock);
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

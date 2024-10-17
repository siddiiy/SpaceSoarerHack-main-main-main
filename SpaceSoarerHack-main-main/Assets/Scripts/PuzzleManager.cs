using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float scrollSpeed = 0.5f;
    public float rockSpeed = 1.0f;   // Speed at which rocks move from right to left
    private float offset;
    private Material mat;
    private float distanceMoved = 0f;
    public float spawnDistance = 10f;
    public TextMeshProUGUI puzzleText;
    public TextMeshProUGUI rockText;

    public GameObject rockPrefab;
    public int numberOfRocks = 5; // Number of rocks to spawn
    public float spacing = 2f;    // Space between each rock
    public Transform spawnPoint;

    private int correctAnswer;
    //private bool questionDisplayed = false;
    //private bool rocksSpawned = false;
    private List<GameObject> spawnedRocks = new List<GameObject>();

    public Transform spawnStart;


    private int frameCounter = 0;
    public int framesPerSpawn = 600; // The number of frames between each spawn cycle

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        if (puzzleText != null)
        {
            puzzleText.gameObject.SetActive(false); // text is initially hidden
        }
        else
        {
            Debug.LogError("PuzzleText is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));

        distanceMoved += Time.deltaTime * scrollSpeed * 10f;

        // Increment the frame counter
        frameCounter++;

        // Check if it's time to spawn the question and rocks
        if (frameCounter >= framesPerSpawn)
        {
            GeneratePuzzle();
            SpawnRocks();
            frameCounter = 0; // Reset the frame counter
        }

        // Move the spawned rocks from right to left
        foreach (GameObject rock in spawnedRocks)
        {
            if (rock != null)
            {
                rock.transform.Translate(Vector3.left * rockSpeed * Time.deltaTime);
            }
        }
    }

    void GeneratePuzzle()
    {
        if (puzzleText != null)
        {
            int num1 = Random.Range(1, 10);
            int num2 = Random.Range(1, 10);
            correctAnswer = num1 + num2;
            puzzleText.text = $"{num1} + {num2} = ?";
            puzzleText.gameObject.SetActive(true);
            // Debug.Log("Puzzle displayed: " + puzzleText.text); 

            // Debug.Log("TextMeshProUGUI component state: " + puzzleText.text);
        }
        else
        {
            Debug.LogError("PuzzleText is not assigned in the Inspector.");
        }
    }

    public void HidePuzzleText()
    {
        if (puzzleText != null)
        {
            puzzleText.gameObject.SetActive(false);
        }
    }

    void SpawnRocks()
    {
        if (rockPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Rock Prefab or Spawn Point not assigned.");
            return;
        }

        // Clear the list of spawned rocks
        spawnedRocks.Clear();

        // Determine the correct answer position
        int correctAnswerIndex = Random.Range(0, numberOfRocks);

        for (int i = 0; i < numberOfRocks; i++)
        {
            // Adjust the y-coordinate for column layout
            Vector3 spawnPosition = spawnPoint.position + new Vector3(0, i * spacing, 0);


            GameObject spawnedRock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity, spawnStart);
            spawnedRock.SetActive(true); // Ensure the spawned rock is active

            // Add the spawned rock to the list
            spawnedRocks.Add(spawnedRock);


            RockDespawner rockDespawner = spawnedRock.AddComponent<RockDespawner>();
            rockDespawner.Initialize(this);

            // Set the text on the rock
            TextMeshProUGUI rockText = spawnedRock.GetComponentInChildren<TextMeshProUGUI>();
            if (rockText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found in the spawned rock.");
            }
            else
            {
                if (i == correctAnswerIndex)
                {
                    rockText.text = correctAnswer.ToString(); // Assign the correct answer
                    spawnedRock.tag = "CorrectAnswer"; // to the correct answer rock
                }
                else
                {
                    rockText.text = Random.Range(1, 20).ToString(); // Assign a random number
                }
                // Debug.Log("Rock spawned at position: " + spawnPosition + " with text: " + rockText.text);
            }


        }
    }

    public void OnRockDestroyed(GameObject rock)
    {
        // Remove the rock from the list of spawned rocks
        spawnedRocks.Remove(rock);

        // If all rocks are destroyed, hide the puzzle text
        if (spawnedRocks.Count == 0)
        {
            HidePuzzleText();
        }
    }
}

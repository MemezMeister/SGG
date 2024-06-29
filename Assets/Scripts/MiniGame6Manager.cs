using UnityEngine;
using System.Collections;

public class MiniGame6Manager : MonoBehaviour, IMiniGameManager
{
    public GameObject climbingLatchPrefab;
    public Transform climbingLatchSpawnPoint;
    public GameObject workerPrefab;
    public Transform workerSpawnPoint;
    public GameObject ground;
    public float timeLimit = 10f;
    public MiniGameUIManager uiManager; // Reference to the UI manager

    private GameObject currentClimbingLatch;
    private GameObject currentWorker;
    private float timer;
    private bool gameActive = false; // Updated to false
    private bool gameOver = false;

    void Start()
    {
        timer = timeLimit;
        if (uiManager != null)
        {
            uiManager.SetTimer(timer);
            uiManager.SetLives(GameManager.instance.lives);
        }
        SpawnWorker();
        SpawnClimbingLatch();
    }

    void Update()
    {
        if (!gameActive || gameOver) return; // Updated to only run if game is active and not over

        timer -= Time.deltaTime;
        if (uiManager != null)
        {
            uiManager.SetTimer(timer); // Update timer in UI
        }
        if (timer <= 0)
        {
            GameOver(false);
        }
    }

    public void StartGame()
    {
        gameActive = true; // Start the game when this method is called
    }

    public void EndGame()
    {
        gameActive = false; // Implementation for EndGame
    }

    private void SpawnWorker()
    {
        currentWorker = Instantiate(workerPrefab, workerSpawnPoint.position, Quaternion.identity);
        currentWorker.transform.SetParent(transform);
        currentWorker.tag = "Worker"; // Ensure the worker is tagged for detection
    }

    private void SpawnClimbingLatch()
    {
        currentClimbingLatch = Instantiate(climbingLatchPrefab, climbingLatchSpawnPoint.position, Quaternion.identity);
        currentClimbingLatch.transform.SetParent(transform);

        // Ensure the ClimbingLatch script is correctly configured
        ClimbingLatch climbingLatchScript = currentClimbingLatch.GetComponent<ClimbingLatch>();
        if (climbingLatchScript != null)
        {
            climbingLatchScript.SetMiniGame6Manager(this);
        }
        else
        {
            Debug.LogError("Climbing latch prefab does not have a ClimbingLatch script attached.");
        }
    }

    public void GameOver(bool won)
    {
        if (gameOver) return; // Ensure GameOver is only called once
        gameOver = true;

        gameActive = false;
        if (won)
        {
            Debug.Log("You won!");
            GameManager.instance.MiniGameCompleted();
        }
        else
        {
            Debug.Log("You lost!");
            GameManager.instance.LoseLife();
            StartCoroutine(HandleGround());
        }
    }

    private IEnumerator HandleGround()
    {
        ground.GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(3);
        ground.GetComponent<Collider2D>().isTrigger = false;
        GameManager.instance.MiniGameCompleted();
    }
        public void ResetGame()
    {
        // Reset game logic
        // Destroy spawned assets and reset any game-specific variables
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

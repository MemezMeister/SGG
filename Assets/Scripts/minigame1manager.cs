using UnityEngine;
using System.Collections;

public class MiniGame1Manager : MonoBehaviour, IMiniGameManager
{
    public GameObject workerWithoutHatPrefab;
    public GameObject workerWithHatPrefab;
    public GameObject brickPrefab;
    public Transform workerSpawnPoint;
    public Transform brickSpawnPoint;
    public GameObject[] hats;
    public string correctHatName;
    public float timeLimit = 5f;
    public MiniGameUIManager uiManager; // Reference to the UI manager

    private GameObject currentWorker;
    private GameObject currentBrick;
    private float timer;
    private bool gameWon = false;

    private void Start()
    {
        StartGame(); // Start the game on initialization
    }

    private void Update()
    {
        if (gameWon) return;

        timer -= Time.deltaTime;
        uiManager.SetTimer(timer); // Update timer in UI
        if (timer <= 0 && !gameWon)
        {
            DropBrick();
            uiManager.SetLives(uiManager.lives - 1); // Update lives in UI
            GameManager.instance.LoseLife();
            StartCoroutine(ProceedToNextMiniGame());
        }
    }

    public void HatClicked(string hatName)
    {
        if (gameWon) return; // Prevent multiple clicks
        DropBrick();
        if (hatName == correctHatName)
        {
            gameWon = true;
            timer = 0; // Pause the timer
            uiManager.SetTimer(timer); // Update timer in UI
            ReplaceWorker(true);
            Debug.Log("You chose the correct hat!");
            GameManager.instance.AddScore(1); // Add score
            StartCoroutine(ProceedToNextMiniGame());
        }
        else
        {
            gameWon = true; // Prevent further updates
            timer = 0; // Pause the timer
            uiManager.SetTimer(timer); // Update timer in UI
            Debug.Log("Wrong hat, you lose.");
            GameManager.instance.LoseLife();
            StartCoroutine(ProceedToNextMiniGame());
        }
    }

    private void SpawnWorker(bool withHat)
    {
        if (currentWorker != null)
        {
            Destroy(currentWorker);
        }

        GameObject workerPrefab = withHat ? workerWithHatPrefab : workerWithoutHatPrefab;
        currentWorker = Instantiate(workerPrefab, workerSpawnPoint.position, Quaternion.identity);
        currentWorker.transform.SetParent(transform); // Set parent to MiniGame1Manager
        Debug.Log("Spawned worker at: " + workerSpawnPoint.position + ", World position: " + currentWorker.transform.position);

        if (withHat)
        {
            Rigidbody2D rb = currentWorker.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void ReplaceWorker(bool withHat)
    {
        Destroy(currentWorker);
        SpawnWorker(withHat);
    }

    private void SpawnHats()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            GameObject spawnPoint = GameObject.Find("HatSpawnPoint" + (i + 1));
            if (spawnPoint != null)
            {
                hats[i].transform.position = spawnPoint.transform.position;
                hats[i].transform.SetParent(transform); // Set parent to MiniGame1Manager
                Debug.Log("Spawned hat " + (i + 1) + " at: " + hats[i].transform.position);
            }
            else
            {
                Debug.LogError("Spawn point HatSpawnPoint" + (i + 1) + " not found.");
            }
        }
    }

    private void SpawnBrick()
    {
        currentBrick = Instantiate(brickPrefab, brickSpawnPoint.position, Quaternion.identity);
        currentBrick.transform.SetParent(transform); // Set parent to MiniGame1Manager
        Rigidbody2D rb = currentBrick.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Debug.Log("Spawned brick at: " + brickSpawnPoint.position + ", World position: " + currentBrick.transform.position);
    }

    private void DropBrick()
    {
        if (currentBrick != null)
        {
            Rigidbody2D rb = currentBrick.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
        }
    }

    private IEnumerator ProceedToNextMiniGame()
    {
        GameManager.instance.MiniGameCompleted();
        yield return new WaitForSeconds(2);
        Destroy(currentWorker);
        Destroy(currentBrick);

    }

    public void ResetGame()
    {

        Debug.Log("Game reset triggered");
        gameWon = false;
        StartGame(); 
    }

    public void StartGame()
    {
        Debug.Log("Game reset starts");
        timer = timeLimit;
        if (workerWithoutHatPrefab == null || workerWithHatPrefab == null || brickPrefab == null || workerSpawnPoint == null || brickSpawnPoint == null)
        {
            Debug.LogError("One or more references are not assigned in the MiniGame1Manager.");
            return;
        }
        uiManager.SetTimer(timer);
        uiManager.SetLives(GameManager.instance.lives);
        SpawnWorker(false);
        SpawnHats();
        SpawnBrick();
    }

    public void EndGame()
    {
    }
}

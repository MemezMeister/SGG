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
    public MiniGameUIManager uiManager; 

    private GameObject currentClimbingLatch;
    private GameObject currentWorker;
    private float timer;
    private bool gameActive = false; 
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
        if (!gameActive || gameOver) return; 

        timer -= Time.deltaTime;
        if (uiManager != null)
        {
            uiManager.SetTimer(timer);
        }
        if (timer <= 0)
        {
            GameOver(false);
        }
    }

    public void StartGame()
    {
        gameActive = true;
    }

    public void EndGame()
    {
        gameActive = false; 
    }

    private void SpawnWorker()
    {
        currentWorker = Instantiate(workerPrefab, workerSpawnPoint.position, Quaternion.identity);
        currentWorker.transform.SetParent(transform);
        currentWorker.tag = "Worker"; 
    }

    private void SpawnClimbingLatch()
    {
        currentClimbingLatch = Instantiate(climbingLatchPrefab, climbingLatchSpawnPoint.position, Quaternion.identity);
        currentClimbingLatch.transform.SetParent(transform);

     
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
        if (gameOver) return; 
        gameOver = true;

        gameActive = false;
        if (won)
        {
            Debug.Log("You won!");
            GameManager.instance.AddScore(1);
            GameManager.instance.MiniGameCompleted();
        }
        else
        {
            Debug.Log("You lost!");
            GameManager.instance.LoseLife();
            StartCoroutine(HandleGround());
            GameManager.instance.MiniGameCompleted();
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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

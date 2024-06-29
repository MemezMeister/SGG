using UnityEngine;
using System.Collections;

public class MiniGame3Manager : MonoBehaviour, IMiniGameManager
{
    public GameObject forkliftPrefab;
    public Transform forkliftSpawnPoint;
    public GameObject obstaclePrefab;
    public Transform[] obstaclePositions; 
    public float timeLimit = 10f;
    public MiniGameUIManager uiManager; 

    private GameObject currentForklift;
    private float timer;
    private bool gameActive = false;
    private bool lifeLost = false;

    void Start()
    {
        if (uiManager != null)
        {
            uiManager.SetLives(GameManager.instance.lives);
        }
        SpawnForklift();
        SpawnObstacles();
    }

    void Update()
    {
        if (!gameActive) return;

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
        timer = timeLimit;
        if (uiManager != null)
        {
            uiManager.SetTimer(timer);
        }
    }
    public void EndGame()
    {
        gameActive = false; 
    }
    private void SpawnForklift()
    {
        currentForklift = Instantiate(forkliftPrefab, forkliftSpawnPoint.position, Quaternion.identity);
        currentForklift.AddComponent<Forklift>(); 
    }

    private void SpawnObstacles()
    {
        foreach (var position in obstaclePositions)
        {
            Instantiate(obstaclePrefab, position.position, Quaternion.identity);
        }
    }

    public void GameOver(bool won)
    {
        if (!lifeLost)
        {
            gameActive = false;
            lifeLost = true;
            if (won)
            {
                Debug.Log("You won!");
                GameManager.instance.MiniGameCompleted();
            }
            else
            {
                Debug.Log("You lost!");
                GameManager.instance.LoseLife();
                StartCoroutine(ProceedToNextMiniGame());
            }
        }
    }

    private IEnumerator ProceedToNextMiniGame()
    {
        yield return new WaitForSeconds(2);
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

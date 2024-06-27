using UnityEngine;
using System.Collections; // Add this for IEnumerator

public class MiniGame2Manager : MonoBehaviour, IMiniGameManager
{
    public Transform wireStart;
    public Transform wireEnd;
    public GameObject wirePrefab;
    public GameObject obstaclePrefab;
    public Transform obstacle1Pos;
    public Transform obstacle2Pos;
    public float timeLimit = 10f;
    public MiniGameUIManager uiManager; 

    private GameObject obstacle1;
    private GameObject obstacle2;
    private GameObject currentWire;
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
        SpawnObstacles();
        SpawnWire();
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

    private void SpawnObstacles()
    {
        obstacle1 = Instantiate(obstaclePrefab, obstacle1Pos.position, Quaternion.identity, transform);
        obstacle2 = Instantiate(obstaclePrefab, obstacle2Pos.position, Quaternion.identity, transform);

        obstacle1.tag = "Obstacle";
        obstacle2.tag = "Obstacle";
        obstacle1.AddComponent<BoxCollider2D>();
        obstacle2.AddComponent<BoxCollider2D>();
    }

    private void SpawnWire()
    {
        currentWire = Instantiate(wirePrefab, wireStart.position, Quaternion.identity);
        currentWire.transform.SetParent(transform);

        Wire wireScript = currentWire.GetComponent<Wire>();
        if (wireScript != null)
        {
            wireScript.miniGame2Manager = this;
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
            GameManager.instance.MiniGameCompleted();
        }
        else
        {
            Debug.Log("You lost!");
            GameManager.instance.LoseLife();
            StartCoroutine(ProceedToNextMiniGame());
        }
    }

    private IEnumerator ProceedToNextMiniGame()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.MiniGameCompleted();
    }
}

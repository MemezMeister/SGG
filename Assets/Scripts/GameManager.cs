using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int lives = 3;
    public float initialTimeLimit = 10f; // Starting time for each mini-game
    public Camera mainCamera;
    public Transform[] miniGamePositions; // Positions for each mini-game
    private int currentMiniGameIndex = 0;
    private float currentTimeLimit;
    private MiniGameUIManager uiManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uiManager = FindObjectOfType<MiniGameUIManager>();
        currentTimeLimit = initialTimeLimit;
        StartNextMiniGame();
    }

    public void MiniGameCompleted()
    {
        currentMiniGameIndex++;
        if (currentMiniGameIndex < miniGamePositions.Length)
        {
            StartCoroutine(DelayedStartNextMiniGame());
        }
        else
        {
            Debug.Log("All mini-games completed!");
            // Restart the game with a reduced timer
            RestartMiniGamesWithReducedTime();
        }
    }

    private IEnumerator DelayedStartNextMiniGame()
    {
        yield return new WaitForSeconds(2); // 2 seconds delay before starting the next mini-game
        StartNextMiniGame();
    }

    private void StartNextMiniGame()
    {
        mainCamera.transform.position = miniGamePositions[currentMiniGameIndex].position;
        uiManager.SetTimer(currentTimeLimit);
        var currentMiniGame = miniGamePositions[currentMiniGameIndex].GetComponent<IMiniGameManager>();
        currentMiniGame?.StartGame();
    }

    public void LoseLife()
    {
        lives--;
        uiManager.SetLives(lives);

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            RestartMiniGames();
        }
        else
        {
            StartCoroutine(DelayedStartNextMiniGame());
        }
    }

    private void RestartMiniGamesWithReducedTime()
    {
        currentMiniGameIndex = 0;
        currentTimeLimit = Mathf.Max(1f, currentTimeLimit - 1f); // Reduce the timer but not below 1 second
        StartNextMiniGame();
    }

    private void RestartMiniGames()
    {
        lives = 3; // Reset lives
        currentMiniGameIndex = 0;
        currentTimeLimit = initialTimeLimit; // Reset the time limit
        StartNextMiniGame();
    }
}
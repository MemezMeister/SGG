using UnityEngine;
using UnityEngine.SceneManagement;
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
    private static int score = 0; // Static variable to persist score

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
            StartCoroutine(RestartWithDelay()); 
        }
    }

    private IEnumerator DelayedStartNextMiniGame()
    {
        yield return new WaitForSeconds(2); 
        StartNextMiniGame();
    }

    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSeconds(2); /
        ReloadCurrentSceneWithReducedTime();
    }

    private void StartNextMiniGame()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (miniGamePositions.Length > currentMiniGameIndex && miniGamePositions[currentMiniGameIndex] != null)
        {
            mainCamera.transform.position = miniGamePositions[currentMiniGameIndex].position;
        }
        else
        {
            Debug.LogError("Mini game position not set or out of bounds.");
        }

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

    private void ResetMiniGames()
    {
        foreach (var position in miniGamePositions)
        {
            var miniGameManager = position.GetComponent<IMiniGameManager>();
            miniGameManager?.ResetGame();
        }
    }

    private void ReloadCurrentSceneWithReducedTime()
    {
        currentMiniGameIndex = 0;
        currentTimeLimit = Mathf.Max(1f, currentTimeLimit - 1f); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RestartMiniGames() 
    {
        currentMiniGameIndex = 0;
        currentTimeLimit = initialTimeLimit; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int points)
    {
        score += points;
        uiManager.SetScore(score);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main; 
        uiManager = FindObjectOfType<MiniGameUIManager>();
        uiManager.SetScore(score); 
        StartNextMiniGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

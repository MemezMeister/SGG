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
<<<<<<< Updated upstream
            // Handle game completion logic here
=======
            StartCoroutine(RestartWithDelay()); // Add a delay before restarting with reduced timer
>>>>>>> Stashed changes
        }
    }

    private IEnumerator DelayedStartNextMiniGame()
    {
        yield return new WaitForSeconds(2); // 2 seconds delay before starting the next mini-game
        StartNextMiniGame();
    }

    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSeconds(2); // 2 seconds delay before restarting
        ReloadCurrentSceneWithReducedTime();
    }

    private void StartNextMiniGame()
    {
<<<<<<< Updated upstream

        mainCamera.transform.position = miniGamePositions[currentMiniGameIndex].position;
        uiManager.SetTimer(initialTimeLimit);
=======
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
>>>>>>> Stashed changes
    }

    public void LoseLife()
    {
        lives--;
        uiManager.SetLives(lives);

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
        }
    }
<<<<<<< Updated upstream
    public interface IMiniGameManager
{
    void StartGame();
    void EndGame();
}
=======

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
        currentTimeLimit = Mathf.Max(1f, currentTimeLimit - 1f); // Reduce the timer but not below 1 second
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RestartMiniGames() // Change method to IEnumerator
    {
        currentMiniGameIndex = 0;
        currentTimeLimit = initialTimeLimit; // Reset the time limit
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int points)
    {
        score += points;
        uiManager.SetScore(score);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main; // Re-establish the camera reference
        uiManager = FindObjectOfType<MiniGameUIManager>();
        uiManager.SetScore(score); // Restore the score
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
>>>>>>> Stashed changes
}

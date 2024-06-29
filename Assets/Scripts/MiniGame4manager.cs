using UnityEngine;
using System.Collections;

public class MiniGame4Manager : MonoBehaviour, IMiniGameManager
{
    public GameObject phonePrefab;
    public Transform phoneSpawnPoint;
    public GameObject player;
    public float timeLimit = 10f;
    public MiniGameUIManager uiManager;

    private GameObject currentPhone;
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
        SpawnPhone();
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

    private void SpawnPhone()
    {
        currentPhone = Instantiate(phonePrefab, phoneSpawnPoint.position, Quaternion.identity);
        currentPhone.transform.SetParent(transform);
        Phone phoneScript = currentPhone.GetComponent<Phone>();
        if (phoneScript != null)
        {
            phoneScript.miniGame4Manager = this;
        }
        else
        {
            Debug.LogError("Phone prefab does not have a Phone script attached.");
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
            StartCoroutine(ProceedToNextMiniGame());
        }
    }

    private IEnumerator ProceedToNextMiniGame()
    {
        yield return new WaitForSeconds(2);
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

using UnityEngine;
using System.Collections;

public class MiniGame5Manager : MonoBehaviour, IMiniGameManager
{
    public GameObject weldingToolPrefab;
    public Transform weldingToolSpawnPoint;
    public float timeLimit = 10f;
    public MiniGameUIManager uiManager; 

    private GameObject currentWeldingTool;
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
        SpawnWeldingTool();
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

    private void SpawnWeldingTool()
    {
        currentWeldingTool = Instantiate(weldingToolPrefab, weldingToolSpawnPoint.position, Quaternion.identity);
        currentWeldingTool.transform.SetParent(transform);

        
        WeldingTool weldingToolScript = currentWeldingTool.GetComponent<WeldingTool>();
        if (weldingToolScript != null)
        {
            weldingToolScript.SetMiniGame5Manager(this);
        }
        else
        {
            Debug.LogError("Welding tool prefab does not have a WeldingTool script attached.");
        }

        
        WeldingFlame weldingFlameScript = currentWeldingTool.GetComponentInChildren<WeldingFlame>();
        if (weldingFlameScript != null)
        {
            weldingFlameScript.SetMiniGame5Manager(this);
        }
        else
        {
            Debug.LogError("Welding tool prefab does not have a WeldingFlame script attached.");
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
  
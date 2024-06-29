using UnityEngine;
using TMPro;

public class MiniGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public int lives = 3;
    private float timer;

    private void Start()
    {
        UpdateLivesText();
        UpdateTimerText();
    }

    public void SetLives(int lives)
    {
        this.lives = lives;
        UpdateLivesText();
    }

    public void SetTimer(float timer)
    {
        this.timer = timer;
        UpdateTimerText();
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }

    private void UpdateTimerText()
    {
        timerText.text = "Time: " + timer.ToString("F2");
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();
            if (timer <= 0)
            {
                // Handle timer running out if needed
            }
        }
    }
}

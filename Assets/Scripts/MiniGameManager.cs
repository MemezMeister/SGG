using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public GameObject[] miniGames;
    private int currentMiniGameIndex = -1;

    void Start()
    {
        StartNextMiniGame();
    }

    public void StartNextMiniGame()
    {
        if (currentMiniGameIndex >= 0 && currentMiniGameIndex < miniGames.Length)
        {
            miniGames[currentMiniGameIndex].SetActive(false);
        }

        currentMiniGameIndex = (currentMiniGameIndex + 1) % miniGames.Length;
        miniGames[currentMiniGameIndex].SetActive(true);
    }

    public void RestartMiniGame()
    {
        if (currentMiniGameIndex >= 0 && currentMiniGameIndex < miniGames.Length)
        {
            miniGames[currentMiniGameIndex].SetActive(false);
            miniGames[currentMiniGameIndex].SetActive(true);
        }
    }
}

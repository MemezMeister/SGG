using UnityEngine;

public class WeldingFlame : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool gameOver = false;
    private MiniGame5Manager miniGame5Manager;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.positionCount = 0;

        
        miniGame5Manager = FindObjectOfType<MiniGame5Manager>();
        if (miniGame5Manager == null)
        {
            Debug.LogError("MiniGame5Manager not found in the scene.");
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameOver) return;

        if (other.CompareTag("WireEnd"))
        {
            gameOver = true;
            miniGame5Manager.GameOver(true);
        }
        else if (!other.CompareTag("Path"))
        {
            gameOver = true;
            miniGame5Manager.GameOver(false);
        }
    }

    
    public void SetMiniGame5Manager(MiniGame5Manager manager)
    {
        miniGame5Manager = manager;
    }
}

using UnityEngine;

public class ClimbingLatch : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    public MiniGame6Manager miniGame6Manager; 
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        dragging = true;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            transform.position = newPosition;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WireEnd"))
        {
            miniGame6Manager.GameOver(true);
        }
    }
}

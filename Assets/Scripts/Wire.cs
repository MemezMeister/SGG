using UnityEngine;

public class Wire : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    public MiniGame2Manager miniGame2Manager; 
    private LineRenderer lineRenderer;
    private Vector3 initialPosition;
    private Rigidbody2D rb2D;
    private bool insideFuseBox = false;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, initialPosition);
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        dragging = true;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        Debug.Log("Dragging started for wire.");
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            if (IsValidPosition(newPosition))
            {
                rb2D.MovePosition(newPosition);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
            }
            else
            {
                Debug.Log("Invalid position detected during drag.");
                miniGame2Manager.GameOver(false);
            }
        }
    }

    void OnMouseUp()
    {
        dragging = false;
        Debug.Log("Wire dropped at position: " + transform.position);
        if (Vector3.Distance(transform.position, miniGame2Manager.wireEnd.position) < 0.5f)
        {
            Debug.Log("Wire reached the end.");
            miniGame2Manager.GameOver(true);
        }
        else if (!insideFuseBox)
        {
            Debug.Log("Wire not inside fusebox.");
            miniGame2Manager.GameOver(false);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private bool IsValidPosition(Vector3 position)
    {
        if (!insideFuseBox)
        {
            Debug.Log("Position outside fusebox boundaries: " + position);
            return false;
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.tag);

        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with obstacle.");
            miniGame2Manager.GameOver(false);
        }
        else if (collision.CompareTag("WireEnd"))
        {
            Debug.Log("Collided with wire end.");
            miniGame2Manager.GameOver(true);
        }
        else if (collision.CompareTag("FuseBox"))
        {
            Debug.Log("Collided with fuse box.");
            insideFuseBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FuseBox"))
        {
            insideFuseBox = false;
            Debug.Log("Exited fusebox.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Staying in collision with obstacle.");
            miniGame2Manager.GameOver(false);
        }
    }
}

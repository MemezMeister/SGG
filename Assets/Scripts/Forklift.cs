using UnityEngine;

public class Forklift : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private MiniGame3Manager miniGameManager;
    private Rigidbody2D rb2D;

    void Start()
    {
        miniGameManager = FindObjectOfType<MiniGame3Manager>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        dragging = true;
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            rb2D.MovePosition(newPosition);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Collision with obstacle detected.");
            miniGameManager.GameOver(false);
        }
        else if (collision.CompareTag("WireEnd")) // Assuming there's a goal area
        {
            Debug.Log("Reached the goal.");
            miniGameManager.GameOver(true);
        }
    }
}

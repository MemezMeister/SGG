using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
            transform.position = GetMouseWorldPos() + offset;
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
}

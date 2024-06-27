using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        dragging = true;
        Debug.Log("Dragging started.");
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            transform.position = GetMouseWorldPos() + offset;
            Debug.Log("Dragging in progress. Current position: " + transform.position);
        }
    }

    void OnMouseUp()
    {
        dragging = false;
        Debug.Log("Dragging ended.");
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

using UnityEngine;

public class WeldingTool : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private LineRenderer lineRenderer;
    public GameObject flame;
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
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        dragging = true;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, flame.transform.position);
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            transform.position = newPosition;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, flame.transform.position);
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

    public void SetMiniGame5Manager(MiniGame5Manager manager)
    {
        miniGame5Manager = manager;
    }

}

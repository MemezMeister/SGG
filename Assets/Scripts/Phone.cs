using UnityEngine;

public class Phone : MonoBehaviour
{
    public MiniGame4Manager miniGame4Manager;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            miniGame4Manager.GameOver(true);
        }
    }

    void OnMouseDown()
    {
        
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }

    void OnMouseUp()
    {
        
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
}

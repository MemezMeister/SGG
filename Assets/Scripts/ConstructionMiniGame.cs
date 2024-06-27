using UnityEngine;

public class ConstructionMiniGame : MonoBehaviour
{
    private void OnEnable()
    {
        GameObject.Find("Brick").GetComponent<Rigidbody2D>().isKinematic = true;
        GameObject.Find("Brick").transform.position = new Vector3(0, 10, 0); // Position the brick off-screen
        GameObject.Find("HardHat").transform.position = new Vector3(-2, 0, 0); // Reset the hard hat position
    }
}

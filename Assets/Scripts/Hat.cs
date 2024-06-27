using UnityEngine;

public class Hat : MonoBehaviour
{
    private MiniGame1Manager miniGame1Manager;

    void Start()
    {
        miniGame1Manager = FindObjectOfType<MiniGame1Manager>();
    }

    void OnMouseDown()
    {
        miniGame1Manager.HatClicked(gameObject.name);
    }
}

using UnityEngine;

public class MiniGame1 : MonoBehaviour
{
    private MiniGame1Manager miniGame1Manager;

    void Start()
    {
        miniGame1Manager = FindObjectOfType<MiniGame1Manager>();
    }

    public void HatClicked(string hatName)
    {
        miniGame1Manager.HatClicked(hatName);
    }
}

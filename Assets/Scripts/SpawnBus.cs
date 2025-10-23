using UnityEngine;

public class SpawnBus : MonoBehaviour
{
    public GameObject Autobus;
    private Animator busAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        busAnimation = Autobus.GetComponent<Animator>();

    }

    public void Spawn()
    {
        if (!Autobus.activeSelf)
        {
            Autobus.SetActive(true);
        }
            
        busAnimation.SetTrigger("Move");
    }
}

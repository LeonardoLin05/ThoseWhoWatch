using UnityEngine;

public class SpawnBus : MonoBehaviour
{
    [SerializeField] private GameObject Autobus;
    private Animator busAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        busAnimation = Autobus.GetComponent<Animator>();
        Autobus.SetActive(false);
    }

    public void Spawn()
    {
        if (!Autobus.activeSelf)
        {
            Debug.Log("Spawn");
            Autobus.SetActive(true);
        }
        busAnimation.SetTrigger("Move");
    }
}

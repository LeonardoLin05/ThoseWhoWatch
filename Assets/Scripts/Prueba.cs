using UnityEngine;
using UnityEngine.Events;

public class Prueba : MonoBehaviour
{
    public UnityEvent spawnBus;

    void OnTriggerEnter(Collider other)
    {
        if(spawnBus != null)
        {
            spawnBus.Invoke();
        }
    }
}

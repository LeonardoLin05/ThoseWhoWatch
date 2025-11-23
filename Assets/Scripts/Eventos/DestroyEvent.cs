using UnityEngine;

public class DestroyEvent : MonoBehaviour
{
    // Por si quieres que el gameObject sea destruido después de un tiempo tras
    // poner setActive a true
    [SerializeField] private bool destroyOnEnable = false;
    [SerializeField] private float timeToDestroy;

    void OnEnable()
    {
        if(destroyOnEnable)
        {
            Destroy(gameObject, timeToDestroy);
        }
    }

    // Para llamarlo desde un UnityEvent
    public void DestroyObject()
    {
        Destroy(gameObject, timeToDestroy);
    }
}

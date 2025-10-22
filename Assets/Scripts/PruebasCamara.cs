using JetBrains.Annotations;
using UnityEngine;

public class PruebasCamara : MonoBehaviour
{
    public Transform cubo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - cubo.position), 1 *Time.deltaTime);
    }
}

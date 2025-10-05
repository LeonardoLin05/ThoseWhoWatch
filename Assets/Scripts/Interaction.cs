using UnityEditor;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    private LayerMask mask;

    void Start()
    {
        mask = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, mask))
        {
            Debug.Log("Estas mirando a un objecto interactuable");
        }
    }
}

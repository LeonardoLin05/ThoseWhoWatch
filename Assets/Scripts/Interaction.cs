using UnityEditor;
using UnityEngine;

public interface IInteractable
{
    public void interact();
}

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
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable i))
            {
                i.interact();
            }
            Debug.Log("Estas mirando a un objecto interactuable");
        }
    }
}

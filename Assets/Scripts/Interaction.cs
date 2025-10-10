using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public IEnumerator interact();
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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f, mask))
        {
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable i))
            {
                Debug.Log("Objeto interactuable detectado: " + hit.collider.name);
                if (VariablesGlobales.INTERACTUAR && Input.GetKeyDown(KeyCode.E))
                {
                    VariablesGlobales.INTERACTUAR = false;
                    StartCoroutine(i.interact());
                }
                
            }
            Debug.Log("Estas mirando a un objecto interactuable");
        }
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public interface IInteractable
{
    public IEnumerator interact();

    public string MensajeInteraccion();
}

public class Interaction : MonoBehaviour
{

    private LayerMask mask;
    private IInteractable currentInteractable;

    private TextMeshProUGUI texto;

    void Start()
    {
        mask = LayerMask.GetMask("Interactable");

        if(texto == null){

            texto = GameObject.Find("texto_interactuar").GetComponent<TextMeshProUGUI>();
        }

        texto.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f, mask))
        {
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable i))
            {
                currentInteractable = i;
                texto.text = i.MensajeInteraccion();
                texto.gameObject.SetActive(true);
                Debug.Log("Objeto interactuable detectado: " + hit.collider.name);
                
                if (VariablesGlobales.INTERACTUAR && Input.GetKeyDown(KeyCode.E))
                {
                    VariablesGlobales.INTERACTUAR = false;
                    StartCoroutine(i.interact());
                }
                
            }
            Debug.Log("Estas mirando a un objecto interactuable");
            return;
        }
        currentInteractable = null;
        texto.gameObject.SetActive(false);
    }
}

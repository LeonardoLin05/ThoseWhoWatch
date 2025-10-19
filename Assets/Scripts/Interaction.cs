using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public interface IInteractable
{
    public IEnumerator interact();

    public string MensajeInteraccion();
}

public class Interaction : MonoBehaviour
{

    private LayerMask mask;
    private TextMeshProUGUI texto;
    public Image punteroInteractuar;

    void Start()
    {
        mask = LayerMask.GetMask("Interactable");

        if (texto == null)
        {

            texto = GameObject.Find("texto_interactuar").GetComponent<TextMeshProUGUI>();
        }
        punteroInteractuar = GameObject.Find("PunteroInteractuar").GetComponent<Image>();

        punteroInteractuar.gameObject.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f, mask))
        {
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable i))
            {

                texto.text = i.MensajeInteraccion();
                punteroInteractuar.gameObject.GetComponent<Image>().enabled = true;
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
        punteroInteractuar.gameObject.GetComponent<Image>().enabled = false;
        texto.text = "";
    }
}

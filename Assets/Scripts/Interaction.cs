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
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, mask | LayerMask.GetMask("Default")))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance);
            if (hit.transform.gameObject.layer == 6)
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
                // NO quiten este return, por algún motivo si se quita los textos de las interacciones
                // no aparecen
                return;
            }
        }
        punteroInteractuar.gameObject.GetComponent<Image>().enabled = false;
        texto.text = "";
    }
}

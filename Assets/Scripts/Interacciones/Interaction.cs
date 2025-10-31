using System.Collections;
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
    public static Interaction Instance { get; private set; }

    private LayerMask mask;
    private TextMeshProUGUI texto;
    public Image punteroInteractuar;

    void Awake()
    {
          if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        mask = LayerMask.GetMask("Interactable") | LayerMask.GetMask("Default");

        if (texto == null)
        {
            texto = GameObject.Find("texto_interactuar").GetComponent<TextMeshProUGUI>();
        }

        if(punteroInteractuar == null)
        {
            punteroInteractuar = GameObject.Find("PunteroInteractuar").GetComponent<Image>();
        }
    
        punteroInteractuar.gameObject.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, mask) && hit.transform.gameObject.layer == 6)
        {
        Debug.DrawRay(ray.origin, ray.direction * hit.distance);
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable i))
            {
                texto.text = i.MensajeInteraccion();
                punteroInteractuar.gameObject.GetComponent<Image>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    VariablesGlobales.INTERACTUAR = false;
                    StartCoroutine(i.interact());
                }
            }
            // NO quiten este return, por algún motivo si se quita los textos de las interacciones
            // no aparecen
            return;
        }
        punteroInteractuar.gameObject.GetComponent<Image>().enabled = false;
        texto.text = "";
    }
}

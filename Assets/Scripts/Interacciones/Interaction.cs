using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public interface IInteractable
{
    public void interact();

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
        enabled = true;
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
        Ray ray = new(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, mask) && hit.transform.gameObject.layer == 6)
        {
        Debug.DrawRay(ray.origin, ray.direction * hit.distance);
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable i))
            {
                texto.text = i.MensajeInteraccion();
                punteroInteractuar.gameObject.GetComponent<Image>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    texto.text = "";
                    i.interact();
                }
            }
        }
        else {
            punteroInteractuar.gameObject.GetComponent<Image>().enabled = false;
            texto.text = "";
        }
    }
}

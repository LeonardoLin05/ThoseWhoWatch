using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class InteractRead : MonoBehaviour, IInteractable
{
    [SerializeField] private string texto;

    [SerializeField] private GameObject panelNota;
    [SerializeField] private TextMeshProUGUI textoNota;

    [SerializeField] private Transform objetivoZoom;
    [SerializeField] private float velocidadZoom = 10f;
    [SerializeField] private UnityEvent eventos;

    void Awake()
    {
        if(panelNota == null)
        {
            panelNota = GameObject.FindGameObjectWithTag("panelNota");
        }

        if(textoNota == null)
        {
            textoNota = GameObject.FindGameObjectWithTag("TextoNota").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Cerrar();
        }
    }

    public void Interact()
    {
        // Activamos el update
        enabled = true;
        Abrir();
    }

    private void Cerrar()
    {
        // Desactivamos el update
        enabled = false;
        // Comprobamos que haya eventos que ejecutar
        eventos?.Invoke();

        InteractNPCs.ActivarInstances(true);

        if(objetivoZoom != null)
        {
            TalkZoomMoveCamera.Instance.StopZoomMovement();
        }

        panelNota.SetActive(false);
    }

    private void Abrir()
    {
        InteractNPCs.ActivarInstances(false);

        if(objetivoZoom != null)
        {
            TalkZoomMoveCamera.Instance.SetCabeza(objetivoZoom);
            TalkZoomMoveCamera.Instance.StartZoomMovement(velocidadZoom, true);
        }

        panelNota.SetActive(true);
        textoNota.text = texto;

    }

    public string MensajeInteraccion()
    {
        return "[E] para leer";
    }
}

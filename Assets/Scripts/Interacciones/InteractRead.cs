using System.Collections;
using UnityEngine;
using TMPro;
public class InteractRead : MonoBehaviour, IInteractable
{
    public string texto;

    public GameObject panelNota;
    public TextMeshProUGUI textoNota;

    public Transform objetivoZoom;
    public float velocidadZoom = 10f;

    private bool viendo = false;

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

    void Update()
    {
        if(viendo && Input.GetKeyDown(KeyCode.E))
        {
            cerrar();
        }
    }

    public IEnumerator interact()
    {
        if (!viendo)
        {
            abrir();
        }
        else
        {
            cerrar();
        }

        yield break;
    }

    private void cerrar()
    {
        viendo = false;

        InteractNPCs.ActivarInstances(true);

        TalkZoomMoveCamera.Instance.StopZoomMovement();

        panelNota.SetActive(false);
    }
    private void abrir()
    {
        viendo = true;

        InteractNPCs.ActivarInstances(false);

        TalkZoomMoveCamera.Instance.setCabeza(objetivoZoom);
        TalkZoomMoveCamera.Instance.StartZoomMovement(velocidadZoom);

        panelNota.SetActive(true);
        textoNota.text = texto;

    }

    public string MensajeInteraccion()
    {
        if (!viendo)
        {
            return "[E] para leer";
        }
        else
        {
            return "[E] para cerrar";
        }
    }
}

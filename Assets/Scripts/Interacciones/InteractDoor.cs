using System.Collections;
using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private bool bloqueada;
    [SerializeField] private string pensamientoPuertaBloqueada;
    [SerializeField] private AudioSource sonidoAbrir;

    [SerializeField] private AudioSource sonidoCerrarConLlave;

    private Animator door;
    private bool open = false;

    private WaitForSecondsRealtime esperar = new(1f);

    void Start()
    {
        door = gameObject.GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!bloqueada)
        {
            if (sonidoAbrir != null)
            {
                sonidoAbrir.Play(); 
            }
            open = !open;
            door.SetBool("open", open);
        }
        else
        {
            StartCoroutine(PuertaBloqueada());
        }
    }

    private IEnumerator PuertaBloqueada()
    {
        gameObject.layer = 0;
        door.SetTrigger("blocked");
        Thoughts.Instance.StartThoughts(pensamientoPuertaBloqueada);
        yield return esperar;
        gameObject.layer = 6;
    }

    // IGNORAR: para evento gasolinera
    public void EventoGasolineraEncerrar()
    {
        if(open)
        {
            open = !open;
        }
        door.SetBool("open", open);
        bloqueada = true;
        pensamientoPuertaBloqueada = "Me había encerrado con llave";
    }

    // IGNORAR: llamada al evento casa final de noche
    public void EventoCerrarConLlave()
    {
        StartCoroutine(EventoCerrarConLlaveEnumerator());
    }

    // IGNORAR: para evento puerta casa final de noche
    private IEnumerator EventoCerrarConLlaveEnumerator()
    {
        PlayerMovement.Instance.enabled = false;
        HeadbobSystem.Instance.enabled = false;
        CameraMovement.Instance.enabled = false;
        gameObject.layer = 0;

        TalkZoomMoveCamera.Instance.SetCabeza(transform.GetChild(0));
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f, false);

        if(open)
        {
            open = !open;
            door.SetBool("open", open);
            yield return new WaitForSecondsRealtime(1f);
        }
        
        sonidoCerrarConLlave.Play();
        yield return new WaitForSecondsRealtime(4f);
        PlayerMovement.Instance.enabled = true;
        HeadbobSystem.Instance.enabled = true;
        CameraMovement.Instance.enabled = true;

        TalkZoomMoveCamera.Instance.StopZoomMovement();

        Thoughts.Instance.StartThoughts("Me fuí a la cama rezando para que mañana todo volviese a la normalidad");
    }

    public bool GetBloqueada()
    {
        return bloqueada;
    }

    public void SetBloqueada(bool bloqueada)
    {
        this.bloqueada = bloqueada;
    }

    public string MensajeInteraccion()
    {
        return !open ? "[E] para Abrir" : "[E] para Cerrar";
    }
}

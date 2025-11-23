using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private bool bloqueada;
    [SerializeField] private string pensamientoPuertaBloqueada;

    private Animator door;
    private bool open = false;

    void Start()
    {
        door = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!bloqueada)
        {
            open = !open;
            door.SetBool("open", open);
        }
        else
        {
            PuertaBloqueada();
        }
    }

    private void PuertaBloqueada()
    {
        Thoughts.Instance.StartThoughts(pensamientoPuertaBloqueada);
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
        if (!open)
        {
            return "[E] para Abrir";
        }
        else
        {
            return "[E] para Cerrar";
        }
    }
}

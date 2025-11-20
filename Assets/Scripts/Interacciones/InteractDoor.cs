using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    public bool bloqueada;
    [SerializeField] private string pensamientoPuertaBloqueada;

    private Animator door;
    private bool open = false;
    private TextMeshProUGUI pensamientos;

    void Start()
    {
        door = GetComponent<Animator>();
        pensamientos = GameObject.Find("Pensamiento").GetComponent<TextMeshProUGUI>();
    }

    public void interact()
    {
        if (!bloqueada)
        {
            open = !open;
            door.SetBool("open", open);
        }
        else
        {
            puertaBloqueada();
        }
    }

    private void puertaBloqueada()
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

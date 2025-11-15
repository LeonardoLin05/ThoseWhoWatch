using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    public bool bloqueada;
    [SerializeField] private string frasePuertaBloqueada;

    private Animator door;
    private bool open = false;
    private TextMeshProUGUI pensamientos;

    void Start()
    {
        door = GetComponent<Animator>();
        pensamientos = GameObject.Find("Pensamiento").GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator interact()
    {
        if (!bloqueada)
        {
            open = !open;
            door.SetBool("open", open);
        }
        else
        {
            if(!VariablesGlobales.EN_PENSAMIENTO)
            StartCoroutine(puertaBloqueada());
        }
        yield break;
    }

    private IEnumerator puertaBloqueada()
    {
        pensamientos.text = frasePuertaBloqueada;
        yield return new WaitForSeconds(2f);
        pensamientos.text = "";
        VariablesGlobales.EN_PENSAMIENTO = false;
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
        frasePuertaBloqueada = "Me había encerrado con llave";
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

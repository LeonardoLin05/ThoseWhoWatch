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
            door.SetTrigger("interact");
            Debug.Log("El objecto ha hecho algo");
            yield break;
        }
        else
        {
            if(!VariablesGlobales.EN_PENSAMIENTO)
            StartCoroutine(puertaBloqueada());
        }
        VariablesGlobales.INTERACTUAR = true;
    }

    private IEnumerator puertaBloqueada()
    {
        pensamientos.text = frasePuertaBloqueada;
        yield return new WaitForSeconds(2f);
        pensamientos.text = "";
        VariablesGlobales.EN_PENSAMIENTO = false;
    }

    public string MensajeInteraccion() {
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

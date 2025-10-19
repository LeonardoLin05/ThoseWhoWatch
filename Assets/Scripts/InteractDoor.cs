using System.Collections;
using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    private Animator door;
    private bool open = false;

    void Start()
    {
        door = GetComponent<Animator>();
    }

    public IEnumerator interact()
    {
        open = !open;
        door.SetBool("open", open);
        door.SetTrigger("interact");
        VariablesGlobales.INTERACTUAR = true;
        Debug.Log("El objecto ha hecho algo");
        yield break;
    }

    public string MensajeInteraccion(){
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

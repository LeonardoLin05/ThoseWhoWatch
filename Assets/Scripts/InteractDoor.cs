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
        Debug.Log("El objecto ha hecho algo");
        yield return null;
    }

    public string MensajeInteraccion(){

        return "Press E to open";
    }


}

using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    private Animator door;
    private bool open = false;

    void Start()
    {
        door = gameObject.GetComponent<Animator>();
    }

    public void interact()
    {
        
        open = !open;
        door.SetBool("open", open);
        door.SetTrigger("interact");
        Debug.Log("El objecto ha hecho algo");
        
    }

}

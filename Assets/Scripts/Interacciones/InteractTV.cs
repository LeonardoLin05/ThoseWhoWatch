using UnityEngine;

public class InteractTV : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject panelVideo;

    private bool active = false;

    public void Interact()
    {
        if(!active){
      panelVideo.SetActive(true);
        active = true;
        }
        else
        {
            panelVideo.SetActive(false);
            active = false;
            Debug.Log("hola");
        }
    }

    public string MensajeInteraccion()
    {
        if (!active)
        {
           return "[E] para encender"; 
        }
        else
        {
            return "[E] para apagar";
        }
    }
}

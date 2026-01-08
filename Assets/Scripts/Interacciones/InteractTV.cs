using UnityEngine;
using UnityEngine.Video;

public class InteractTV : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject[] panelVideo;

    private bool active = false;

    private int videoAReproducir = 0;

    public void Interact()
    {
        if(!active)
        {
            panelVideo[videoAReproducir].SetActive(true);
            active = true;
        }
        else
        {
            panelVideo[videoAReproducir].SetActive(false);
            active = false;
            videoAReproducir = (videoAReproducir + 1)%panelVideo.Length;
        }
    }

    public string MensajeInteraccion()
    {
        return !active ? "[E] para Encender" : "[E] para Apagar";
    }
}

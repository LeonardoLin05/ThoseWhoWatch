using System.Collections;
using UnityEngine;

public class InteractBus : MonoBehaviour, IInteractable
{
    public GameObject Autobus;
    private Animator busAnimation;
    public Transform teleportBus;
    private bool active = false;

    void Start()
    {
        busAnimation = Autobus.GetComponent<Animator>();
    }

    public IEnumerator interact()
    {
        if (!active)
        {
            active = true;

            if (!Autobus.activeSelf)
            {
                Autobus.SetActive(true);
            }

            busAnimation.SetTrigger("Move");

            yield return new WaitForSeconds(6f);

            InteractTeleport busTeleport = Autobus.GetComponent<InteractTeleport>();
            if (busTeleport != null)
            {
                busTeleport.teleportDestino = teleportBus;
                busTeleport.player = GameObject.FindWithTag("Player").transform;
                busTeleport.ActivarTeleport();
            }
        }
        VariablesGlobales.INTERACTUAR = true;
    }

    public string MensajeInteraccion()
    {
        if (active)
        {
            return "Bus is arriving";
        }
        else
        {
            return "Press E to spawn bus";
        }
    }
}

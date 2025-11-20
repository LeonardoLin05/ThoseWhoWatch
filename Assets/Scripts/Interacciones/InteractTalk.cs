using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class InteractTalk : MonoBehaviour, IInteractable
{
    [SerializeField] private string pensamientoObjeto;
    public GameObject gameObjectDesactivar;

    [SerializeField] private UnityEvent evento;

    public void interact()
    {
        if(evento != null)
        {
            evento.Invoke();
        }

        Thoughts.Instance.StartThoughts(pensamientoObjeto);

        if (gameObjectDesactivar != null)
        {
            Destroy(gameObjectDesactivar);
        }
    }
    
    public string MensajeInteraccion()
    {
        return "[E] para Interactuar";
    }
}

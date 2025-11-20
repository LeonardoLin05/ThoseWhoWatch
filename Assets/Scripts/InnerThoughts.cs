using System.Collections;
using UnityEngine;
using TMPro;

public class InnerThoughts : MonoBehaviour
{
    [SerializeField] private string pensamiento_mostrar;

    // Variable para dictar si se quiere que se destruya el gameObject o no
    [SerializeField] private bool oneTimeOnly = true;

    [SerializeField] private GameObject activaTrigger;
    [SerializeField] private InteractNPCs npc;
    [SerializeField] private bool desbloquear = false;
    [SerializeField] private int indice;
    [SerializeField] private int fila;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Thoughts.Instance.StartThoughts(pensamiento_mostrar);

        if (activaTrigger != null)
        {
            activaTrigger.SetActive(true);
        }

        if (desbloquear)
        {
            npc.ActivarBoton(fila, indice);
        }

        if(oneTimeOnly)
        {
            Destroy(gameObject);
        }
        }
    }
}

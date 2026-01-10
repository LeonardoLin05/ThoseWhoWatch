using UnityEngine;

public class DesactivarObjetoMano : MonoBehaviour
{
    private static InteractPickUp interactPickUpScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && interactPickUpScript == null)
        {
            interactPickUpScript = GameObject.FindWithTag("ObjetoEnMano").GetComponent<InteractPickUp>();
            if(interactPickUpScript != null)
            {
                interactPickUpScript.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && interactPickUpScript != null)
        {
            interactPickUpScript.enabled = true;
            interactPickUpScript = null;
        }
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class InteractTalk : MonoBehaviour, IInteractable
{
    [SerializeField] private string pensamientoObjeto;
    private TextMeshProUGUI pensamiento;
    public GameObject gameObjectDesactivar;

    [SerializeField] private UnityEvent evento;

    void Start()
    {
        pensamiento = GameObject.Find("Pensamiento").GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator interact()
    {
        if (!VariablesGlobales.EN_PENSAMIENTO)
        {
            if(evento != null)
            {
                evento.Invoke();
            }

            VariablesGlobales.EN_PENSAMIENTO = true;
            pensamiento.text = pensamientoObjeto;
            yield return new WaitForSeconds(2f);
            VariablesGlobales.EN_PENSAMIENTO = false;
            pensamiento.text = "";
        }

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

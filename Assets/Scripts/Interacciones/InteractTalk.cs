using System.Collections;
using UnityEngine;
using TMPro;

public class InteractTalk : MonoBehaviour, IInteractable
{
    [SerializeField] private string pensamientoObjeto;
    private TextMeshProUGUI pensamiento;
    
    void Start()
    {
        pensamiento = GameObject.Find("Pensamiento").GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator interact()
    {
        if (!VariablesGlobales.EN_PENSAMIENTO)
        {
            VariablesGlobales.EN_PENSAMIENTO = true;
            pensamiento.text = pensamientoObjeto;
            yield return new WaitForSeconds(2f);
            VariablesGlobales.EN_PENSAMIENTO = false;
            pensamiento.text = "";
        }
    }
    
    public string MensajeInteraccion()
    {
        return "[E] para Interactuar";
    }
}

using System.Collections;
using UnityEngine;
using TMPro;

public class InnerThoughts : MonoBehaviour
{
    [SerializeField] private string pensamiento_mostrar;
    [SerializeField] private TextMeshProUGUI pensamiento;

    // Variable para dictar si se quiere que se destruya el gameObject o no
    [SerializeField] private bool oneTimeOnly = true;

    [SerializeField] private GameObject activaTrigger;
    [SerializeField] private InteractNPCs npc;
    [SerializeField] private bool desbloquear;
    [SerializeField] private int indice = -1;

    void Start()
    {
        pensamiento = GameObject.FindGameObjectWithTag("Pensamiento").GetComponent<TextMeshProUGUI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!VariablesGlobales.EN_PENSAMIENTO && other.CompareTag("Player"))
        {
            StartCoroutine(Thoughts());
        }
    }

    private IEnumerator Thoughts()
    {
        VariablesGlobales.EN_PENSAMIENTO = true;

        pensamiento.text = pensamiento_mostrar;
        yield return new WaitForSeconds(5f);
        pensamiento.text = "";

        if (activaTrigger != null)
        {
            activaTrigger.SetActive(true);
        }

        if (desbloquear && indice >= 0)
        {
            npc.ActivarBoton(indice);
        }

        VariablesGlobales.EN_PENSAMIENTO = false;

        if(oneTimeOnly)
        {
            Destroy(gameObject);
        }
    }
}

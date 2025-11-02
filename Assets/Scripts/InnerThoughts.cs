using System.Collections;
using UnityEngine;
using TMPro;
public class InnerThoughts : MonoBehaviour
{
    public string pensamiento_mostrar;
    public TextMeshProUGUI pensamiento;
    public GameObject activaTrigger;
    public InteractNPCs npc;
    public bool desbloquear;
    public int indice = -1;

    void Start()
    {
        if (pensamiento == null)
        {
            pensamiento = GameObject.Find("pensamiento").GetComponent<TextMeshProUGUI>();
        }
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
        Destroy(gameObject);
    }
}

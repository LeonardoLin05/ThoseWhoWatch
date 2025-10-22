using System.Collections;
using UnityEngine;
using TMPro;

public class InnerThoughts : MonoBehaviour
{
    public string pensamiento_mostrar;
    public TextMeshProUGUI pensamiento;
    private Coroutine textoAnimado;
    public GameObject activaTrigger;

    void Start()
    {
        if (pensamiento == null)
        {
            pensamiento = GameObject.Find("pensamiento").GetComponent<TextMeshProUGUI>();
        }
        pensamiento.gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (textoAnimado == null)
            {
                textoAnimado = StartCoroutine(pensamientoAnimado());
            }
        }
    }
    
    private IEnumerator pensamientoAnimado()
    {
            pensamiento.gameObject.SetActive(true);
            pensamiento.text = "";

        for (int i = 0; i < pensamiento_mostrar.Length; i++)
        {
            pensamiento.text = pensamiento.text + pensamiento_mostrar[i];
            yield return new WaitForSeconds(0.05f);
        }
            if(activaTrigger != null)
            {
                activaTrigger.SetActive(true);
            }
            yield return new WaitForSeconds(10f);
            pensamiento.gameObject.SetActive(false);
            
        Destroy(gameObject);
    }
}

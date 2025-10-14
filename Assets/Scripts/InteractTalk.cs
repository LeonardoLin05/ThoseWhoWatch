using System.Collections;
using UnityEngine;
using TMPro;

public class InteractTalk : MonoBehaviour, IInteractable
{

    public string[] dialogo;
    private int i;
    public TextMeshProUGUI texto;
    public bool hablando = false;

    void Start()
    {
        if (texto == null)
        {
            texto = texto = GameObject.Find("texto_dialogo").GetComponent<TextMeshProUGUI>();
        }

        texto.gameObject.SetActive(false);
    }

    public IEnumerator interact()
    {
        if (!hablando)
        {
            hablando = true;
            i = 0;
            texto.text = dialogo[i];
            texto.gameObject.SetActive(true);

            VariablesGlobales.PARAR_CAMARA = true;
			VariablesGlobales.PARAR_MOVIMIENTO = true;
        }
        else
        {
            i++;
            if (i < dialogo.Length)
            {
                texto.text = dialogo[i];
            }
            else
            {
            hablando = false;
                texto.gameObject.SetActive(false);
            
                VariablesGlobales.PARAR_CAMARA = false;
			    VariablesGlobales.PARAR_MOVIMIENTO = false;
            }
        }

        yield return new WaitForSeconds(1.5f);
        VariablesGlobales.INTERACTUAR = true;
    }
    
    public string MensajeInteraccion()
    {
        if (!hablando)
        {
            return "Press E to talk";
        }
        else
        {
            return "Press E to continue";
        }
    }
}

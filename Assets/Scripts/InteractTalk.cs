using System.Collections;
using UnityEngine;
using TMPro;

public class InteractTalk : MonoBehaviour, IInteractable
{

    public string[] dialogo;
    private int i;
    public TextMeshProUGUI texto;
    public bool hablando = false;
    private Coroutine textoAnimado;
    
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
            texto.gameObject.SetActive(true);

            VariablesGlobales.PARAR_CAMARA = true;
            VariablesGlobales.PARAR_MOVIMIENTO = true;

            textoAnimado = StartCoroutine(textoAnimar(dialogo[i]));
        }
        else
        {
            if (textoAnimado != null)
            {
                StopCoroutine(textoAnimado);
                texto.text = dialogo[i];
                textoAnimado = null;
                VariablesGlobales.INTERACTUAR = true;
                yield break;
            }
            i++;
            if (i < dialogo.Length)
            {
                textoAnimado = StartCoroutine(textoAnimar(dialogo[i]));
            }
            else
            {
                hablando = false;
                texto.gameObject.SetActive(false);

                VariablesGlobales.PARAR_CAMARA = false;
                VariablesGlobales.PARAR_MOVIMIENTO = false;
            }
        }
        VariablesGlobales.INTERACTUAR = true;
    }
    
    private IEnumerator textoAnimar(string dial)
    {
        texto.text = "";

        for (int j = 0; j < dial.Length; j++)
        {
            texto.text = texto.text + dial[j];
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    public string MensajeInteraccion()
    {
        if (!hablando)
        {
            return "[E] para Hablar";
        }
        else
        {
            return "[E] para Continuar";
        }
    }
}

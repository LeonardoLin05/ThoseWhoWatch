using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


[System.Serializable]
public class DialogoFilas
{
    public string[] lineas;
}
[System.Serializable]
public class DialogosOpcion
{
    public string[] respuestas;
    public int[] saltar;
}
public class InteractNPCs : MonoBehaviour, IInteractable
{
    public DialogoFilas[] dialogos;
    public DialogosOpcion[] opciones;
    public int[] siguienteFila;
    public TextMeshProUGUI texto;
    public Button[] botones;
    public Button continueButton;
    private int fila = 0;
    private int i = 0;
    public bool hablando = false;
    private Coroutine textoAnimado;
    public bool puedeInteractuar = true;
    public bool opcionSecreta;
    public string textoSecreto;
    public int saltoSecreto;

    void Start()
    {
        if (texto == null)
        {
            texto = GameObject.Find("texto_dialogo").GetComponent<TextMeshProUGUI>();
        }

        for(int j = 0; j < botones.Length; j++)
        {
            botones[j].gameObject.SetActive(false);
        }
        texto.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        GameObject.Find("Image").GetComponent<Image>().enabled = false; 
    }

    public IEnumerator interact()
    {
        if (!puedeInteractuar)
        {
            yield break;
        }
        if (!hablando)
        {
            hablando = true;
            i = 0;
            texto.gameObject.SetActive(true);

            TalkZoomMoveCamera.Instance.setCabeza(transform);
            TalkZoomMoveCamera.Instance.StartZoomMovement(true);

            CameraMovement.Instance.enabled = false;
            PlayerMovement.Instance.enabled = false;
            HeadbobSystem.Instance.enabled = false;

            GameObject.Find("Image").GetComponent<Image>().enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            textoAnimado = StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
        }
        else
        {
            if(textoAnimado != null)
            {
                StopCoroutine(textoAnimado);
                texto.text = dialogos[fila].lineas[i];
                textoAnimado = null;

                if (i < dialogos[fila].lineas.Length - 1)
                {
                    continueButton.gameObject.SetActive(true);
                    continueButton.onClick.RemoveAllListeners();
                    continueButton.onClick.AddListener(() => StartCoroutine(AvanzarDialogo()));
                }
                VariablesGlobales.INTERACTUAR = true;
                yield break;
            }
        }
        VariablesGlobales.INTERACTUAR = true;
    }
    
    private IEnumerator AvanzarDialogo()
    {
        continueButton.gameObject.SetActive(false);
        i++;
        if (i < dialogos[fila].lineas.Length)
        {
            textoAnimado = StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
        }
        else
        {
            if (fila < opciones.Length && opciones[fila].respuestas.Length > 0)
            {

                MostrarOpcionesFila(fila);
            }
            else
            {
                if (fila < siguienteFila.Length && siguienteFila[fila] >= 0)
                {
                    FinDialogo();
                    fila = siguienteFila[fila];
                }
                else
                {
                    FinDialogo();
                    puedeInteractuar = false;
                }
            }
        }
        yield break;
    }

    private void MostrarOpcionesFila(int fila)
    {
        if (fila < opciones.Length && opciones[fila].respuestas.Length > 0)
        {
            MostrarOpciones(opciones[fila].respuestas, opciones[fila].saltar);
        }
        else
        {
            FinDialogo();
        }         
        
     }

    private void MostrarOpciones(string[] opciones, int[] saltar)
    {
        continueButton.gameObject.SetActive(false);
        GameObject.Find("Image").GetComponent<Image>().enabled = true; 

        if (opcionSecreta)
        {
            string[] opcionesNuevas = new string[opciones.Length + 1];
            int[] saltarNuevo = new int[saltar.Length + 1];

            for (int j = 0; j < opciones.Length; j++)
            {
                opcionesNuevas[j] = opciones[j];
                saltarNuevo[j] = saltar[j];
            }

            opcionesNuevas[opciones.Length] = textoSecreto;
            saltarNuevo[saltar.Length] = saltoSecreto;

            opciones = opcionesNuevas;
            saltar = saltarNuevo;
        }

        for (int i = 0; i < botones.Length; i++)
        {
            if (i < opciones.Length)
            {
                botones[i].gameObject.SetActive(true);
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];
                int salto = saltar[i];
                botones[i].onClick.RemoveAllListeners();
                botones[i].onClick.AddListener(() => SeleccionRespuesta(salto));
            }
            else
            {
                botones[i].gameObject.SetActive(false);
            }
        }
    }

    private void SeleccionRespuesta(int sigFila)
    {        
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }
        fila = sigFila;
        i = 0;
        textoAnimado = StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
    }
    
    private void FinDialogo()
    {
        hablando = false;
        texto.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TalkZoomMoveCamera.Instance.StartZoomMovement(false);

        CameraMovement.Instance.enabled = true;
        HeadbobSystem.Instance.enabled = true;
        PlayerMovement.Instance.enabled = true;

        GameObject.Find("Image").GetComponent<Image>().enabled = false;
    }

    public IEnumerator textoAnimar(string dial)
    {
        texto.text = "";

        for (int j = 0; j < dial.Length; j++)
        {
            texto.text = texto.text + dial[j];
            yield return new WaitForSeconds(0.05f);
        }
        if (i < dialogos[fila].lineas.Length - 1)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => StartCoroutine(AvanzarDialogo()));
        }
        else
        {
            if (fila < opciones.Length && opciones[fila].respuestas.Length > 0)
            {
                continueButton.gameObject.SetActive(false);
                MostrarOpcionesFila(fila);
            }
            else
            {
                continueButton.gameObject.SetActive(true);
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(() => StartCoroutine(AvanzarDialogo()));
            }
        }
    }

    public bool ocupado()
    {
        return hablando;
    }

    public string MensajeInteraccion()
    {
        if (!puedeInteractuar)
        {
            return "";
        }

        if (!hablando)
        {
            return "[E] para Hablar";
        }
        else
        {
            return "";
        }
    }
}
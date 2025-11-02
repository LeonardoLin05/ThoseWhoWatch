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
public enum EstadoBoton
{
    Oculto, Visble, Bloqueado
}
[System.Serializable]
public class BotonFila
{
    public EstadoBoton[] estado;
}
[System.Serializable]
public class BotonDialogo
{
    public Button boton;
}
public class InteractNPCs : MonoBehaviour, IInteractable
{
    public DialogoFilas[] dialogos;
    public DialogosOpcion[] opciones;
    public int[] siguienteFila;
    public TextMeshProUGUI texto;

    public BotonDialogo[] botones;
    public BotonFila[] botonesFila;
    public Button continueButton;

    private int fila = 0;
    private int i = 0;
    public bool hablando = false;
    private Coroutine textoAnimado;
    public bool puedeInteractuar = true;
    void Start()
    {
        if (texto == null)
        {
            texto = GameObject.Find("texto_dialogo").GetComponent<TextMeshProUGUI>();
        }

        for (int j = 0; j < botones.Length; j++)
        {
            botones[j].boton.gameObject.SetActive(false);
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
            TalkZoomMoveCamera.Instance.StartZoomMovement(50f);

            CameraMovement.Instance.enabled = false;
            PlayerMovement.Instance.enabled = false;
            HeadbobSystem.Instance.enabled = false;

            GameObject.Find("Image").GetComponent<Image>().enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            textoAnimado = StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
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
            bool opcionesVisibles = false;
            if(fila < opciones.Length && opciones[fila].respuestas.Length > 0)
            {
                for (int j = 0; j < botones.Length && j < opciones[fila].respuestas.Length; j++)
                {
                    if (botonesFila[fila].estado[j] == EstadoBoton.Visble)
                    {
                        opcionesVisibles = true;
                        break;
                    }
                }

                if (opcionesVisibles)
                {
                    MostrarOpcionesFila(fila);
                    yield break;
                }
            }

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

        bool opcionesVisibles = false;
        BotonFila estadosFila = botonesFila[fila];

        for (int i = 0; i < botones.Length; i++)
        {
            if (i < opciones.Length)
            {
                if (botonesFila[fila].estado[i] == EstadoBoton.Visble)
                {
                    opcionesVisibles = true;
                    botones[i].boton.gameObject.SetActive(true);
                    botones[i].boton.interactable = true;
                    botones[i].boton.GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];

                    int salto = saltar[i];
                    botones[i].boton.onClick.RemoveAllListeners();
                    botones[i].boton.onClick.AddListener(() => SeleccionRespuesta(salto));
                }
                else if (botonesFila[fila].estado[i] == EstadoBoton.Bloqueado)
                {
                    botones[i].boton.gameObject.SetActive(true);
                    botones[i].boton.interactable = false;
                    botones[i].boton.GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];
                }
                else if (botonesFila[fila].estado[i] == EstadoBoton.Oculto)
                {
                    botones[i].boton.gameObject.SetActive(false);
                }
            }
            else
            {
                botones[i].boton.gameObject.SetActive(false);
            }
        }

        if (!opcionesVisibles)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => StartCoroutine(AvanzarDialogo()));
        }
    }

    private void SeleccionRespuesta(int sigFila)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].boton.gameObject.SetActive(false);
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

        TalkZoomMoveCamera.Instance.StopZoomMovement();

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

    public void ActivarBoton(int i)
    {
        botonesFila[fila].estado[i] = EstadoBoton.Visble;
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
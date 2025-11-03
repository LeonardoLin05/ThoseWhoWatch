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
    Oculto, Visible, Bloqueado
}

[System.Serializable]
public class BotonFila
{
    public EstadoBoton[] estado;
}

public class InteractNPCs : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogoFilas[] dialogos;
    [SerializeField] private DialogosOpcion[] opciones;
    [SerializeField] private int[] siguienteFila;

    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private GameObject fondoTexto;

    [SerializeField] private Button[] botones;
    [SerializeField] private Button continueButton;
    [SerializeField] private BotonFila[] botonesFila;
    
    private int fila = 0;
    private int i = 0;
    
    void Start()
    {
        if (texto == null)
        {
            texto = GameObject.Find("texto_dialogo").GetComponent<TextMeshProUGUI>();
        }

        if(fondoTexto == null)
        {
            fondoTexto = GameObject.Find("Image");
        }

        for (int j = 0; j < botones.Length; j++)
        {
            botones[j].gameObject.SetActive(false);
        }

        texto.gameObject.SetActive(false);

        continueButton.gameObject.SetActive(false);
        
        fondoTexto.SetActive(false);
    }

    public IEnumerator interact()
    {
        i = 0;
        texto.gameObject.SetActive(true);
        fondoTexto.SetActive(true);

        TalkZoomMoveCamera.Instance.setCabeza(transform);
        TalkZoomMoveCamera.Instance.StartZoomMovement(50f);

        // Bloqueamos movimientos de camara, personaje e interaccion
        CameraMovement.Instance.enabled = false;
        PlayerMovement.Instance.enabled = false;
        HeadbobSystem.Instance.enabled = false;
        Interaction.Instance.enabled = false;
        Zoom.Instance.enabled = false;

        // Desbloquamos el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Añadimos la acción al boton de continuar
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => AvanzarDialogo());

        // Añadimos la acción correspondiente a los botones de respuestas
        for (int i = 0; i < botones.Length; i++)
        {
            int j = i;
            botones[i].onClick.RemoveAllListeners();
            botones[i].onClick.AddListener(() => SeleccionRespuesta(j));
        }

        StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
        yield break;
    }

    private void AvanzarDialogo()
    {
        continueButton.gameObject.SetActive(false);
        
        i++;

        if (i < dialogos[fila].lineas.Length)
        {
            StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
        }
        else
        {
            bool opcionesVisibles = false;
            if(fila < opciones.Length && opciones[fila].respuestas.Length > 0)
            {
                for (int j = 0; j < botones.Length && j < opciones[fila].respuestas.Length; j++)
                {
                    if (botonesFila[fila].estado[j] == EstadoBoton.Visible)
                    {
                        opcionesVisibles = true;
                        break;
                    }
                }

                if (opcionesVisibles)
                {
                    MostrarOpcionesFila();
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
                gameObject.layer = 0;
            }
        }
    }

    private void MostrarOpcionesFila()
    {
        if (fila < opciones.Length && opciones[fila].respuestas.Length > 0)
        {
            MostrarOpciones(opciones[fila].respuestas);
        }
        else
        {
            FinDialogo();
        }
    }

    private void MostrarOpciones(string[] opciones)
    {
        continueButton.gameObject.SetActive(false);

        bool opcionesVisibles = false;

        for (int i = 0; i < botones.Length; i++)
        {
            if (i < opciones.Length)
            {
                if (botonesFila[fila].estado[i] == EstadoBoton.Visible)
                {
                    opcionesVisibles = true;
                    botones[i].gameObject.SetActive(true);
                    botones[i].interactable = true;
                    botones[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];
                }
                else if (botonesFila[fila].estado[i] == EstadoBoton.Bloqueado)
                {
                    botones[i].gameObject.SetActive(true);
                    botones[i].interactable = false;
                    botones[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];
                }
            }
        }

        if (!opcionesVisibles)
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    private void SeleccionRespuesta(int sigFila)
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }

        fila = opciones[fila].saltar[sigFila];
        i = 0;
        
        StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
    }

    private void FinDialogo()
    {
        texto.gameObject.SetActive(false);
        fondoTexto.SetActive(false);
        continueButton.gameObject.SetActive(false);

        // Bloqueamos el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TalkZoomMoveCamera.Instance.StopZoomMovement();

        // Desbloqueamos moviemientos de camara, jugador e interaccion
        CameraMovement.Instance.enabled = true;
        HeadbobSystem.Instance.enabled = true;
        PlayerMovement.Instance.enabled = true;
        Interaction.Instance.enabled = true;
        Zoom.Instance.enabled = true;
    }

    public IEnumerator textoAnimar(string dial)
    {
        texto.text = "";

        for (int j = 0; j < dial.Length; j++)
        {
            texto.text = texto.text + dial[j];
            yield return new WaitForSecondsRealtime(0.04f);
        }
        
        // Hacer que los botones aparezcan después de que el texto termine
        if (i < dialogos[fila].lineas.Length - 1)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            if (fila < opciones.Length && opciones[fila].respuestas.Length > 0)
            {
                continueButton.gameObject.SetActive(false);
                MostrarOpcionesFila();
            }
            else
            {
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    public void ActivarBoton(int i)
    {
        botonesFila[fila].estado[i] = EstadoBoton.Visible;
    }

    public string MensajeInteraccion()
    {
        return "[E] para Hablar";
    }
}
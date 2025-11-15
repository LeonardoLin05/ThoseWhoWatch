using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    [SerializeField] private Button[] botones;
    [SerializeField] private Button continueButton;
    [SerializeField] private BotonFila[] botonesFila;

    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private GameObject fondoTexto;
    [SerializeField] private GameObject puntero;

    [SerializeField] private UnityEvent evento;

    [SerializeField] private TextMeshProUGUI textoInteraccion2;

    private CanvasGroup continueGroup;

    private string oldText;
    
    private int fila = 0;
    private int i = 0;
    
    private bool esperandoMovimiento = false;

    void Awake()
    {
        if (texto == null)
        {
            texto = GameObject.FindGameObjectWithTag("TextoDialogo").GetComponent<TextMeshProUGUI>();
        }
        if(fondoTexto == null)
        {
            fondoTexto = GameObject.FindGameObjectWithTag("FondoDialogo");
        }
        if (puntero == null)
        {
            puntero = GameObject.FindGameObjectWithTag("Puntero");
        }
        if(textoInteraccion2 == null)
        {
            textoInteraccion2 = GameObject.FindGameObjectWithTag("TextoInteractuar2").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        continueGroup = continueButton.GetComponent<CanvasGroup>();
        foreach (Button boton in botones)
        {
            boton.gameObject.SetActive(false);
        }

        fondoTexto.SetActive(false);
        texto.gameObject.SetActive(false);
        SetContinueButtonVisible(false);
    }

    void Update()
    {
        if (esperandoMovimiento && (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.001f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.001f))
        {
            Interaction.Instance.enabled = true;
            esperandoMovimiento = false;
        }
    }

    public IEnumerator interact()
    {
        i = 0;
        texto.gameObject.SetActive(true);
        fondoTexto.SetActive(true);
        puntero.gameObject.SetActive(false);

        TalkZoomMoveCamera.Instance.setCabeza(transform);
        TalkZoomMoveCamera.Instance.StartZoomMovement(50f);

        // Bloqueamos movimientos de camara, personaje e interaccion
        ActivarInstances(false);

        // Desbloquamos el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Añadimos la acción al boton de continuar
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => AvanzarDialogo());

        // Compruebas si el texto de la interacción 2 tiene algo escrito
        if (textoInteraccion2.text != "")
        {
            // Guarda el texto para reestablecerlo al final
            oldText = textoInteraccion2.text;
            textoInteraccion2.text = "";
        }

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
        SetContinueButtonVisible(false);        
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
                gameObject.layer = 0;
                FinDialogo();
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
        EstadoBoton boton;

        bool opcionesVisibles = false;
        int limite = Mathf.Min(botones.Length, botonesFila[fila].estado.Length, opciones.Length);

        for (int i = 0; i < limite; i++)
        {
            boton = botonesFila[fila].estado[i];

            if (boton != EstadoBoton.Oculto)
            {
                botones[i].gameObject.SetActive(true);
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];
                if (boton == EstadoBoton.Visible)
                {
                    opcionesVisibles = true;
                    botones[i].interactable = true;
                }
                // Si el botón esta bloqueado
                else botones[i].interactable = false;
            }
        }
        // Mostramos el botón de continuar en caso de que no haya ninguna opción en Visible
        if (!opcionesVisibles)
        {
            SetContinueButtonVisible(true);
        }
    }

    private void SeleccionRespuesta(int sigFila)
    {
        // Hacemos que ningún boton se vea en pantalla
        foreach(Button boton in botones)
        {
            boton.gameObject.SetActive(false);
        }

        fila = opciones[fila].saltar[sigFila];
        i = 0;
        
        StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
    }

    private void FinDialogo()
    {
        // Ejecutamos el evento si es que hay uno asignado
        // NOTA: solo se ejecuta si es la última conversación que puedes
        // hacer con el NPC, es decir, que ya no puedes volver a hablar con el NPC de nuevo
        if(gameObject.layer == 0 && evento != null)
        {
            evento.Invoke();
        }

        texto.gameObject.SetActive(false);
        fondoTexto.SetActive(false);
        SetContinueButtonVisible(false);
        puntero.SetActive(true);

        // Bloqueamos el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TalkZoomMoveCamera.Instance.StopZoomMovement();

        // Desbloqueamos moviemientos de camara, jugador e interaccion
        ActivarInstances(true);

        Interaction.Instance.enabled = false;
        esperandoMovimiento = true;

        if(oldText != null)
        {
            // Restablecemos el texto de la interacción 2
            textoInteraccion2.text = oldText;
            oldText = null;
        }
    }

    public IEnumerator textoAnimar(string dial)
    {
        texto.text = "";

        foreach(char letra in dial)
        {
            texto.text += letra;
            yield return VariablesGlobales.esperarTexto;
        }
        
        // Hacer que los botones aparezcan después de que el texto termine
        if (i < dialogos[fila].lineas.Length - 1)
        {
            SetContinueButtonVisible(true);
        }
        else
        {
            if (fila < opciones.Length && opciones[fila].respuestas.Length > 0)
            {
                SetContinueButtonVisible(false);
                MostrarOpcionesFila();
            }
            else
            {
                SetContinueButtonVisible(true);
            }
        }
    }

    /// <summary>
    /// Hace que un botón sea visible
    /// </summary>
    /// <param name="fila"> La fila donde se encuentra el botón </param>
    /// <param name="i"> Que botón de la fila es el que hay que activar </param>
    public void ActivarBoton(int fila, int i)
    {
        botonesFila[fila].estado[i] = EstadoBoton.Visible;
    }

    /// <summary>
    /// Hace que un botón se vuelva oculto
    /// </summary>
    /// <param name="fila"> La fila donde se encuentra el botón </param>
    /// <param name="i"> Que botón de la fila es el que hay que activar </param>
    public void DesactivarBoton(int fila, int i)
    {
        botonesFila[fila].estado[i] = EstadoBoton.Oculto;
    }

    private void SetContinueButtonVisible(bool visible)
    {
        if (visible)
        {
            continueGroup.alpha = 1f;
            continueGroup.interactable = true;
            continueGroup.blocksRaycasts = true;
        }
        else
        {
            continueGroup.alpha = 0f;
            continueGroup.interactable = false;
            continueGroup.blocksRaycasts = false;
            EventSystem.current.SetSelectedGameObject(null);
            continueButton.animator.Play("Normal", 0, 0f);
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(continueButton.gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }
    }
    
    /// <summary>
    /// Activa o desactiva una serie de scripts
    /// </summary>
    /// <param name="activar"> true para activar y false para desactivar </param>
    /// NOTA: no quitar el static o el public, se hace uso de esta función en NPCReact
    public static void ActivarInstances(bool activar)
    {
        PlayerMovement.Instance.enabled = activar;
        CameraMovement.Instance.enabled = activar;
        HeadbobSystem.Instance.enabled = activar;
        Interaction.Instance.enabled = activar;
        Zoom.Instance.enabled = activar;
    }

    /// <summary>
    /// Para llamarlo en el inspector a través de un UnityEvent
    /// </summary>
    public void EmpezarConversacion()
    {
        StartCoroutine(interact());
    }

    /// <summary>
    /// Para llamarlo en el inspector a través de un UnityEvent
    /// </summary>
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public string MensajeInteraccion()
    {
        return "[E] para Hablar";
    }
}
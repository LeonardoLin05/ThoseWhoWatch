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

    public UnityEvent evento;
    private bool ejecutado = false;

    public bool GetEjecutado()
    {
        return ejecutado;
    }

    public void SetEjecutado(bool estado)
    {
        ejecutado = estado;
    }
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
    [SerializeField] private bool activarAccion = false;
    // Por si quieres que el gameObject esté desactivado al empezar la escena
    [SerializeField] private bool activarStart = true;
    // Para la velocidad de giro de la cámara que hay para mirar al NPC
    [SerializeField] private float velocidadGiro = 50f;

    [SerializeField] private DialogoFilas[] dialogos;
    [SerializeField] private DialogosOpcion[] opciones;
    [SerializeField] private int[] siguienteFila;

    [SerializeField] private Button[] botones;
    [SerializeField] private Button continueButton;
    [SerializeField] private BotonFila[] botonesFila;

    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private GameObject fondoTexto;
    [SerializeField] private GameObject puntero;
    [SerializeField] private Transform npcHips;

    [SerializeField] private TextMeshProUGUI textoInteraccion2;

    private CanvasGroup continueGroup;
    private InteractPickUp objeto;
    private RotarNPC rotarNPC;

    private string oldText;
    
    private int fila = 0;
    private int i = 0;

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
        rotarNPC = gameObject.GetComponent<RotarNPC>();
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
        if(!activarStart)
        {
            gameObject.SetActive(false);
        }
        enabled = false;
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            texto.text = dialogos[fila].lineas[i];
            StartCoroutine(BotonesMostrar());
        }
    }

    public void Interact()
    {
        if(rotarNPC != null) rotarNPC.enabled = true;
        i = 0;
        texto.gameObject.SetActive(true);
        fondoTexto.SetActive(true);
        puntero.SetActive(false);

        // Impedimos que el jugador pueda lanzar el objeto en mitad de la conversación
        if(InteractPickUp.objetoEnMano)
        {
            objeto = GameObject.FindGameObjectWithTag("ObjetoEnMano").GetComponent<InteractPickUp>();
            objeto.enabled = false;
        }

        // Por si hay algun pensamiento
        if(Thoughts.Instance.gameObject.GetComponent<TextMeshProUGUI>().text != "")
        {
            Thoughts.Instance.enabled = false;
        }

        TalkZoomMoveCamera.Instance.setCabeza(npcHips);
        TalkZoomMoveCamera.Instance.StartZoomMovement(velocidadGiro);

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

        StartCoroutine(TextoAnimar(dialogos[fila].lineas[i]));
    }

    private void AvanzarDialogo()
    {
        SetContinueButtonVisible(false);        
        i++;

        if (i < dialogos[fila].lineas.Length)
        {
            StartCoroutine(TextoAnimar(dialogos[fila].lineas[i]));
        }
        else
        {
            if(rotarNPC != null) rotarNPC.enabled = false;
            // Si hay evento que ejecutar en la última fila de diálogo
            if(fila < opciones.Length) EjecutarEvento();

            if (fila < siguienteFila.Length && siguienteFila[fila] >= 0)
            {
                fila = siguienteFila[fila];   
            }
            else
            {
                if(activarAccion && InteractPickUp.objetoEnMano)
                {
                    objeto.ActivarAccion();
                }
                gameObject.layer = 0;
            }
            FinDialogo();
        }
    }

    private void MostrarRespuestas()
    {
        EstadoBoton boton;

        string[] opciones = this.opciones[fila].respuestas;
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
                    botones[i].interactable = true;
                }
                // Si el botón está bloqueado
                else botones[i].interactable = false;
            }
        }
    }

    private void SeleccionRespuesta(int respuesta)
    {
        // Hacemos que ningún botón se vea en pantalla
        foreach(Button boton in botones)
        {
            boton.gameObject.SetActive(false);
        }

        EjecutarEvento();

        fila = opciones[fila].saltar[respuesta];

        i = 0;
        StartCoroutine(TextoAnimar(dialogos[fila].lineas[i]));
    }

    private void EjecutarEvento()
    {
        DialogosOpcion opciones = this.opciones[fila];
        // Miramos si hay un evento a ejecutar y que no se haya ejecutado ya
        if(opciones.evento != null && !opciones.GetEjecutado())
        {
           opciones.evento.Invoke();
           opciones.SetEjecutado(true);
        }
    }

    private void FinDialogo()
    {
        texto.gameObject.SetActive(false);
        fondoTexto.SetActive(false);
        SetContinueButtonVisible(false);
        puntero.SetActive(true);

        // Permitimos al jugador poder tirar el objeto en mano
        if(InteractPickUp.objetoEnMano)
        {
            objeto.enabled = true;
        }

        // Por si había algún pensamiento
        if(!Thoughts.Instance.enabled == false)
        {
            Thoughts.Instance.enabled = true;
        }

        // Bloqueamos el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TalkZoomMoveCamera.Instance.StopZoomMovement();

        // Desbloqueamos moviemientos de camara, jugador e interaccion
        ActivarInstances(true);

        if(oldText != null)
        {
            // Restablecemos el texto de la interacción 2
            textoInteraccion2.text = oldText;
            oldText = null;
        }
    }

    public IEnumerator TextoAnimar(string dialogo)
    {
        enabled = true;

        texto.text = "";

        foreach(char letra in dialogo)
        {
            texto.text += letra;
            yield return VariablesGlobales.esperarTexto;
        }
        StartCoroutine(BotonesMostrar());
    }

    // Esta función se usa para mostrar los botones de continuar o respuesta después de que
    // el texto animado haya terminado
    private IEnumerator BotonesMostrar()
    {
        enabled = false;
        yield return new WaitForSecondsRealtime(0.01f);
        if (i < dialogos[fila].lineas.Length - 1 || !(fila < opciones.Length && opciones[fila].respuestas.Length > 0))
        {
            SetContinueButtonVisible(true);
        }
        else
        {
            MostrarRespuestas();
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

    public string MensajeInteraccion()
    {
        return "[E] para Hablar";
    }
}
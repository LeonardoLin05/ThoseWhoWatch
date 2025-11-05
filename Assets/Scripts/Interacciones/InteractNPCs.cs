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
    
    private int fila = 0;
    private int i = 0;
    private CanvasGroup continueGroup;
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
        if(puntero == null)
        {
            puntero = GameObject.FindGameObjectWithTag("Puntero");
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
        //EstadoBoton boton;

        bool opcionesVisibles = false;
        int limite = Mathf.Min(botones.Length, botonesFila[fila].estado.Length, opciones.Length);

        for (int i = 0; i < limite; i++)
        {
            /* Otra posible implementación, no se si es mejor o igual que la de abajo
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
                else if (boton == EstadoBoton.Bloqueado) botones[i].interactable = false;
            }
            */
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
        // Mostramos el botón de continuar en caso de que no hay ninguna opción en Visible
        if (!opcionesVisibles)
        {
            SetContinueButtonVisible(true);
        }
    }

    private void SeleccionRespuesta(int sigFila)
    {
        for(int i = 0; i < botones.Length; i++)
        {
            botones[i].gameObject.SetActive(false);
        }

        fila = opciones[fila].saltar[sigFila];
        i = 0;
        
        StartCoroutine(textoAnimar(dialogos[fila].lineas[i]));
    }

    private void FinDialogo()
    {
        if(evento != null)
        {
            Debug.Log("acabe");
            evento.Invoke();
            // Para que el evento solo pueda ocurrir una vez
            evento = null;
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
        CameraMovement.Instance.enabled = true;
        HeadbobSystem.Instance.enabled = true;
        PlayerMovement.Instance.enabled = true;
        Interaction.Instance.enabled = true;
        Zoom.Instance.enabled = true;

        Interaction.Instance.enabled = false;
        esperandoMovimiento = true;
    }

    public IEnumerator textoAnimar(string dial)
    {
        texto.text = "";

        foreach(char letra in dial)
        {
            texto.text += letra;
            yield return new WaitForSecondsRealtime(0.04f);
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

    public void ActivarBoton(int i)
    {
        botonesFila[fila].estado[i] = EstadoBoton.Visible;
    }

    private void SetContinueButtonVisible(bool visible)
    {
        continueGroup = continueButton.GetComponent<CanvasGroup>();
        
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
    public string MensajeInteraccion()
    {
        return "[E] para Hablar";
    }
}
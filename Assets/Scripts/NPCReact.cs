using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NPCReact : MonoBehaviour
{
    // Una o más frases que dice el NPC al reaccionar
    [SerializeField] private string[] frase;
    // El NPC que va a reaccionar
    [SerializeField] private Transform NPC;
    // Fondo dialogo
    [SerializeField] private GameObject fondoDialogo;
    // Texto interaccion (no la 2)
    [SerializeField] private TextMeshProUGUI textoInteraccion;
    // Texto del dialogo
    [SerializeField] private TextMeshProUGUI texto;
    // Boton de respuesta/continuar
    [SerializeField] private Button boton;
    // Puntero en pantalla
    [SerializeField] private GameObject puntero;

    private CanvasGroup continueGroup;

    private string oldText;

    // Lleva la cuenta de cuantas veces se ha ejecutado el script
    // para ir aumentando a la siguiente frase que se vaya a decir
    // Ejemplo funcionalidad: para que vaya diciendo frases cada vez más enfadado
    private static int vecesEjecutadas = 0;

    void Awake()
    {
        if (fondoDialogo == null)
        {
            fondoDialogo = GameObject.FindGameObjectWithTag("FondoDialogo");
        }
        if (texto == null)
        {
            texto = GameObject.FindGameObjectWithTag("TextoDialogo").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        if (puntero == null)
        {
            puntero = GameObject.FindGameObjectWithTag("Puntero");
        }
        if (textoInteraccion == null)
        {
            textoInteraccion = GameObject.FindGameObjectWithTag("TextoInteractuar").GetComponent<TextMeshProUGUI>();
        }
        
        continueGroup = boton.GetComponent<CanvasGroup>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Solo se puede realizar si las interacciones estan activadas
        if(Interaction.Instance.isActiveAndEnabled)
        {
            if (collision.gameObject.CompareTag("SueloGasolinera"))
            {
                StartConversation();
            } 
        }
    }

    private void StartConversation()
    {
        // Bloqueamos movimiento de la camara, jugador e interaccion
        InteractNPCs.ActivarInstances(false);

        // Desbloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Forzamos al jugar a mirar al NPC
        TalkZoomMoveCamera.Instance.setCabeza(NPC);
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f);

        texto.gameObject.SetActive(true);
        fondoDialogo.SetActive(true);

        puntero.SetActive(false);

        // Comprobamos si el texto de interaccion tiene algo escrito
        if (textoInteraccion.text != "")
        {
            // Lo guardamos para reestablecerlo más adelante
            oldText = textoInteraccion.text;
            textoInteraccion.text = "";
        }

        // Para que el texto aparezca de poco a poco animado
        StartCoroutine(TextoAnimado(frase[vecesEjecutadas]));

        if(vecesEjecutadas < frase.Length - 1)
        {
            vecesEjecutadas++;
        }

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(() => StopConversation());

    }

    private void StopConversation()
    {
        // Desbloqueamos movimiento de la camera, jugador e interaccion
        InteractNPCs.ActivarInstances(true);

        // Bloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        SetContinueButtonVisible(false);

        texto.text = "";
        texto.gameObject.SetActive(false);
        fondoDialogo.SetActive(false);

        puntero.SetActive(true);

        if(oldText != null)
        {
            // Restablecemos el texto que había en interacción
            textoInteraccion.text = oldText;
            oldText = null;
        }

        // Liberamos al jugador de mirar al NPC
        TalkZoomMoveCamera.Instance.StopZoomMovement();
    }

    private IEnumerator TextoAnimado(string frase)
    {
        texto.text = "";

        foreach (char letra in frase)
        {
            texto.text += letra;
            yield return new WaitForSecondsRealtime(0.04f);
        }
        SetContinueButtonVisible(true);
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
            boton.animator.Play("Normal", 0, 0f);
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(boton.gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }
    }
}

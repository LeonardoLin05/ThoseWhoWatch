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
    // Texto del dialogo
    [SerializeField] private TextMeshProUGUI texto;
    // Boton de respuesta/continuar
    [SerializeField] private Button boton;
    // Canvas interaccion
    [SerializeField] private GameObject interaccion;

    [SerializeField] private AudioSource sonidoTexto;

    private CanvasGroup continueGroup;

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
        enabled = false;
    }

    void Start()
    {   
        continueGroup = boton.GetComponent<CanvasGroup>();
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            texto.text = frase[vecesEjecutadas];
            StartCoroutine(SetContinueButtonVisible(true));
            enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Solo se puede realizar si las interacciones estan activadas
        if(Interaction.Instance.isActiveAndEnabled)
        {
            if (NPC != null && collision.gameObject.CompareTag("SueloGasolinera"))
            {
                StartConversation();
            } 
        }
    }

    private void StartConversation()
    {
        // Bloqueamos movimiento de la camara, jugador e interaccion
        InteractNPCs.ActivarInstances(false);

        // Por si hay algun pensamiento
        if(Thoughts.Instance.gameObject.GetComponent<TextMeshProUGUI>().text != "")
        {
            Thoughts.Instance.enabled = false;
        }

        // Desbloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Forzamos al jugar a mirar al NPC
        TalkZoomMoveCamera.Instance.SetCabeza(NPC);
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f, true);

        texto.gameObject.SetActive(true);
        fondoDialogo.SetActive(true);

        interaccion.SetActive(false);

        // Para que el texto aparezca de poco a poco animado
        StartCoroutine(TextoAnimado());

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(() => StopConversation());

    }

    private void StopConversation()
    {
        if(vecesEjecutadas < frase.Length - 1)
        {
            vecesEjecutadas++;
        }

        // Desbloqueamos movimiento de la camera, jugador e interaccion
        InteractNPCs.ActivarInstances(true);

        // Por si había algún pensamiento
        if(!Thoughts.Instance.enabled)
        {
            Thoughts.Instance.enabled = true;
        }

        // Bloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        StartCoroutine(SetContinueButtonVisible(false));

        texto.text = "";
        texto.gameObject.SetActive(false);
        fondoDialogo.SetActive(false);

        interaccion.SetActive(true);

        // Liberamos al jugador de mirar al NPC
        TalkZoomMoveCamera.Instance.StopZoomMovement();
    }

    private IEnumerator TextoAnimado()
    {
        enabled = true;

        texto.text = "";

        foreach (char letra in frase[vecesEjecutadas])
        {
            sonidoTexto.Play();
            texto.text += letra;
            yield return VariablesGlobales.esperarTexto;
        }
        enabled = false;
        StartCoroutine(SetContinueButtonVisible(true));
    }

    private IEnumerator SetContinueButtonVisible(bool visible)
    {
        if (visible)
        {
            yield return new WaitForSecondsRealtime(0.01f);
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

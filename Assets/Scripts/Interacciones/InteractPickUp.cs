using UnityEngine;
using TMPro;
using System.Collections;

public class InteractPickUp : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform mano;

    private bool lanzar = false;

    public static bool objetoEnMano = false;

    private bool activarAccion = false;
    private Animator accion;
    private AudioSource sonidoBeber;

    private Rigidbody objeto;
    private TextMeshProUGUI texto;
    private TextMeshProUGUI texto2;

    public InteractNPCs npc;

    [SerializeField] private bool desbloquear = false;
    [SerializeField] private int indice;
    [SerializeField] private int fila;

    void Awake()
    {
        if(enabled != false)
        {
            enabled = false;
        }
        accion = gameObject.GetComponent<Animator>();
        sonidoBeber = gameObject.GetComponent<AudioSource>();
        objeto = gameObject.GetComponent<Rigidbody>();

        texto = GameObject.FindGameObjectWithTag("TextoInteractuar2").GetComponent<TextMeshProUGUI>();
        texto2 = GameObject.FindGameObjectWithTag("TextoInteractuar3").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        mano = GameObject.FindGameObjectWithTag("Mano").GetComponent<Transform>();
    }

    public void Interact()
    {
        if (!objetoEnMano)
        {
            objetoEnMano = true;
            gameObject.tag = "ObjetoEnMano";

            // Ponemos el objeto en la mano del jugador
            objeto.isKinematic = true;
            transform.SetParent(mano);
            transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            Physics.SyncTransforms();

            texto.text = "[G] para Lanzar";
            enabled = true;

            // Si activa alguna respuesta oculta al recogerlo
            if (npc != null && npc.gameObject.layer == 6 && desbloquear)
            {
                npc.ActivarBoton(fila, indice);
            }
        }
        else
        {
            Thoughts.Instance.StartThoughts("Tenía las manos llenas");
        }
    }

    public string MensajeInteraccion()
    {
        return "[E] para Recoger";
    }

    // Update is called once per frame
    void Update()
    {
        if (objetoEnMano && Input.GetKeyDown(KeyCode.G))
        {
            Lanzar();
        }
        // Para la acción de beber la botella de agua
        else if(activarAccion && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(AccionBeber());
        }
    }

    void FixedUpdate()
    {
        if (lanzar)
        {
            // NO poner la fuerza a más de 10f por favor
            objeto.AddForce(mano.transform.forward * 6f, ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        enabled = false;
        lanzar = false;
    }

    void OnDestroy()
    {
        texto.text = "";
        texto2.text = "";
        objetoEnMano = false;
    }

    private IEnumerator AccionBeber()
    {
        texto.text = "";
        texto2.text = "";

        accion.enabled = true;
        enabled = false;
        Interaction.Instance.enabled = false;

        sonidoBeber.Play();

        // Esperar lo que dura la animación
        yield return new WaitForSecondsRealtime(5f);
        // Para que el FixedUpdate se ejecute
        enabled = true;
        Interaction.Instance.enabled = true;
        accion.enabled = false;

        lanzar = true;
        Lanzar();
        // El objeto ya no puedes volver a recogerlo
        gameObject.layer = 0;
    }

    public void ActivarAccion()
    {
        activarAccion = true;
        texto2.text = "[R para beber]";
    }

    private void Lanzar()
    {
        objeto.isKinematic = false;
        gameObject.tag = "Untagged";

        lanzar = true;
        mano.transform.DetachChildren();
        texto.text = "";
        texto2.text = "";

        objetoEnMano = false;

        if (npc != null && npc.gameObject.layer == 6 && desbloquear)
        {
            npc.DesactivarBoton(fila, indice);
        }
    }
}
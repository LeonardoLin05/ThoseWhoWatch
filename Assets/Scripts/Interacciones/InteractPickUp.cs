using UnityEngine;
using TMPro;

public class InteractPickUp : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform mano;

    private bool lanzar = false;

    public static bool objetoEnMano = false;

    private Rigidbody objeto;
    private TextMeshProUGUI texto;
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
        objeto = gameObject.GetComponent<Rigidbody>();
        texto = GameObject.FindGameObjectWithTag("TextoInteractuar2").GetComponent<TextMeshProUGUI>();
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

    private void Lanzar()
    {
        objeto.isKinematic = false;

        lanzar = true;
        mano.transform.DetachChildren();
        texto.text = "";

        objetoEnMano = false;

        if (npc != null && npc.gameObject.layer == 6 && desbloquear)
        {
            npc.DesactivarBoton(fila, indice);
        }
    }
}
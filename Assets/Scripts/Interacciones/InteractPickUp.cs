using System.Collections;
using UnityEngine;
using TMPro;

public class InteractPickUp : MonoBehaviour, IInteractable
{
    private Transform mano;

    private bool interactuar = false;
    private bool lanzar = false;
    private bool enMano = false;

    private Transform posicion;
    private Rigidbody objeto;
    private BoxCollider boxCollider;
    private TextMeshProUGUI texto;
    public InteractNPCs npc;

    public IEnumerator interact()
    {
        if (!enMano)
        {
            enabled = true;
            interactuar = true;
        }
        VariablesGlobales.INTERACTUAR = true;
        yield break;
    }

    public string MensajeInteraccion()
    {
        if (!enMano)
            return "[E] para Recoger";
        else
            return "";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enabled = false;
        objeto = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Start()
    {
        mano = GameObject.Find("Mano").GetComponent<Transform>();
        texto = GameObject.Find("texto_interactuar2").GetComponent<TextMeshProUGUI>();
        posicion = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactuar)
        {
            Recoger();
        }
        else if (enMano && Input.GetKeyDown(KeyCode.G))
        {
            Lanzar();
        }
        if (enMano)
        {
            CameraMovement.Instance.GirarObjeto(transform);
        }
        transform.position = posicion.position;
        Physics.SyncTransforms();
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

    private void Recoger()
    {
        objeto.useGravity = false;
        objeto.freezeRotation = true;
        objeto.linearVelocity = new Vector3(0, 0, 0);
        objeto.rotation = Quaternion.Euler(0,0,0);

        boxCollider.enabled = false;
        interactuar = false;
        lanzar = false;
        enMano = true;

        posicion = mano;
        texto.text = "[G] para Lanzar";

        if(npc != null)
        {
            npc.opcionSecreta = true;
        }
    }
    
    private void Lanzar()
    {
        objeto.useGravity = true;
        objeto.freezeRotation = false;

        lanzar = true;
        boxCollider.enabled = true;
        posicion = transform;
        texto.text = "";

        enMano = false;
    }
}
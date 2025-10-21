using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class InteractPickUp : MonoBehaviour, IInteractable
{
    public Transform mano;

    private bool interactuar = false;
    private bool lanzar = false;
    private bool enMano = false;
    private Transform posicion;
    private Rigidbody objeto;
    private BoxCollider boxCollider;
    private TextMeshProUGUI texto;

    public IEnumerator interact()
    {
        if (!VariablesGlobales.OBJETO_MANO)
        {
            interactuar = true;
        }
        VariablesGlobales.INTERACTUAR = true;
        yield break;
    }

    public string MensajeInteraccion()
    {
        if (!VariablesGlobales.OBJETO_MANO)
            return "[E] para Recoger";
        else
            return "";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        objeto = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Start()
    {
        texto = GameObject.Find("texto_interactuar2").GetComponent<TextMeshProUGUI>();
        posicion = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactuar)
        {
            objeto.useGravity = false;
            objeto.freezeRotation = true;
            objeto.linearVelocity = new Vector3(0, 0, 0);
            objeto.rotation = Quaternion.Euler(0,0,0);

            boxCollider.enabled = false;
            interactuar = false;
            lanzar = false;

            enMano = true;
            VariablesGlobales.OBJETO_MANO = true;

            posicion = mano;
            texto.text = "[G] para Lanzar";
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            objeto.useGravity = true;
            objeto.freezeRotation = false;

            lanzar = true;
            boxCollider.enabled = true;
            posicion = transform;
            texto.text = "";

            enMano = false;
            VariablesGlobales.OBJETO_MANO = false;
        }
        if (enMano)
        {
            CameraMovement.GirarObjeto(transform);
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
        lanzar = false;
    }
}
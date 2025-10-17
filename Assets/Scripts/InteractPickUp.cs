using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class InteractPickUp : MonoBehaviour, IInteractable
{
    public Transform mano;

    private Boolean enMano = false;
    private Transform posicion;
    private Rigidbody objeto;
    private BoxCollider boxCollider;
    private TextMeshProUGUI texto;

    public IEnumerator interact()
    {
        enMano = true;
        VariablesGlobales.INTERACTUAR = true;
        yield break;
    }

    public string MensajeInteraccion()
    {
        if (!enMano)
            return "Press E to pick up";
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
        if (enMano)
        {
            objeto.useGravity = false;
            boxCollider.enabled = false;
            enMano = false;

            posicion = mano;
            texto.gameObject.SetActive(true);
            texto.text = "Presiona G para soltar";
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            objeto.useGravity = true;
            boxCollider.enabled = true;
            posicion = transform;
            texto.gameObject.SetActive(false);
        }
        objeto.position = posicion.position;
        Physics.SyncTransforms();
    }
}

using System.Collections;
using UnityEngine;
using TMPro;

public class Thoughts : MonoBehaviour
{
    public static Thoughts Instance { get; private set; }

    // Texto donde se va a mostrar el pensamiento
    [SerializeField] private TextMeshProUGUI pensamiento;
    [SerializeField] private TextMeshProUGUI interactuar;

    private bool enPensamiento = false;

    // Variable donde guardamos la corutina que se ejecuta para poder pararlo
    private Coroutine coroutine;

    // Tiempo que se espera hasta que desaparezca el pensamiento
    private WaitForSecondsRealtime esperarPensamiento = new(5f);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if(pensamiento == null)
        {
            pensamiento = gameObject.GetComponent<TextMeshProUGUI>();
        }
    }

    void OnDisable()
    {
        pensamiento.text = "";
        if(enPensamiento)
        {
            StopCoroutine(coroutine);
            esperarPensamiento.Reset();
        }
    }

    public void StartThoughts(string pensamiento)
    {
        // Si ya estaba un pensamiento en ejecución
        if(enPensamiento)
        {
            // Paramos la corutina asociada al pensamiento que estaba ya en ejecución
            StopCoroutine(coroutine);
            esperarPensamiento.Reset();
        }
        coroutine = StartCoroutine(MostrarPensamiento(pensamiento));
    }

    public void StartInstruction(string instruction)
    {
        StartCoroutine(MostrarInstruccion(instruction));
    }

    private IEnumerator MostrarPensamiento(string pensamiento)
    {
        enPensamiento = true;
        this.pensamiento.text = pensamiento;
        yield return esperarPensamiento;
        this.pensamiento.text = "";
        enPensamiento = false;
    }

    private IEnumerator MostrarInstruccion(string instruction)
    {
        interactuar.text = instruction;
        yield return esperarPensamiento;
        interactuar.text = "";
    }
}
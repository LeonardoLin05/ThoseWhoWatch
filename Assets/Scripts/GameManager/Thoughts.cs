using System.Collections;
using UnityEngine;
using TMPro;

public class Thoughts : MonoBehaviour
{
    public static Thoughts Instance { get; private set; }

    // Texto donde se va a mostrar el pensamiento
    [SerializeField] private TextMeshProUGUI pensamiento;

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

    public void StartThoughts(string pensamiento)
    {
        // Si ya estaba un pensamiento en ejecución
        if(enPensamiento == true)
        {
            // Paramos la corutina asociada al pensamiento que estaba ya en ejecución
            StopCoroutine(coroutine);
            esperarPensamiento.Reset();
        }
        coroutine = StartCoroutine(mostrarPensamiento(pensamiento));
    }

    private IEnumerator mostrarPensamiento(string pensamiento)
    {
        enPensamiento = true;
        this.pensamiento.text = pensamiento;
        yield return esperarPensamiento;
        this.pensamiento.text = "";
        enPensamiento = false;
    }
}
using System.Collections;
using TMPro;
using UnityEngine;

public class ThoughtsNarration : MonoBehaviour
{

    [SerializeField] private string[] lineas;
    [SerializeField] private bool cambiaHora;
    [SerializeField] private string hora;
    [SerializeField] private string nuevaHora;

    private TextMeshProUGUI pensamientos;
    private TextMeshProUGUI textoInteractuar2;

    [SerializeField] private TextMeshProUGUI textoHora;

    private Animator fade;

    private int numeroLinea;

    private WaitForSecondsRealtime entreLetra = new(0.5f);
    private WaitForSecondsRealtime dosSeg = new(2f);

    void Awake()
    {
        pensamientos = gameObject.GetComponent<TextMeshProUGUI>();
        numeroLinea = 1;
    }

    void Start()
    {
        CameraMovement.Instance.enabled = false;
        PlayerMovement.Instance.enabled = false;
        Interaction.Instance.enabled = false;
        Zoom.Instance.enabled = false;

        fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        textoInteractuar2 = GameObject.Find("texto_interactuar2").GetComponent<TextMeshProUGUI>();

        // Comprobamos si lineas tiene algo escrito
        if (lineas.Length > 0)
        {
            textoInteractuar2.text = "[Espacio] para avanzar";
            pensamientos.SetText(lineas[0]);
        }
        // En caso contrario salimos del pensamiento
        else
        {
            SalirPensamiento();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AvanzarPensamiento();
        }
    }

    private void AvanzarPensamiento()
    {
        if (numeroLinea >= lineas.Length)
        {
            SalirPensamiento();
        }
        else
        {
            // Usar la palabra saltar para empezar de nuevo con el siguiente texto borrando
            // todo lo anterior
            if (lineas[numeroLinea].CompareTo("saltar") == 0)
            {
                pensamientos.text = lineas[++numeroLinea];
            }
            else
            {
                pensamientos.text = pensamientos.text + "\n" + lineas[numeroLinea];
            }
            numeroLinea++;
        }
    }
    
    private void SalirPensamiento()
    {
        enabled = false;
        pensamientos.text = "";
        textoInteractuar2.text = "";
        StartCoroutine(HoraAnimada());
        fade.SetTrigger("FadeOut");
    }

    private IEnumerator HoraAnimada()
    {
        foreach(char letra in hora)
        {
            textoHora.text += letra;
            if(letra != ' ') yield return entreLetra;
        }
        if(cambiaHora) {
            yield return entreLetra;
            textoHora.text = nuevaHora;
        }
        yield return dosSeg;
        textoHora.text = "";
        CameraMovement.Instance.enabled = true;
        PlayerMovement.Instance.enabled = true;
        Interaction.Instance.enabled = true;
        Zoom.Instance.enabled = true;
    }
}

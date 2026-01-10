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

    [SerializeField] private AudioSource alarma;
    [SerializeField] private AudioSource sirena;
    [SerializeField] private GameObject creditos;

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
        if(hora != "" ) InteractNPCs.ActivarInstances(false);

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
            if (sirena != null && numeroLinea == 21)
            {
                StartCoroutine(ReproducirAuidio(sirena, 0.5f, 0.4f)); 
            }
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
        if(hora != "") StartCoroutine(HoraAnimada());

        if (sirena == null)
        {
            fade.SetTrigger("FadeOut");
        }
        else
        {
            creditos.SetActive(true); 
            AudioSource musicaFianl = GameObject.Find("Creditos").GetComponent<AudioSource>();
            musicaFianl.time = 77f; 
            StartCoroutine(ReproducirAuidio(musicaFianl, 18f, 1f)); 
        }
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
            if (alarma != null)
            {
                alarma.Play();
            }
        }
        yield return dosSeg;
        textoHora.text = "";
        CameraMovement.Instance.enabled = true;
        if (alarma != null)
        {
            CameraMovement.Instance.lockY = true;
        }
        else
        {
            PlayerMovement.Instance.enabled = true;
            HeadbobSystem.Instance.enabled = true;
        }
        GamePause.Instance.enabled = true;
        Interaction.Instance.enabled = true;
        Zoom.Instance.enabled = true;
    }

    private IEnumerator ReproducirAuidio(AudioSource audio, float duracion, float volumen)
    {
        float fadeOut = 3f; 
        float t = 0f;

        audio.Play();
        yield return new WaitForSeconds(duracion);

        while (t < fadeOut) 
        { 
            t += Time.deltaTime; 
            audio.volume = Mathf.Lerp(volumen, 0f, t / fadeOut); 
            yield return null; 
        }
        audio.Stop(); 
    }
}

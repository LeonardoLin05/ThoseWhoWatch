using System.Collections;
using UnityEngine;

public class TirarPuerta : MonoBehaviour
{
    [SerializeField] private AudioSource sonidoPuerta;
    [SerializeField] private AudioSource sonidoPuertaViolento;
    [SerializeField] private AudioSource sonidoMusica;

    [SerializeField] private FollowPath script;

    private Animator animacion;

    private WaitForSecondsRealtime treSegundos = new (3f);

    void Awake()
    {
        animacion = gameObject.GetComponent<Animator>();
    }

    public void StartEvent()
    {
        animacion.SetTrigger("takeDown");
        StartCoroutine(TimingSonido());
    }

    private IEnumerator TimingSonido()
    {
        sonidoMusica.Play();
        for(int i = 0; i < 5; i++)
        {
            sonidoPuerta.Play();
            yield return treSegundos;
        }
        sonidoPuertaViolento.Play();
        sonidoMusica.Stop();
        script.enabled = true;
    }
}

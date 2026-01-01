using System.Collections;
using UnityEngine;

public class TirarPuerta : MonoBehaviour
{
    [SerializeField] private AudioSource sonidoPuerta;
    [SerializeField] private AudioSource sonidoPuertaViolento;
    [SerializeField] private AudioSource sonidoMusica;
    [SerializeField] private Animator animacionNPC;

    [SerializeField] private FollowPath script;

    private Animator animacion;

    private WaitForSecondsRealtime treSegundos = new (3f);

    void Awake()
    {
        animacion = gameObject.GetComponent<Animator>();
    }

    public void StartEvent()
    {
        EsconderseArmario.DESACTIVAR_ARMARIO = true;
        animacion.SetTrigger("takeDown");
        StartCoroutine(TimingSonido());
    }

    private IEnumerator TimingSonido()
    {
        sonidoMusica.Play();
        for(int i = 0; i < 5; i++)
        {
            sonidoPuerta.Play();
            animacionNPC.SetTrigger("kick");
            yield return treSegundos;
        }
        animacionNPC.SetTrigger("idle");
        sonidoPuertaViolento.Play();
        sonidoMusica.Stop();
        yield return new WaitForSecondsRealtime(1f);
        script.enabled = true;
        animacionNPC.SetTrigger("walking");
    }
}

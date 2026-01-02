using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TirarPuerta : MonoBehaviour
{
    [SerializeField] private AudioSource sonidoPuerta;
    [SerializeField] private AudioSource sonidoPuertaViolento;
    [SerializeField] private AudioSource sonidoMusica;
    [SerializeField] private AudioSource heartbeat;
    [SerializeField] private Animator animacionNPC;

    [SerializeField] private FollowPath script1;
    [SerializeField] private ChasePlayer script2;

    private Animator animacionPuerta;

    private WaitForSecondsRealtime treSegundos = new (3f);

    void Awake()
    {
        animacionPuerta = gameObject.GetComponent<Animator>();
    }

    public void StartEvent()
    {
        EsconderseArmario.DESACTIVAR_ARMARIO = true;
        animacionPuerta.SetTrigger("takeDown");
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
        if(EsconderseArmario.DENTRO_ARMARIO) {
            script1.enabled = true;
            heartbeat.Play();
            animacionNPC.SetTrigger("walking");
        }
        else
        {
            script2.enabled = true;
            animacionNPC.SetTrigger("runFast");
        }
    }
}

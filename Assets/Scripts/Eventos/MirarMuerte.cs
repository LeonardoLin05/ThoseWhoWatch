using System.Collections;
using RetroTVFX;
using UnityEngine;
using UnityEngine.AI;

public class MirarMuerte : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CRTEffect efectoCamara;
    [SerializeField] private RetryMenu retryMenu;
    [SerializeField] private GameObject sangre;
    [SerializeField] private AudioSource stab;

    private GameObject puntero;

    private NavMeshAgent navMeshAgent;
    private ChasePlayer perseguirScript;
    private ReducirVolumen reducirVolumenScript;
    private RotarNPC rotarNPCScript;
    private Animator animacionNPC;

    void Awake()
    {
        rotarNPCScript = gameObject.GetComponent<RotarNPC>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        perseguirScript = gameObject.GetComponent<ChasePlayer>();
        animacionNPC = gameObject.GetComponent<Animator>();
        reducirVolumenScript = gameObject.GetComponent<ReducirVolumen>();
    }

    void Start()
    {
        puntero = GameObject.FindGameObjectWithTag("Puntero");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EventCoroutine());
        }
    }

    private IEnumerator EventCoroutine()
    {
        rotarNPCScript.enabled = true;
        puntero.SetActive(false);

        perseguirScript.enabled = false;
        navMeshAgent.enabled = false;

        // Animacion ataque
        animacionNPC.SetTrigger("attack");

        // Efecto camara
        efectoCamara.VideoMode = VideoType.RF;

        // Movimiento mirar NPC sin zoom
        TalkZoomMoveCamera.Instance.SetCabeza(target);
        TalkZoomMoveCamera.Instance.StartZoomMovement(300f, false);
        stab.Play();

        InteractNPCs.ActivarInstances(false);
        yield return new WaitForSeconds(2f);
        sangre.SetActive(true);
        reducirVolumenScript.enabled = true;
        yield return new WaitForSeconds(2f);
        stab.Stop();
        sangre.SetActive(false);
        retryMenu.ShowMenu();
    }
}

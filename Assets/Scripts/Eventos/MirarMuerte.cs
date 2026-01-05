using System.Collections;
using RetroTVFX;
using UnityEngine;
using UnityEngine.AI;

public class MirarMuerte : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CRTEffect efectoCamara;
    [SerializeField] private RetryMenu retryMenu;

    private NavMeshAgent navMeshAgent;
    private ChasePlayer perseguirScript;
    private ReducirVolumen reducirVolumenScript;
    private Animator animacionNPC;

    void Awake()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        perseguirScript = gameObject.GetComponent<ChasePlayer>();
        animacionNPC = gameObject.GetComponent<Animator>();
        reducirVolumenScript = gameObject.GetComponent<ReducirVolumen>();
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
        perseguirScript.enabled = false;
        navMeshAgent.enabled = false;
        animacionNPC.SetTrigger("attack");
        efectoCamara.VideoMode = VideoType.RF;
        TalkZoomMoveCamera.Instance.SetCabeza(target);
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f, false);
        InteractNPCs.ActivarInstances(false);
        yield return new WaitForSeconds(2f);
        reducirVolumenScript.enabled = true;
        yield return new WaitForSeconds(2f);
        retryMenu.ShowMenu();
    }
}

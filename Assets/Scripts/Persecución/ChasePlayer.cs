using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private AudioSource screamer;
    [SerializeField] private AudioSource musicaPersecucion;
    [SerializeField] private StressReceiver stressReceiverScript;
    [SerializeField] private TraumaInducer traumaInducerScritp;

    private NavMeshAgent npc;

    private bool inCoroutine = false;

    void OnEnable()
    {
        npc.enabled = true;
    }

    void Awake()
    {
        npc = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(!inCoroutine && Vector3.Distance(npc.transform.position, target.position) <= 7)
        {
            StartCoroutine(StartScream());
        }
        npc.destination = target.position;
    }

    private IEnumerator StartScream()
    {
        inCoroutine = true;

        if(musicaPersecucion.isPlaying) {
            musicaPersecucion.Stop();
            yield return new WaitForSeconds(1f);
        }
        screamer.Play();

        // Desactivamos el headbob y hacemos que la camara
        // se agite dependiendo de la distancia con el NPC
        HeadbobSystem.Instance.enabled = false;
        stressReceiverScript.enabled = true;
        traumaInducerScritp.enabled = true;
    }
}

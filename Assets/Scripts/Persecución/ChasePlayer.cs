using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private AudioSource screamer;
    [SerializeField] private StressReceiver stressReceiverScript;
    [SerializeField] private TraumaInducer traumaInducerScritp;

    private NavMeshAgent npc;

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
        if(!screamer.isPlaying && Vector3.Distance(npc.transform.position, target.position) <= 7)
        {
            screamer.Play();
            // Desactivamos el headbob y hacemos que la camara
            // se agite dependiendo de la distancia con el NPC
            HeadbobSystem.Instance.enabled = false;
            stressReceiverScript.enabled = true;
            traumaInducerScritp.enabled = true;
        }
        npc.destination = target.position;
    }
}

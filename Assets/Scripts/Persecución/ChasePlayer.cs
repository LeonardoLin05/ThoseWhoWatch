using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private AudioSource screamer;

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
        if(!screamer.isPlaying && Vector3.Distance(npc.transform.position, target.position) < 5)
        {
            screamer.Play();
        }
        npc.destination = target.position;
    }
}

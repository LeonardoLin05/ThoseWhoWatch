using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform target;

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
        npc.destination = target.position;
    }
}

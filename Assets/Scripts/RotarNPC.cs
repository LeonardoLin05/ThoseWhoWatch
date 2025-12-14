using UnityEngine;

public class RotarNPC : MonoBehaviour
{
    [SerializeField] Transform player;

    void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicion = new(player.position.x - transform.position.x, 0, player.position.z - transform.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(posicion), 150f * Time.deltaTime);
    }
}

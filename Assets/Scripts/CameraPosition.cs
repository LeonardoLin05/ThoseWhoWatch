using UnityEngine;

public class CameraPosition : MonoBehaviour
{

    public Transform player;

    void Update()
    {
        transform.position = player.position;
    }
}

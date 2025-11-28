using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

public AudioClip audioFootsteps;
private Vector3 prevPosition;
float minDistance = 1.2f;

void Start()
{
  prevPosition = transform.position;
}

void Update()
{
    if (Vector3.Distance(transform.position, prevPosition) > minDistance)
    {
        AudioSource.PlayClipAtPoint(audioFootsteps, transform.position, 0.1f);
        prevPosition = transform.position;
    }
}
}

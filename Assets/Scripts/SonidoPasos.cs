using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

public AudioClip audioFootsteps;
private Vector3 prevPosition;
float minDistance = 2.4f;

void Start()
{
  prevPosition = transform.position;
}

void Update()
{
    if (Vector3.Distance(transform.position, prevPosition) > minDistance)
    {
        AudioSource.PlayClipAtPoint(audioFootsteps, transform.position);
        prevPosition = transform.position;
    }
}
}

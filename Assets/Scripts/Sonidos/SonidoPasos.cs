using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

  public AudioClip audioFootsteps;
  private Vector3 prevPosition;
  float minDistance = 1.2f;
  bool stepped = false;

  void Start()
  {
    prevPosition = transform.position;
  }

  void Update()
  {
    if (!stepped && HeadbobSystem.Instance.sumY < -0.005f)
    {
        AudioSource.PlayClipAtPoint(audioFootsteps, transform.position, 0.1f);
        stepped = true;
    }
    else if(stepped && HeadbobSystem.Instance.sumY >= 0f)
    {
        stepped = false;
    }
  }
}

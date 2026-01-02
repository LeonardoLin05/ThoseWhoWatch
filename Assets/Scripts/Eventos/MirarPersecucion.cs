using System.Collections;
using UnityEngine;

public class MirarPersecucion : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void StartEvent()
    {
        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        RetryMenu.ZONA_PERSECUCION = true;
        audioSource.Play();
        TalkZoomMoveCamera.Instance.SetCabeza(transform);
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f, true);
        InteractNPCs.ActivarInstances(false);
        yield return new WaitForSecondsRealtime(2f);
        TalkZoomMoveCamera.Instance.StopZoomMovement();
        InteractNPCs.ActivarInstances(true);
    }
}

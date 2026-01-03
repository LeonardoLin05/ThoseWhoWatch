using System.Collections;
using UnityEngine;

public class MirarMuerte : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Awake()
    {
        
    }

    public void StartEvent()
    {
        StartCoroutine(EventCoroutine());
    }

    private IEnumerator EventCoroutine()
    {
        RetryMenu.ZONA_PERSECUCION = true;
        TalkZoomMoveCamera.Instance.SetCabeza(target);
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f, true);
        InteractNPCs.ActivarInstances(false);
        yield return new WaitForSecondsRealtime(2f);
        TalkZoomMoveCamera.Instance.StopZoomMovement();
        InteractNPCs.ActivarInstances(true);
    }
}

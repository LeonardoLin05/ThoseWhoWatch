using UnityEngine;

public class TalkZoomMoveCamera : MonoBehaviour
{
    public static TalkZoomMoveCamera Instance { get; private set; }

    private Camera camara;
    private Quaternion cabeza;

    private float zoomFOV = 50;
    private float noZoomFOV = 100;

    private bool start = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Instance.enabled = false;

        camara = transform.GetChild(0).gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            camara.fieldOfView = Mathf.Lerp(camara.fieldOfView, zoomFOV, 2 * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, cabeza, 50 * Time.deltaTime);
        }
        /*
        else
        {
            camara.fieldOfView = Mathf.Lerp(camara.fieldOfView, noZoomFOV, 2 * Time.deltaTime);

            if (camara.fieldOfView == noZoomFOV)
            {
                Instance.enabled = false;
            }
        }
        */
    }

    public void setCabeza(Transform cabeza)
    {
        this.cabeza = Quaternion.LookRotation(cabeza.position - transform.position);
    }

    public void StartZoomMovement(bool start)
    {
        if (start)
        {
            this.start = start;
            Instance.enabled = true;
        }
        else
        {
            this.start = start;
            Instance.enabled = false;
            camara.fieldOfView = noZoomFOV;
        }
    }
}

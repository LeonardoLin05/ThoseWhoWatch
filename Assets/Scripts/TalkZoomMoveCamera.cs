using UnityEngine;

public class TalkZoomMoveCamera : MonoBehaviour
{
    public static TalkZoomMoveCamera Instance { get; private set; }

    private Camera camara;
    private Quaternion cabeza;

    private float zoomFOV = 50;
    private float noZoomFOV = 100;

    private float rotationSpeed;

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
        camara.fieldOfView = Mathf.Lerp(camara.fieldOfView, zoomFOV, 2 * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, cabeza, rotationSpeed * Time.deltaTime);
    }

    public void setCabeza(Transform cabeza)
    {
        this.cabeza = Quaternion.LookRotation(cabeza.position - transform.position);
    }

    /// <summary>
    /// Empieza el movimiento de zoom y rotacion
    /// </summary>
    public void StartZoomMovement(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
        Instance.enabled = true;
    }

    /// <summary>
    /// Restablece todo como estaba antes de empezar el zoom y rotacion
    /// </summary>
    public void StopZoomMovement()
    {
        camara.fieldOfView = noZoomFOV;
        Instance.enabled = false;
    }
}

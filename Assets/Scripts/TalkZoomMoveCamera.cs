using UnityEngine;

public class TalkZoomMoveCamera : MonoBehaviour
{
    public static TalkZoomMoveCamera Instance { get; private set; }

    private Camera camara;
    private Quaternion cabeza;

    private readonly float ZOOMFOV = 50;
    private readonly float NOZOOMFOV= 100;

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
        enabled = false;

        camara = transform.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        camara.fieldOfView = Mathf.Lerp(camara.fieldOfView, ZOOMFOV, 2 * Time.deltaTime);
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
        enabled = true;
    }

    /// <summary>
    /// Restablece todo como estaba antes de empezar el zoom y rotacion
    /// </summary>
    public void StopZoomMovement()
    {
        camara.fieldOfView = NOZOOMFOV;
        enabled = false;
    }
}

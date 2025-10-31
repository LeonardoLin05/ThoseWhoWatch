using System;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public static Zoom Instance { get; private set; }

    private Camera camara;

    [SerializeField] private float zoomFOV;
    private float noZoomFOV = 100;

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
    }

    void Start()
    {
        camara = gameObject.GetComponent<Camera>();
    }

    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            camara.fieldOfView = Mathf.Lerp(camara.fieldOfView, zoomFOV, 2 * Time.deltaTime);
        }
        else
        {
            camara.fieldOfView = Mathf.Lerp(camara.fieldOfView, noZoomFOV, 2 * Time.deltaTime);
        }
    }
}

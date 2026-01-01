using System;
using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }

    private Transform player;

    public static float mouseSensitivityX = 500f;
    public static float mouseSensitivityY = 500f;

    public bool lockY = false;
    public bool lockX = false;
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;

    private float inputX;
    private float inputY;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        GirarCamara();
    }

    void LateUpdate()
    {
        GirarPersonaje();   
    }

    private void GirarCamara()
    {
        if(!lockX) inputX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivityX;
        if(!lockY) inputY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivityY;

        yRotation += inputX;
        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void GirarPersonaje()
    {
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    public void ChangeCameraRotation(float x, float y)
    {
        xRotation = x;
        yRotation = y;
        transform.rotation = Quaternion.Euler(x, y, 0);
    }

    /// <summary>
    /// Hace que el objeto rote junto con la cámara
    /// </summary>
    /// <param name="objeto">El objeto que se quiere girar</param>
    public void GirarObjeto(Transform objeto)
    {
        objeto.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}

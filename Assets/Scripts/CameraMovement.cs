using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }

    private Transform player;

    [SerializeField, Range(0f, 1000f)] private float mouseSensitivity = 500f;
    public float xRotation;
    public float yRotation;

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
        float inputX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float inputY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        yRotation += inputX;
        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void GirarPersonaje()
    {
        player.rotation = Quaternion.Euler(0, yRotation, 0);
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

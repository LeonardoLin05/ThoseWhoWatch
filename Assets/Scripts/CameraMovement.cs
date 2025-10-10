using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    private float mouseSensitivity = 500f;
    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
	if(!VariablesGlobales.PARAR_CAMARA)
	{
        	GirarCamara();
	}
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
}

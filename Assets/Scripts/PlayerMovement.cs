using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    private float walkSpeed = 2f;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private float speed;

    private bool crouching = false;

    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Aplicamos la gravedad
        velocity.y += gravity;

        // Reiniciamos la velocidad aplicada por la gravedad si el personaje
        // esta tocando el suelo
        if (characterController.isGrounded && characterController.velocity.y < 0)
        {
            velocity.y = 0f;
        }

        speed = Crouch();

        if (!crouching)
        {
            speed = Run();
        }

        Vector3 finalMove = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")) * speed + velocity;

        characterController.Move(finalMove * Time.deltaTime);
    }

    /// <summary>
    /// Devuelve la velocidad adecuada dependiendo de si
    /// el jugador esta corriendo o no (presionando la tecla left shift o no)
    /// </summary>
    /// <returns></returns>
    private float Run()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Corriendo");
            HeadbobSystem.ChangeData(0.008f, 15f);
            return walkSpeed * 1.3f;
        }
        else
        {
            Debug.Log("No corriendo");
            HeadbobSystem.ChangeData(0.004f, 10f);
            return walkSpeed;
        }
    }

    // TODO: mejorar sistema de agachar
    private float Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            crouching = true;
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            return walkSpeed * 0.6f;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            crouching = false;
            return walkSpeed;
        }
    }
}

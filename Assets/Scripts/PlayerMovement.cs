using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public Transform orientation;

    private float walkSpeed = 2f;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private float speed;

    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!VariablesGlobales.PARAR_MOVIMIENTO)
        {
            // Aplicamos la gravedad
            velocity.y += gravity;

            // Reiniciamos la velocidad aplicada por la gravedad si el personaje
            // esta tocando el suelo
            if (characterController.isGrounded && characterController.velocity.y < 0)
            {
                velocity.y = 0f;
            }

            speed = Run();

            Vector3 finalMove = (orientation.right * Input.GetAxisRaw("Horizontal") + orientation.forward * Input.GetAxisRaw("Vertical")) * speed + velocity;

            characterController.Move(finalMove * Time.deltaTime);
        }
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
            HeadbobSystem.ChangeData(0.008f, 15f);
            return walkSpeed * 1.3f;
        }
        else
        {
            HeadbobSystem.ChangeData(0.004f, 10f);
            return walkSpeed;
        }
    }
}

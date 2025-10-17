using UnityEngine;

public class Perseguir : MonoBehaviour
{

    public Transform player; 
    float moveSpeed = 1.5f;
    private float rotationSpeed = 6f;
    private Transform enemigoTrans;
    private CharacterController enemigoControl;
    private Vector3 velocity;
    private float gravity = -9.81f;


    void Awake()
    {
       enemigoTrans = GetComponent<Transform>();
       enemigoControl = GetComponent<CharacterController>();
    }

    void Update ()
    {
        
        //Calcular distancia
        float distancia;
        distancia = Vector3.Distance(player.transform.position, transform.position);

        if(distancia<15 && distancia>2)
        {
            // Aplicamos la gravedad
            velocity.y += gravity;

            // Reiniciamos la velocidad aplicada por la gravedad si el personaje
            // esta tocando el suelo
            if (enemigoControl.isGrounded && enemigoControl.velocity.y < 0)
            {
                velocity.y = 0f;
            }

            //Voltear
            enemigoTrans.rotation = Quaternion.Slerp(enemigoTrans.rotation,
            Quaternion.LookRotation(player.position - enemigoTrans.position), rotationSpeed*Time.deltaTime);

            if (VariablesGlobales.DENTRO_ARMARIO)
            {
                if (distancia < 8)
                {
                    enemigoControl.Move((-enemigoTrans.forward * moveSpeed/2 + velocity) * Time.deltaTime);
                }
            }
            else
            {
                enemigoControl.Move((enemigoTrans.forward * moveSpeed + velocity) * Time.deltaTime);
            }
            
            
            
            //Lineas de debug que aparecen en la ventana Scene
            //Debug.DrawLine (player.transform.position, transform.position, Color.red,  Time.deltaTime, false);
        }
    }
}
/*
public class Perseguir : MonoBehaviour
{
    public Transform player;
    private Transform enemigo; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemigo = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.position.x != enemigo.position.x + 3 || player.position.x != enemigo.position.x - 3) &&
            (player.position.z != enemigo.position.z + 3 || player.position.z != enemigo.position.x - 3))
        {
            if (player.position.x < enemigo.position.x)
            {
                enemigo.position = new Vector3(enemigo.position.x - 0.01f, enemigo.position.y, enemigo.position.z) * Time.deltaTime;
            }
            else
            {
                enemigo.position = new Vector3(enemigo.position.x + 0.01f, enemigo.position.y, enemigo.position.z) * Time.deltaTime;
            }

            if (player.position.z < enemigo.position.z)
            {
                enemigo.position = new Vector3(enemigo.position.x, enemigo.position.y,enemigo.position.z - 0.01f) * Time.deltaTime;
            }
            else
            {
                enemigo.position = new Vector3(enemigo.position.x, enemigo.position.y,enemigo.position.z + 0.01f) * Time.deltaTime;
            }
        }   
    }
}
*/

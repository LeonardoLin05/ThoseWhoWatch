using System.Collections;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private GameObject[] WP;
    [SerializeField] private GameObject armario;
    [SerializeField] private AudioSource heartbeat;

    private Animator animacion;

    private WaitForSecondsRealtime _waitForSecondsRealtime3 = new(3f);
    private int currentWP = 0;

    void Awake()
    {
        animacion = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, WP[currentWP].transform.position) < 0.1)
        {
            // Para tema de animaciones (si quieres que el NPC se pare a mirar alrededor antes 
            // continuar su ruta en los puntos que quieras, puedes poner más con un OR)
            if (currentWP == 0 || currentWP == 3)
            {
                StartCoroutine(Wait());
            }
            // Vamos al siguiente waypoint
            currentWP++;
        }
        // ¿Hemos llegado al final?
        if(currentWP >= WP.Length)
        {
            heartbeat.Stop();
            armario.layer = 6;
            Thoughts.Instance.StartThoughts("Salí corriendo de casa");
            animacion.SetTrigger("lookAround");
            enabled = false;
        }
        else
        {
            Quaternion lookAtWP = Quaternion.LookRotation(WP[currentWP].transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, 5f * Time.deltaTime);

            transform.Translate(0, 0, 0.7f * Time.deltaTime); 
        }
    }

    private IEnumerator Wait()
    {
        animacion.SetTrigger("lookAround");
        enabled = false;
        yield return _waitForSecondsRealtime3;
        enabled = true;
        animacion.SetTrigger("walking");
    }
}

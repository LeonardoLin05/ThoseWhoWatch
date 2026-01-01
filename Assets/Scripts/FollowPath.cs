using System.Collections;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private GameObject[] WP;

    private WaitForSecondsRealtime _waitForSecondsRealtime3 = new(3f);
    private int currentWP = 0;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, WP[currentWP].transform.position) < 0.1)
        {
            // Para tema de animaciones (si quieres que el NPC se pare a mirar alrededor antes 
            // continuar su ruta en los puntos que quieras, puedes poner más con un OR)
            if(currentWP == 0)
            {
                StartCoroutine(Wait());
            }
            currentWP++;
        }

        if(currentWP >= WP.Length)
        {
            enabled = false;
        }
        else
        {
           Quaternion lookAtWP = Quaternion.LookRotation(WP[currentWP].transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, 5f * Time.deltaTime);

            transform.Translate(0, 0, 0.5f * Time.deltaTime); 
        }
    }

    private IEnumerator Wait()
    {
        enabled = false;
        yield return _waitForSecondsRealtime3;
        enabled = true;
    }
}

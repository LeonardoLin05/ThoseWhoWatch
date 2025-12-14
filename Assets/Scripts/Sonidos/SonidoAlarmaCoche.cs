using System.Collections;
using UnityEngine;

public class SonidoAlarmaCoche : MonoBehaviour
{
    private Light luz1;
    private Light luz2;

    private AudioSource alarma;

    private readonly WaitForSecondsRealtime parpadeo = new(0.5f);

    void Awake()
    {
        luz1 = gameObject.GetComponentsInChildren<Light>()[0];
        luz2 = gameObject.GetComponentsInChildren<Light>()[1];
        alarma = gameObject.GetComponent<AudioSource>();
    }

    public void StartAlarma()
    {
        alarma.Play();
        StartCoroutine(Parpadeo());
    }

    private IEnumerator Parpadeo()
    {
        
        for(int i = 0; i < 9; i++)
        {
            luz1.intensity = 3;
            luz2.intensity = 3;
            yield return parpadeo;
            luz1.intensity = 0;
            luz2.intensity = 0;
            yield return parpadeo;
        }
        yield return null;
    }
}

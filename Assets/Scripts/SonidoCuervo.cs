using UnityEngine;
using System.Collections;

public class SonidoCuervo : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float inicio = 7f;   
    [SerializeField] private float fin = 12f;     
    [SerializeField] private float duracionFade = 1f; 

    void OnEnable()
    {
        StartCoroutine(ReproducirConFade());
    }

    private IEnumerator ReproducirConFade()
    {
        // Configura el inicio
        audioSource.time = inicio;
        audioSource.volume = 1f;
        audioSource.Play();

        yield return new WaitForSeconds(fin - inicio - duracionFade);

        float startVolume = audioSource.volume;
        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duracionFade);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume; 
    }
}

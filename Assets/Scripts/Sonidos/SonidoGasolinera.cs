using UnityEngine;

public class SonidoGasolinera : MonoBehaviour
{
    [SerializeField] private AudioSource audioGasolinera;
    [SerializeField] private Transform player;
    [SerializeField] private float distanciaMin;

    void Update()
    {
        float distancia = Vector3.Distance(player.position, transform.position);

        // Reducimos el volumen gradualmente hasta 0
        if(distancia < distanciaMin)
        {
            audioGasolinera.volume = Mathf.Lerp(audioGasolinera.volume, 0f, Time.deltaTime);
        }
        // Recuperamos volumen predeterminado
        else
        {
            audioGasolinera.volume = Mathf.Lerp(audioGasolinera.volume, 0.05f, Time.deltaTime);
        }
    }
}

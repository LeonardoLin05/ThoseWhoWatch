using UnityEngine;

public class SonidoGasolinera : MonoBehaviour
{
    [SerializeField] private AudioSource audioGasolinera;
    [SerializeField] private Transform player;
    [SerializeField] private float distanciaMin;

    void Update()
    {
        float distancia = Vector3.Distance(player.position, transform.position);
        // Hacemos que el sonido sea espacial para que se vaya reduciendo segun entramos en la gasolinera
        if(distancia < distanciaMin) audioGasolinera.spatialBlend = 1;
        // Hacemos que el sonido deje de ser espacial para que el audio sea mono
        else audioGasolinera.spatialBlend = 0;
    }
}

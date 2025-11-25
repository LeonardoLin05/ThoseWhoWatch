using UnityEngine;

public class SonidoMusicaBebe : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;   
    [SerializeField] private Transform jugador;         
    [SerializeField] private float distanciaMax = 8f;  
    [SerializeField] private float volumenMax = 1f;     

    void Start()
    {
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        float distancia = Vector3.Distance(jugador.position, transform.position);
        float factor = Mathf.Clamp01(1f - (distancia / distanciaMax));
        audioSource.volume = factor * volumenMax;
    }
}

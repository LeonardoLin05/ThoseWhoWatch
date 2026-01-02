using UnityEngine;

public class PlaySonidoAnimacion : MonoBehaviour
{
    [SerializeField] private AudioSource sonido;

    void OnEnable()
    {
        PlaySound();
    }

    public void PlaySound()
    {
        sonido.Play();
    }
}

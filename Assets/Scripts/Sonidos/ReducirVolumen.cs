using UnityEngine;

public class ReducirVolumen : MonoBehaviour
{

    [SerializeField] private AudioSource sonido;
    [SerializeField] private AudioSource screamer;

    void Update()
    {
        if(sonido.isPlaying) sonido.volume = Mathf.Lerp(sonido.volume, 0, 1f * Time.deltaTime);
        if(screamer.isPlaying) screamer.volume = Mathf.Lerp(screamer.volume, 0, 4f * Time.deltaTime);
    }
}

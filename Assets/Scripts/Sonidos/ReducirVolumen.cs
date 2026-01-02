using UnityEngine;

public class ReducirVolumen : MonoBehaviour
{

    [SerializeField] private AudioSource sonido;

    void Update()
    {
        sonido.volume = Mathf.Lerp(sonido.volume, 0, 1f * Time.deltaTime);
    }
}

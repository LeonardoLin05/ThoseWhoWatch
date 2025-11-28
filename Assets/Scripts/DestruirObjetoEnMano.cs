using UnityEngine;

public class DestruirObjetoEnMano : MonoBehaviour
{
    [SerializeField] private AudioClip clip; 

    public void DestruirObjeto()
    {
        GameObject objeto = GameObject.FindGameObjectWithTag("ObjetoEnMano");
        Destroy(objeto);
        AudioSource.PlayClipAtPoint(clip, transform.position);
        InteractPickUp.objetoEnMano = false;
    }
}

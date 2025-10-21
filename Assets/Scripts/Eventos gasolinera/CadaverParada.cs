using UnityEngine;

public class CadaverParada : MonoBehaviour
{
    private GameObject killer;
    private Animator animacion;

    void Awake()
    {
        killer = GameObject.Find("Killer");
        animacion = killer.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animacion.SetTrigger("Andar");
            Destroy(killer, 20f);
            Destroy(gameObject, 0f);
        }
    }
}

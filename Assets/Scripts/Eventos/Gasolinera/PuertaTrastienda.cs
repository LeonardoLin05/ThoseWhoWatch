using UnityEngine;

public class PuertaTrastienda : MonoBehaviour
{
    [SerializeField] InteractDoor puertaTrasera;
    private InteractDoor interactDoor;
    private Animator animacion;

    void Awake()
    {
        interactDoor = gameObject.GetComponent<InteractDoor>();
        animacion = gameObject.GetComponent<Animator>();
    }

    public void PuertaMedioAbierta()
    {
        puertaTrasera.bloqueada = false;
        interactDoor.bloqueada = false;
        animacion.SetTrigger("Start");
    }
}

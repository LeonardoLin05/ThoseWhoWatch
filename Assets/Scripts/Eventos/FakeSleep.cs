using UnityEngine;

public class FakeSleep : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource sonidoPuerta;
    [SerializeField] private GameObject eventoPararSonido;
    [SerializeField] private GameObject killer;

    public void Interact()
    {
        gameObject.layer = 0;
        eventoPararSonido.SetActive(true);
        sonidoPuerta.Play();
        killer.SetActive(true);
        Thoughts.Instance.StartThoughts("");
    }

    public string MensajeInteraccion()
    {
        return "[E] para dormir";
    }
}

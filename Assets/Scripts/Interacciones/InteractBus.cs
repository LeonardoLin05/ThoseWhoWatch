using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InteractBus : MonoBehaviour, IInteractable
{
    public GameObject Autobus;
    private Animator busAnimation;
    private bool active = false;
    private bool ready = false;
    private Animator fade;
    void Start()
    {
        busAnimation = Autobus.GetComponent<Animator>();
        fade = GameObject.FindGameObjectsWithTag("Fade")[0].GetComponent<Animator>();
    }

    public IEnumerator interact()
    {
        if (!active)
        {
            active = true;

            if (!Autobus.activeSelf)
            {
                Autobus.SetActive(true);
            }

            busAnimation.SetTrigger("Move");

            yield return new WaitForSeconds(6f);
            ready = true;

        }
        else if (active && ready)
        {
            /*VariablesGlobales.PARAR_CAMARA = true;
            VariablesGlobales.PARAR_MOVIMIENTO = true;*/

            fade.SetTrigger("Fade");
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("Gasolinera");
        }
        VariablesGlobales.INTERACTUAR = true;
    }

    public string MensajeInteraccion()
    {
        if (!active && !ready)
        {
            return "[E] para llamar al bus";
        }
        else if (active && !ready)
        {
            return "El bus está llegando";
        }
        else
        {
            return "[E] para montarte";
        }
    }

    public bool ocupado()
    {
        return false;
    }
}

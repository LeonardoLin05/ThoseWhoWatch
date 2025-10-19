using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InteractTeleport : MonoBehaviour, IInteractable
{
    public Transform teleportDestino;
	public Transform player;
	private bool ready = false;
	private Animator fade;

	void Start()
	{
		fade = GameObject.FindGameObjectsWithTag("Fade")[0].GetComponent<Animator>();
	}
	
	public void ActivarTeleport()
    {
		ready = true;
    }

    public IEnumerator interact()
	{
        if (!ready)
        {
			yield break;
        }

		fade.SetTrigger("Fade");
		yield return new WaitForSeconds(1.5f);

		player.position = teleportDestino.position;
		Physics.SyncTransforms();
		VariablesGlobales.INTERACTUAR = true;
		Debug.Log("Jugador teletransportado");
	}

	public string MensajeInteraccion(){
        if (ready)
        {
          return "[E] para Viajar";  
        }
        else
        {
			return "Waiting for Bus";
        }
	}
}
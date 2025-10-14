using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InteractTeleport : MonoBehaviour, IInteractable
{
    public Transform teleportDestino;
	public Transform player;

	private Animator fade;

	void Start()
    {
		fade = GameObject.FindGameObjectsWithTag("Fade")[0].GetComponent<Animator>();
    }

    public IEnumerator interact()
	{
		fade.SetTrigger("Fade");
		yield return new WaitForSeconds(1.5f);

		player.position = teleportDestino.position;
		Physics.SyncTransforms();
		VariablesGlobales.INTERACTUAR = true;
		Debug.Log("Jugador teletransportado");
	}

	public string MensajeInteraccion(){

		return "Press E to teleport";
	}
}
using UnityEngine;

public class InteractTeleport : MonoBehaviour, IInteractable
{
    public Transform teleportDestino;

	public Transform player;	

    public void interact()
	{

		Debug.Log("Hola");

		if (player != null && teleportDestino != null)
		{

			player.position = teleportDestino.position;
			Physics.SyncTransforms();
			Debug.Log("Jugador teletransportado");
		}
	}
}
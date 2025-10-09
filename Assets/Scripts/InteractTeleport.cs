using UnityEngine;

public class InteractTeleport : MonoBehaviour, IInteractable
{
    	public Transform teleportDestino;

    	public void interact()
    	{
        	
		Debug.Log("Hola"); 
            	GameObject player = GameObject.FindGameObjectWithTag("Player");
			
            	if (player != null && teleportDestino != null)
            	{
                	CharacterController cc = player.GetComponent<CharacterController>();
                	if (cc != null) 
			{
				cc.enabled = false;
				player.transform.position = teleportDestino.position;
				cc.enabled = true;
				Debug.Log("Jugador teletransportado");
			}
            	}
        	
    	}
}
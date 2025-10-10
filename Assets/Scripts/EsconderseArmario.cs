using UnityEngine;

public class EsconderseArmario : MonoBehaviour, IInteractable
{
    public GameObject teleportDestino;
	public Transform player;
	public Transform camara; 
	public Material materialTransparente;
	private Material materialArmario; 
	private bool estoyFuera = true; 
	

    public void interact()
	{
		if (player != null && teleportDestino != null && materialTransparente != null)
		{
			if (estoyFuera)
			{
				player.position = teleportDestino.GetComponent<Transform>().position;
				Physics.SyncTransforms();
				//camara.eulerAngles = new Vector3(camara.rotation.x,0,camara.rotation.z);
				//player.eulerAngles = new Vector3(player.rotation.x,0,player.rotation.z);
				camara.rotation = Quaternion.Euler(0,Time.deltaTime * 10,0);  
				player.rotation = Quaternion.Euler(0,Time.deltaTime * 10,0);
				materialArmario = GetComponent<MeshRenderer>().material; 
				GetComponent<MeshRenderer>().material = materialTransparente;
				estoyFuera = false; 
			}
			else
			{
				Vector3 newPos= teleportDestino.GetComponent<Transform>().position;
				newPos.z = newPos.z + 1;
				player.position = newPos;
				Physics.SyncTransforms();
				GetComponent<MeshRenderer>().material = materialArmario;
				estoyFuera = true; 	
			} 			
		}
	}
}

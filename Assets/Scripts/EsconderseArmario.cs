using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EsconderseArmario : MonoBehaviour, IInteractable
{
	public Transform teleportEntrada;
	public Transform teleportSalida;
	public Transform player;
	public HeadbobSystem headbobSystem;
	public Transform camara;
	public Material materialTransparente;
	private Material materialArmario;

	private Animator fade;

	private bool estoyFuera = true;

    void Start()
	{
		fade = GameObject.FindGameObjectsWithTag("Fade")[0].GetComponent<Animator>();
        materialArmario = GetComponent<MeshRenderer>().material;
    }

    public IEnumerator interact()
	{
		if (estoyFuera)
		{
			VariablesGlobales.PARAR_CAMARA = true;
			VariablesGlobales.PARAR_MOVIMIENTO = true;
			headbobSystem.enabled = false;
			estoyFuera = false;

			fade.SetTrigger("Fade");
			yield return new WaitForSeconds(1.5f);

			camara.rotation = Quaternion.Euler(0, 0, 0);
			player.position = teleportEntrada.position;

			GetComponent<MeshRenderer>().material = materialTransparente;
		}
		else
		{
			fade.SetTrigger("Fade");
			yield return new WaitForSeconds(1.5f);

			VariablesGlobales.PARAR_CAMARA = false;
			VariablesGlobales.PARAR_MOVIMIENTO = false;
			headbobSystem.enabled = true;
			estoyFuera = true;

			player.position = teleportSalida.position;
			GetComponent<MeshRenderer>().material = materialArmario;
		}
		Physics.SyncTransforms();
		VariablesGlobales.INTERACTUAR = true;
	}

	public string MensajeInteraccion(){

		if(estoyFuera){
			return "Press E to hide";
		}
		else{
			return "Press E to exit";
		}
	}
}

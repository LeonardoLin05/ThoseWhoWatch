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

	//private bool estoyFuera = true;

    void Start()
	{
		fade = GameObject.FindGameObjectsWithTag("Fade")[0].GetComponent<Animator>();
        materialArmario = GetComponent<MeshRenderer>().material;
    }

    public IEnumerator interact()
	{
		if (!VariablesGlobales.DENTRO_ARMARIO)
		{
			VariablesGlobales.PARAR_CAMARA = true;
			VariablesGlobales.PARAR_MOVIMIENTO = true;
			headbobSystem.enabled = false;
			VariablesGlobales.DENTRO_ARMARIO = true;

			CameraMovement.xRotation = 0;
			CameraMovement.yRotation = 0;

			fade.SetTrigger("Fade");
			yield return new WaitForSeconds(1.5f);
			VariablesGlobales.PARAR_CAMARA = false;

			//camara.rotation = Quaternion.Euler(0, 0, 0);
			player.position = teleportEntrada.position;

			GetComponent<MeshRenderer>().material = materialTransparente;
		}
		else
		{
			fade.SetTrigger("Fade");
			yield return new WaitForSeconds(1.5f);

			//VariablesGlobales.PARAR_CAMARA = false;
			VariablesGlobales.PARAR_MOVIMIENTO = false;
			headbobSystem.enabled = true;
			VariablesGlobales.DENTRO_ARMARIO = false;

			CameraMovement.xRotation = 0;
			CameraMovement.yRotation = 0;

			player.position = teleportSalida.position;
			GetComponent<MeshRenderer>().material = materialArmario;
		}
		Physics.SyncTransforms();
		VariablesGlobales.INTERACTUAR = true;
	}

	public string MensajeInteraccion(){

		if(!VariablesGlobales.DENTRO_ARMARIO)
		{
			return "[E] para Esconderse";
		}
		else
		{
			return "[E] para Salir";
		}
	}
}

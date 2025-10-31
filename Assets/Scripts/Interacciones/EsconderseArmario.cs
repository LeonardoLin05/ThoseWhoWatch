using System.Collections;
using UnityEngine;

public class EsconderseArmario : MonoBehaviour, IInteractable
{
	[SerializeField] private Transform teleportEntrada;
	[SerializeField] private Transform teleportSalida;

	private Transform player;

	[SerializeField] private Material materialTransparente;
	private Material materialArmario;

	private Animator fade;

    void Start()
	{
		fade = GameObject.FindGameObjectsWithTag("Fade")[0].GetComponent<Animator>();
		player = GameObject.Find("Player").GetComponent<Transform>();
        materialArmario = GetComponent<MeshRenderer>().material;
    }

    public IEnumerator interact()
	{
		if (!VariablesGlobales.DENTRO_ARMARIO)
		{
			StartCoroutine(EntrarArmario());
		}
		else
		{
			StartCoroutine(SalirArmario());
		}
		Physics.SyncTransforms();
		VariablesGlobales.INTERACTUAR = true;
		yield break;
	}

	private IEnumerator EntrarArmario()
    {
        PlayerMovement.Instance.enabled = false;
		HeadbobSystem.Instance.enabled = false;
		VariablesGlobales.DENTRO_ARMARIO = true;

		fade.SetTrigger("Fade");
		yield return new WaitForSeconds(1.5f);

		// Rotamos la cámara para que mire donde queramos
		CameraMovement.Instance.xRotation = teleportEntrada.eulerAngles.x;
		CameraMovement.Instance.yRotation = teleportEntrada.eulerAngles.y;

		player.position = teleportEntrada.position;

		GetComponent<MeshRenderer>().material = materialTransparente;
    }

	private IEnumerator SalirArmario()
    {
        fade.SetTrigger("Fade");
			yield return new WaitForSeconds(1.5f);

			PlayerMovement.Instance.enabled = true;
			HeadbobSystem.Instance.enabled = true;
			VariablesGlobales.DENTRO_ARMARIO = false;

			// Rotamos la cámara para que mire donde queramos
			CameraMovement.Instance.xRotation = teleportSalida.eulerAngles.x;
			CameraMovement.Instance.yRotation = teleportSalida.eulerAngles.y;

			player.position = teleportSalida.position;
			GetComponent<MeshRenderer>().material = materialArmario;
    }

	public string MensajeInteraccion()
	{

		if (!VariablesGlobales.DENTRO_ARMARIO)
		{
			return "[E] para Esconderse";
		}
		else
		{
			return "[E] para Salir";
		}
	}
}

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

	private WaitForSeconds _waitForSeconds1_5 = new(1.5f);

	// Variable global para saber si estamos dentro o fuera del armario;
	// NOTA: se puede llamar desde otras clases pero no cambiar su valor desde ellas
	public static bool DENTRO_ARMARIO { get; private set; } = false;

	void Start()
	{
		fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        materialArmario = GetComponent<MeshRenderer>().material;
    }

    public void interact()
	{
		if (!DENTRO_ARMARIO)
		{
			StartCoroutine(Esconderse());
		}
		else
		{
			StartCoroutine(Salir());
		}
	}

	private IEnumerator Esconderse()
    {
        	PlayerMovement.Instance.enabled = false;
			HeadbobSystem.Instance.enabled = false;
			Interaction.Instance.enabled = false;

			DENTRO_ARMARIO = true;

			fade.SetTrigger("Fade");
			yield return _waitForSeconds1_5;

			Interaction.Instance.enabled = true;

			// Rotamos la cámara para que mire donde queramos
			CameraMovement.Instance.xRotation = teleportEntrada.eulerAngles.x;
			CameraMovement.Instance.yRotation = teleportEntrada.eulerAngles.y;

			player.position = teleportEntrada.position;
			Physics.SyncTransforms();

			GetComponent<MeshRenderer>().material = materialTransparente;
    }

	private IEnumerator Salir()
    {
        Interaction.Instance.enabled = false;

		fade.SetTrigger("Fade");
		yield return _waitForSeconds1_5;

		PlayerMovement.Instance.enabled = true;
		HeadbobSystem.Instance.enabled = true;
		Interaction.Instance.enabled = true;

		DENTRO_ARMARIO = false;

		// Rotamos la cámara para que mire donde queramos
		CameraMovement.Instance.xRotation = teleportSalida.eulerAngles.x;
		CameraMovement.Instance.yRotation = teleportSalida.eulerAngles.y;

		player.position = teleportSalida.position;
		Physics.SyncTransforms();
		GetComponent<MeshRenderer>().material = materialArmario;
    }

	public string MensajeInteraccion()
	{
		if (!DENTRO_ARMARIO)
		{
			return "[E] para Esconderse";
		}
		else
		{
			return "[E] para Salir";
		}
	}
}

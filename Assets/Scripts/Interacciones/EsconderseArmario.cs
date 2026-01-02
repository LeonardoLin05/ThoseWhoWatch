using System.Collections;
using UnityEngine;

public class EsconderseArmario : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform teleportEntrada;
	[SerializeField] private Transform teleportSalida;

	[SerializeField] private string pensamiento;
	[SerializeField] private MeshRenderer puertaDerecha;

	private Transform player;

	[SerializeField] private Material materialTransparente;
	private Material materialArmario;

	private GameObject cursor;

	private Animator fade;

	private WaitForSeconds _waitForSeconds1_5 = new(1.5f);

	// Variable global para saber si estamos dentro o fuera del armario;
	// NOTA: se puede llamar desde otras clases pero no cambiar su valor desde ellas
	public static bool DENTRO_ARMARIO { get; private set; }

	public static bool DESACTIVAR_ARMARIO;

	void Start()
	{
		DESACTIVAR_ARMARIO = false;
		DENTRO_ARMARIO = false;
		cursor = GameObject.FindGameObjectWithTag("Puntero");
		fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        materialArmario = gameObject.GetComponent<MeshRenderer>().material;
    }

    public void Interact()
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
			if(DESACTIVAR_ARMARIO) gameObject.layer = 0;

        	PlayerMovement.Instance.enabled = false;
			HeadbobSystem.Instance.enabled = false;
			Interaction.Instance.enabled = false;
			CameraMovement.Instance.enabled = false;

			DENTRO_ARMARIO = true;

			fade.SetTrigger("Fade");
			yield return _waitForSeconds1_5;

			Interaction.Instance.enabled = true;

			cursor.SetActive(false);

			// Rotamos la cámara para que mire donde queramos
			CameraMovement.Instance.ChangeCameraRotation(teleportEntrada.eulerAngles.x, teleportEntrada.eulerAngles.y);

			gameObject.GetComponent<MeshRenderer>().material = materialTransparente;
			puertaDerecha.material = materialTransparente;
			player.position = teleportEntrada.position;
			Physics.SyncTransforms();
			//yield return _waitForSeconds1_5;
			if(pensamiento != null)
			{
				Thoughts.Instance.StartThoughts(pensamiento);
				pensamiento = null;
			}
			//CameraMovement.Instance.lockX = false;
    }

	private IEnumerator Salir()
    {
        Interaction.Instance.enabled = false;

		fade.SetTrigger("Fade");
		yield return _waitForSeconds1_5;

		if(DESACTIVAR_ARMARIO) {
			PlayerMovement.Instance.activarCorrer = true;
			gameObject.layer = 0;
			Thoughts.Instance.StartInstruction("[LSHIFT] para Correr");
		}

		PlayerMovement.Instance.enabled = true;
		HeadbobSystem.Instance.enabled = true;
		Interaction.Instance.enabled = true;

		cursor.SetActive(true);

		DENTRO_ARMARIO = false;

		// Rotamos la cámara para que mire donde queramos
		CameraMovement.Instance.ChangeCameraRotation(teleportSalida.eulerAngles.x, teleportSalida.eulerAngles.y);

		CameraMovement.Instance.lockY = false;
		CameraMovement.Instance.enabled = true;

		player.position = teleportSalida.position;
		Physics.SyncTransforms();
		gameObject.GetComponent<MeshRenderer>().material = materialArmario;
		puertaDerecha.material = materialArmario;
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

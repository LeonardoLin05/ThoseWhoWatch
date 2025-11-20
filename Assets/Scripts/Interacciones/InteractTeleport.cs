using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InteractTeleport : MonoBehaviour, IInteractable
{
	private Animator fade;

	void OnEnable()
	{
		gameObject.layer = 6;
	}

    void OnDisable()
	{
		gameObject.layer = 0;
    }

    void Start()
	{
		fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
	}

    public void interact()
	{
		StartCoroutine(Teleport());
	}

	private IEnumerator Teleport()
    {
        Interaction.Instance.enabled = false;
		fade.SetTrigger("Fade");
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene("Gasolinera"); 
    }

	public string MensajeInteraccion()
	{
		return "[E] para Viajar";
	}
}
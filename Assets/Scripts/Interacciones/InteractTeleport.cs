using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class InteractTeleport : MonoBehaviour, IInteractable
{
	private Animator fade;

	private string mensajeInteraccion;

	void OnEnable()
	{
		mensajeInteraccion = "[E] para Viajar";
	}

    void OnDisable()
    {
		mensajeInteraccion = "";
    }

    void Start()
	{
		fade = GameObject.Find("Fade").GetComponent<Animator>();
	}

    public IEnumerator interact()
	{
		if(isActiveAndEnabled)
		{
			fade.SetTrigger("Fade");
			mensajeInteraccion = "";
			yield return new WaitForSeconds(1.5f);
			SceneManager.LoadScene("Gasolinera"); 
        }
	}

	public string MensajeInteraccion()
	{
		return mensajeInteraccion;
	}
}
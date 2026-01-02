using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{

    [SerializeField] private string escenaACargar;

    private Animator fade;

    void Start()
	{
		fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
	}

    public void Cargar()
    {
        StartCoroutine(CargarCoroutine());
    }

    private IEnumerator CargarCoroutine()
    {
        fade.SetTrigger("Fade");
		yield return new WaitForSeconds(1.7f);
		SceneManager.LoadScene(escenaACargar); 
    }
}

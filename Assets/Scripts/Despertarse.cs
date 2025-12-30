using UnityEngine;
using System.Collections;

public class Despertarse : MonoBehaviour
{
    [SerializeField] private Transform jugador; 
    [SerializeField] private Transform puntoDestino;
    [SerializeField] private Renderer pantallaNegra;
    [SerializeField] private AudioSource MantaCama;

    void Start()
    {
        CameraMovement.Instance.enabled = false;
    }

    public void Levantarse() 
    { 
        StartCoroutine(ProcesoLevantarse()); 
    }

    public IEnumerator ProcesoLevantarse()
    {
        pantallaNegra.gameObject.SetActive(true);
        Material mat = pantallaNegra.material; 
        MantaCama.Play();

        for (float a = 0; a <= 1; a += Time.deltaTime) 
        { 
            mat.color = new Color(0, 0, 0, a); 
            yield return null; 
        }

        jugador.position = puntoDestino.position;

        for (float a = 1; a >= 0; a -= Time.deltaTime) 
        { 
            mat.color = new Color(0, 0, 0, a); 
            yield return null;
        }
        MantaCama.Stop();
        //yield return new WaitForSeconds(2f);
        pantallaNegra.gameObject.SetActive(false);
        
        HeadbobSystem.Instance.enabled = true; 
        PlayerMovement.Instance.enabled = true;
        CameraMovement.Instance.lockY = false;
        CameraMovement.Instance.enabled = true;
    }
}

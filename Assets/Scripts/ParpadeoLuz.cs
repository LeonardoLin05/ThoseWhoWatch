using UnityEngine;
using System.Collections;

public class ParpadeoLuz : MonoBehaviour
{
    public Light luz;
    public Renderer bombilla;           
    public float minIntensidad = 0.2f;  
    public float maxIntensidad = 1.5f;  
    public float velocidad = 0.1f;  

    void Start()
    {
        luz = GetComponent<Light>();
        StartCoroutine(Parpadeo());
    }

    IEnumerator Parpadeo()
    {
        while (true)
        {
            /*
            luz.intensity = Random.Range(minIntensidad, maxIntensidad);

            
            if (Random.value < 0.1f)
            {
                bombilla.material.SetColor("_Color" ,new Color(90f / 255f, 70f / 255f, 30f / 255f)); // No funciona
                luz.enabled = false;
                yield return new WaitForSeconds(1f); 
            }
            else
            {
                bombilla.material.SetColor("_Color" , new Color(205f / 255f, 170f / 255f, 69f / 255f)); // no funciona
                luz.enabled = true;
                yield return new WaitForSeconds(velocidad); 
            }
            */
            luz.enabled = true;
            luz.intensity = maxIntensidad;

            if (Random.value < 0.2f)
            {
                for(int i = 0; i < 5; i++)
                {
                     luz.intensity = Random.Range(minIntensidad, maxIntensidad);
                }
            }
            if (Random.value < 0.2f)
            {
                luz.enabled = false;
                yield return new WaitForSeconds(0.1f); 
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
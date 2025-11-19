using UnityEngine;

public class Pajaro : MonoBehaviour
{
    /*
    [SerializeField] private GameObject grupoPajaros;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < grupoPajaros.transform.childCount; i++)
        {
            GameObject pajaro = grupoPajaros.transform.GetChild(i).gameObject;
            pajaro.SetActive(false);
        }
    }

    public void ActivarAnimacionEnGrupo()
    {
        for (int i = 0; i < grupoPajaros.transform.childCount; i++)
        {
            Debug.Log("HOLAAAA");
            GameObject pajaro = grupoPajaros.transform.GetChild(i).gameObject;
            pajaro.SetActive(true);
            Debug.Log(pajaro.activeSelf);
            Animator anim = pajaro.GetComponent<Animator>();
            Debug.Log("animaciones es:" + anim);
            anim.SetTrigger("Fly");
        }
    }
    */

    [SerializeField] private GameObject PajaroObjeto;
    private Animator pajaroAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pajaroAnimation = PajaroObjeto.GetComponent<Animator>();
        PajaroObjeto.SetActive(false);
    }

    public void Spawn()
    {
        if (!PajaroObjeto.activeSelf)
        {
            PajaroObjeto.SetActive(true);
        }
        pajaroAnimation.SetTrigger("Fly");
    }
}

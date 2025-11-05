using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCReact : MonoBehaviour
{
    // Una o más frases que dice el NPC al reaccionar
    [SerializeField] private string[] frase;
    // El NPC que va a reaccionar
    [SerializeField] private Transform NPC;
    // Fondo dialogo
    [SerializeField] private Image fondoDialogo;
    // Texto del dialogo
    [SerializeField] private TextMeshProUGUI texto;
    // Boton de respuesta/continuar
    [SerializeField] private Button boton;

    // Lleva la cuenta de cuantas veces se ha ejecutado el script
    // para ir aumentando a la siguiente frase que se vaya a decir
    // Ejemplo funcionalidad: para que vaya diciendo frases cada vez más enfadado
    private static int vecesEjecutadas = 0;

    void OnCollisionEnter(Collision collision)
    {
        // Solo se puede realizar si las interacciones estan activadas
        if(Interaction.Instance.isActiveAndEnabled)
        {
            if (collision.gameObject.CompareTag("SueloGasolinera"))
            {
                StartConversation();
            } 
        }
    }

    private void StartConversation()
    {
        // Bloqueamos movimiento de la camara, jugador e interaccion
        ActivarInstances(false);

        // Desbloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Forzamos al jugar a mirar al NPC
        TalkZoomMoveCamera.Instance.setCabeza(NPC);
        TalkZoomMoveCamera.Instance.StartZoomMovement(150f);

        texto.gameObject.SetActive(true);

        // Para que el texto aparezca de poco a poco animado
        StartCoroutine(TextoAnimado(frase[vecesEjecutadas]));

        if(vecesEjecutadas < frase.Length - 1)
        {
            vecesEjecutadas++;
        }

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(() => StopConversation());

    }

    private void StopConversation()
    {
        // Desbloqueamos movimiento de la camera, jugador e interaccion
        ActivarInstances(true);

        // Bloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        boton.gameObject.SetActive(false);

        texto.text = "";
        texto.gameObject.SetActive(false);

        // Liberamos al jugador de mirar al NPC
        TalkZoomMoveCamera.Instance.StopZoomMovement();
    }

    private IEnumerator TextoAnimado(string frase)
    {
        texto.text = "";

        foreach (char letra in frase)
        {
            texto.text += letra;
            yield return new WaitForSecondsRealtime(0.04f);
        }
        boton.gameObject.SetActive(true);
    }
    
    private void ActivarInstances(bool activar)
    {
        PlayerMovement.Instance.enabled = activar;
        CameraMovement.Instance.enabled = activar;
        HeadbobSystem.Instance.enabled = activar;
        Interaction.Instance.enabled = activar;
        Zoom.Instance.enabled = activar;
    }
}

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
    // Texto del dialogo
    [SerializeField] private TextMeshProUGUI texto;
    // Boton de respuesta/continuar
    [SerializeField] private Button boton;

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
        // Bloqueamos movimiento de la camere, jugador e interaccion
        PlayerMovement.Instance.enabled = false;
        CameraMovement.Instance.enabled = false;
        HeadbobSystem.Instance.enabled = false;
        Interaction.Instance.enabled = false;

        // Desbloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Forzamos al jugar a mirar al NPC
        TalkZoomMoveCamera.Instance.setCabeza(NPC);
        TalkZoomMoveCamera.Instance.StartZoomMovement(true);

        texto.gameObject.SetActive(true);

        // Para que el texto aparezca de poco a poco animado
        StartCoroutine(TextoAnimado(frase[Random.Range(0, frase.Length)]));

        // Configuraciones del boton
        boton.gameObject.SetActive(true);
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(() => StopConversation());

    }

    private void StopConversation()
    {
        // Desbloqueamos movimiento de la camera, jugador e interaccion
        PlayerMovement.Instance.enabled = true;
        CameraMovement.Instance.enabled = true;
        HeadbobSystem.Instance.enabled = true;
        Interaction.Instance.enabled = true;

        // Bloqueamos el cursor del ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Configuraciones del boton
        boton.onClick.RemoveAllListeners();
        boton.gameObject.SetActive(false);

        texto.text = "";
        texto.gameObject.SetActive(false);

        // Liberamos al jugador de mirar al NPC
        TalkZoomMoveCamera.Instance.StartZoomMovement(false);
    }

    private IEnumerator TextoAnimado(string frase)
    {
        texto.text = "";

        for (int i = 0; i < frase.Length; i++)
        {
            texto.text = texto.text + frase[i];
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }
}

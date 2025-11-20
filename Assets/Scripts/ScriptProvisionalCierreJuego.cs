using System.Collections;
using UnityEngine;

public class ScriptProvisionalCierreJuego : MonoBehaviour, IInteractable
{
    public void interact()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public string MensajeInteraccion()
    {
        return "[E] para terminar nivel de prueba";
    }
}

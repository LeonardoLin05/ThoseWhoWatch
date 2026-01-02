using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryMenu : MonoBehaviour
{
    [SerializeField] private GameObject retryMenuUI;
    [SerializeField] private GameObject skipButton;

    private static int tries = 0;

    public static bool ZONA_PERSECUCION = false;

    void Awake()
    {
        ZONA_PERSECUCION = false;
    }

    public void ShowMenu()
    {
        // Desbloquamos el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        GamePause.Instance.enabled = false;
        Time.timeScale = 0f;
        retryMenuUI.SetActive(true);
        if(tries == 3)
        {
            skipButton.SetActive(true);
        }
    }

    public void Retry()
    {
        if(ZONA_PERSECUCION && tries < 3)
        {
            tries++;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("CasaNoche");
    }

    public void Skip()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Creditos");
    }
}

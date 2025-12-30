using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{

    public static GamePause Instance { get; private set; }

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] Slider sliderX;
    [SerializeField] Slider sliderY;

    [SerializeField] TMP_InputField inputX;
    [SerializeField] TMP_InputField inputY;

    private int layer = 0;

    private bool isPaused = false;

    void Awake()
    {
        sliderX.value = CameraMovement.mouseSensitivityX;
        sliderY.value = CameraMovement.mouseSensitivityY;
        inputX.text = sliderX.value.ToString();
        inputY.text = sliderY.value.ToString();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if(layer != 1 && Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {   
                Resume();
            }
            else
            {
                Debug.Log("Pausing game");
                Pause();
            }
        }
    }

    public void SetMouseSensitivityX(float value)
    {
        CameraMovement.mouseSensitivityX = value;
    }

    public void SetMouseSensitivityY(float value)
    {
        CameraMovement.mouseSensitivityY = value;
    }

    public void SynchronizeInputX(float value)
    {
        inputX.text = value.ToString();
    }

    public void SynchronizeInputY(float value)
    {
        inputY.text = value.ToString();
    }

    public void SynchronizeSliderX(string value)
    {
        if(value != "") sliderX.value = float.Parse(value);
    }

    public void SynchronizeSliderY(string value)
    {
        if(value != "") sliderY.value = float.Parse(value);
    }

    public void CheckInputX(string value)
    {
        if(value == "") inputX.text = "0";
    }

    public void CheckInputY(string value)
    {
        if(value == "") inputY.text = "0";
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Desbloquamos el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Bloqueamos el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Options()
    {
        optionsMenuUI.SetActive(true);
        layer++;
    }

    public void ReturnFromOptions()
    {
        optionsMenuUI.SetActive(false);
        layer--;
    }

    public bool GetPauseState()
    {
        return isPaused;
    }
}

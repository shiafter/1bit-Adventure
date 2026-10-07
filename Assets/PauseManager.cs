using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject settingsCanvas;
    public InputActionReference pauseAction;

    private bool paused = false;
    private bool isSetting = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPause;
    }
    private void OnDisable()
    {
        pauseAction.action.Disable();
        pauseAction.action.performed -= OnPause;
    }
    private void Start()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        TogglePause();
    }
    public void TogglePause()
    {
        paused = !paused;

        pauseCanvas.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }
    public void ToggleSettings()
    {
        isSetting = !isSetting;
        settingsCanvas.SetActive(isSetting);
    }

}

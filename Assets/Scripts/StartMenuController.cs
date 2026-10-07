using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject settingsCanvas;
    private void Start()
    {
        settingsCanvas.SetActive(false);
    }
    public void OnStartClick()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void OnSettingsClick()
    {
        settingsCanvas.SetActive(true);
    }
    public void BackToStartScene()
    {
        settingsCanvas.SetActive(false);
    }
}

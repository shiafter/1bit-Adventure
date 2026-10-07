using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    int progressAmount;
    public Slider progressSlider;
    public TMP_Text gemCollectedTxt;
    public TMP_Text levelCompletedTxt;
    public bool levelCompleted;
    public int lastPlayingLevel;

    public GameObject player;
    public GameObject notiIcon;
    public GameObject gameOverCanvas;

    public static event Action OnLevelReset;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;

        Gem.OnGemCollect += LevelProgress;
        PlayerHealth.OnPlayerDead += GameOverScreen;

        lastPlayingLevel = SceneManager.GetActiveScene().buildIndex;

        levelCompleted = false;
        levelCompletedTxt.enabled = false;
        gameOverCanvas.SetActive(false);
    }
    private void OnDestroy()
    {
        Gem.OnGemCollect -= LevelProgress;
        PlayerHealth.OnPlayerDead -= GameOverScreen;
    }
    void LevelProgress(int amount)
    {
        progressAmount += amount;
        gemCollectedTxt.text = "Gem Collected: " + progressAmount + "/10";
        Debug.Log("Gem Collected: " + progressAmount + "/10");
        progressSlider.value = progressAmount;

        if(progressAmount >= 10)
        {
            levelCompleted = true;
            levelCompletedTxt.enabled = true;
            notiIcon.SetActive(true);
        }
    }
    public void GoToNextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
    void GameOverScreen()
    {
        gameOverCanvas.SetActive(true);
        MusicManager.StopBackgroundMusic();
        Time.timeScale = 0;
    }
    public void ResetLevel()
    {
        gameOverCanvas.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        Time.timeScale = 1;
        
        OnLevelReset?.Invoke();

        //reset game progress
        progressAmount = 0;
        gemCollectedTxt.text = "Gem Collected: " + progressAmount + "/10";
        progressSlider.value = 0;
        levelCompletedTxt.enabled = false;
        notiIcon.SetActive(false);
    }
}

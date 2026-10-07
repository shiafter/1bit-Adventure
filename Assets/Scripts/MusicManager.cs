using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider bgmSlider;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
        bgmSlider.onValueChanged.AddListener(delegate { SetVolume(bgmSlider.value); });
    }
    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }
    public static float GetVolume()
    {
        return instance.audioSource.volume;
    }
    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if(audioClip != null)
        {
            instance.audioSource.clip = audioClip;
        }
        if(instance.audioSource != null)
        {
            if (resetSong)
            {
                instance.audioSource.Stop();
            }
            instance.audioSource.Play();
        }
    }
    public static void StopBackgroundMusic()
    {
        instance.audioSource.Pause();
    }
}

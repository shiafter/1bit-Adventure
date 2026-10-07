using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        bgmSlider.value = MusicManager.GetVolume();
        sfxSlider.value = SoundEffectManager.GetVolume();

        bgmSlider.onValueChanged.AddListener(MusicManager.SetVolume);
        sfxSlider.onValueChanged.AddListener(SoundEffectManager.SetVolume);
    }

    private void OnDestroy()
    {
        if (MusicManager.instance != null && SoundEffectManager.instance != null)
        {
            bgmSlider.onValueChanged.RemoveListener(MusicManager.SetVolume);
            sfxSlider.onValueChanged.RemoveListener(SoundEffectManager.SetVolume);
        }
    }
}

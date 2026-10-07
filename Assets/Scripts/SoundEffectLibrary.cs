using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    [System.Serializable]
    public struct SoundEffectGroup
    {
        public string name;
        public List<AudioClip> clip;
    }
    private void Awake()
    {
        InitiallizeDictionary();
    }

    private void InitiallizeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(SoundEffectGroup soundeffectGroup in soundEffectGroups)
        {
            soundDictionary[soundeffectGroup.name] = soundeffectGroup.clip;
        }
    }
    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if(audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.Audio;


public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;

    void Start()
    {
        AudioMixer.SetFloat("MasterVolume", 1f);   
        AudioMixer.SetFloat("MusicVolume", 1f);
        AudioMixer.SetFloat("SoundFXVolume", 1f);
    }
    public void SetMasterVolume (float level)
    {
        AudioMixer.SetFloat("MasterVolume", 0f);
    }
    public void SetSoundFXVolume(float level)
    {
        AudioMixer.SetFloat("SoundFXVolume", 0f);
    }
    public void SetMusicVolume(float level)
    {
        AudioMixer.SetFloat("MusicVolume", 0f);
    }
}

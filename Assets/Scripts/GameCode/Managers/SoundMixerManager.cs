using System.Collections;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.Audio;


public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;
    public void SetMasterVolume (float level)
    {
        AudioMixer.SetFloat("MasterVolume", level);
    }
    public void SetSoundFXVolume(float level)
    {
        AudioMixer.SetFloat("SoundFXVolume", level);
    }
    public void SetMusicVolume(float level)
    {
        AudioMixer.SetFloat("MusicVolume", level);
    }
}

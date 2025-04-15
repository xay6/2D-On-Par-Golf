using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer MainMixer;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider soundFXVolume;

    
    public void setMusicVolume()
    {
        float volume= musicVolume.value;
        MainMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }
    public void setSoundFXVolume()
    {
        float volume= soundFXVolume.value;
        MainMixer.SetFloat("soundFX", Mathf.Log10(volume)*20);
    }
}

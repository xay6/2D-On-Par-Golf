using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Range(0f, 1f)]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float soundfxVolume = 1f;

    [Header("Audio Mixer Reference")]
    [SerializeField] private AudioMixer audioMixer;
    private void Awake()
    {
        Instance = this; 
        LoadSettings();
    }

     public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        soundfxVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(soundfxVolume) * 20);
        PlayerPrefs.SetFloat("SoundFXVolume", soundfxVolume);
    }

    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume  = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundfxVolume    = PlayerPrefs.GetFloat("SoundFXVolume", 1f);

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(soundfxVolume) * 20);
    }

}
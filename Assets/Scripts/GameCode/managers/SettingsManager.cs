using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Range(0f, 1f)]
    public float soundFXvolume = 1f;
    public float musicVolume = 1f;

    private void Awake()
    {
        Instance = this; 
        LoadSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("music", musicVolume);
        PlayerPrefs.Save();
    }
    public void SetSoundFXVolume(float volume)
    {
        soundFXvolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundFXVolume", soundFXvolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = musicVolume;
        soundFXvolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = soundFXvolume;
    }
}
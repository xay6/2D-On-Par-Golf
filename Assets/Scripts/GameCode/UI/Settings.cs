using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingsPanel;
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SoundFXVolumeSlider;
    public Button backToMenuButton;

    private void OnEnable()
    {
        // Always hide the panel when the object becomes active again
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    private void Start()
    {
        // Initialize settings panel
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Set UI values from SettingsManager
        MasterVolumeSlider.value = SettingsManager.Instance.masterVolume;
        MusicVolumeSlider.value  = SettingsManager.Instance.musicVolume;
        SoundFXVolumeSlider.value = SettingsManager.Instance.soundfxVolume;
    

        // Hook up listeners
        MasterVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMasterVolume);
        MusicVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMusicVolume);
        SoundFXVolumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetSFXVolume);
        backToMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            ToggleSettingsPanel();
        }
    }

    
    public void ToggleSettingsPanel()
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("Settings panel not assigned!");
            return;
        }
        Debug.Log("ToggleSettingsPanel called!");
        bool isActive = !settingsPanel.activeSelf; 
        settingsPanel.SetActive(isActive);         
        Time.timeScale = isActive ? 0f : 1f;
    }
    public void testClick()
    {
        Debug.Log("test click");
    }

    public void BackToMainMenu()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        
        Time.timeScale = 1; 
        SceneManager.LoadScene("MetagameScene"); 
    }
}
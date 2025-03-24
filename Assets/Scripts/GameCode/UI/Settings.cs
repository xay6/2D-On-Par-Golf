using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Button backToMenuButton;

    private void Start()
    {
        // Initialize settings panel
        settingsPanel.SetActive(false);

        // Set UI values from SettingsManager
        volumeSlider.value = SettingsManager.Instance.masterVolume;
    

        // Hook up listeners
        volumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetVolume);
        backToMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void Update()
    {
        // Press ESC to toggle settings
        /*if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleSettingsPanel();
        }*/
    }

    
    public void ToggleSettingsPanel()
    {
        bool isActive = !settingsPanel.activeSelf; // Determine what the new state will be
        settingsPanel.SetActive(isActive);         // Apply that new state
        Time.timeScale = isActive ? 0f : 1f;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1; // Unpause before switching scenes
        SceneManager.LoadScene("MetagameScene"); // Make sure this name matches exactly
    }
}
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
        bool isActive = !settingsPanel.activeSelf; 
        settingsPanel.SetActive(isActive);         
        Time.timeScale = isActive ? 0f : 1f;
    }

    public void BackToMainMenu()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        
        Time.timeScale = 1; 
        SceneManager.LoadScene("MetagameScene"); 
    }
}
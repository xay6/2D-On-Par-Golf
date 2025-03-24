using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenuButton : MonoBehaviour
{
    public Button backButton; // Assign this in the inspector

    void Start()
    {
        backButton.onClick.AddListener(BackToMainMenu);
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene("MetagameScene"); // make sure this matches your main menu scene name
    }
}
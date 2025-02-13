using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public GameObject loginPanel;
    public GameObject gameUI;
    
    private const string PLAYER_KEY = "CurrentPlayer";

    void Start()
    {
        string savedPlayer = PlayerPrefs.GetString(PLAYER_KEY, "");
        if (!string.IsNullOrEmpty(savedPlayer))
        {
            // Auto-login last user
            LoadGame(savedPlayer);
        }
        else
        {
            loginPanel.SetActive(true);
            gameUI.SetActive(false);
        }
    }

    public void Login()
    {
        string username = usernameInput.text.Trim();
        if (!string.IsNullOrEmpty(username))
        {
            PlayerPrefs.SetString(PLAYER_KEY, username);
            PlayerPrefs.Save();
            LoadGame(username);
        }
    }

    private void LoadGame(string username)
    {
        Debug.Log($"Logged in as: {username}");
        loginPanel.SetActive(false);
        gameUI.SetActive(true);
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey(PLAYER_KEY);
        loginPanel.SetActive(true);
        gameUI.SetActive(false);
    }

    public string GetCurrentPlayer()
    {
        return PlayerPrefs.GetString(PLAYER_KEY, "Guest");
    }
}
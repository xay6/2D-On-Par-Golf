using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [SerializeField] private LoginView loginView;

    private void Awake()
    {
        loginView.OnLogin += HandleLogin;
        loginView.OnBack += HandleBack;
    }

    private void HandleLogin(string username)
    {
        if (PlayerPrefs.GetString("Username") == username)
        {
            SceneManager.LoadScene("GameScene"); // Replace with your actual game scene
        }
        else
        {
            Debug.Log("Username not found!");
        }
    }

    private void HandleBack()
    {
        SceneManager.LoadScene("MetagameScene");
    }
}

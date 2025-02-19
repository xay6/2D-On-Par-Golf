using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpController : MonoBehaviour
{
    [SerializeField] private SignUpView signUpView;

    private void Awake()
    {
        signUpView.OnStartGame += HandleStartGame;
        signUpView.OnBack += HandleBack;
    }

    private void HandleStartGame(string username)
    {
        PlayerPrefs.SetString("Username", username);
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene
    }

    private void HandleBack()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

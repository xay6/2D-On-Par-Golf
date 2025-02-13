using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Change to your game scene name
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene"); // Create this if needed
    }

    public void ExitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
